using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mondayWebApp.Data;
using mondayWebApp.Models;

namespace mondayWebApp.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private RoleManager<IdentityRole> roleManager;
        private UserManager<IdentityUser> userManager;

        public EmployeesController(ApplicationDbContext context, RoleManager<IdentityRole> roleMgr, UserManager<IdentityUser> userMgr)
        {
            _context = context;
            roleManager = roleMgr;
            userManager = userMgr;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Employees.Include(e => e.Department).Include(e => e.Project);
            return View(await applicationDbContext.ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Project)
                .FirstOrDefaultAsync(m => m.EmployeeID.Equals(id));
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [Authorize(Roles = "Superadmin, Admin")]
        // GET: Employees/Create
        public async Task<IActionResult> CreateAsync()
        {
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "DepartmentName");
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ProjectID", "ProjectName");
            List<IdentityRole> roleList = new List<IdentityRole>();
            foreach (var item in _context.Roles)
            {
                roleList.Add(item);
            }
            if (User.IsInRole("Admin") && (!User.IsInRole("Superadmin")))
            {
                IdentityRole superadminRole = await roleManager.FindByNameAsync("Superadmin");
                roleList.Remove(superadminRole);
            }
            SelectList roleItems = new SelectList(roleList, "Id", "Name");
            ViewData["EmployeeRole"] = roleItems;
            return View();
        }

        [Authorize(Roles = "Superadmin, Admin")]
        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeID,EmployeeUserID,EmployeePassword,EmployeeEmail,EmployeeName,EmployeeSurname,EmployeeDateOfBirth,EmployeePhoneNumber,EmployeeRole,DepartmentID,ProjectID,IsEdited,IsChecked,IsKierownik")] Employee employee)
        {

            var user = new IdentityUser();
            user.UserName = employee.EmployeeEmail;
            user.Email = employee.EmployeeEmail;
            user.EmailConfirmed = true;

            string UserPassword = employee.EmployeePassword;

            IdentityResult identityResult = await userManager.CreateAsync(user, UserPassword);

            if (identityResult.Succeeded)
            {
                var roleId = Request.Form["EmployeeRole"];
                var role = await roleManager.FindByIdAsync(roleId);
                var result = await userManager.AddToRoleAsync(user, role.Name);
                var TempUser = await userManager.FindByEmailAsync(user.Email);
                employee.EmployeeUserID = TempUser.Id;
                employee.EmployeeNameSurname = employee.EmployeeName + " " + employee.EmployeeSurname;
                employee.EmployeePassword = "";
                employee.EmployeeRole.Add(role);
                await _context.AddAsync(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }



            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "DepartmentName", employee.DepartmentID);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ProjectID", "ProjectName", employee.ProjectID);
            ViewData["EmployeeRole"] = new SelectList(_context.Roles, "Id", "Name", employee.EmployeeRole);
            return View(employee);
        }

        [Authorize(Roles = "Superadmin, Admin")]
        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "DepartmentName", employee.DepartmentID);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ProjectID", "ProjectName", employee.ProjectID);
            List<IdentityRole> roleList = new List<IdentityRole>();
            foreach (var item in _context.Roles)
            {
                roleList.Add(item);
            }
            if (User.IsInRole("Admin") && (!User.IsInRole("Superadmin")))
            {
                IdentityRole superadminRole = await roleManager.FindByNameAsync("Superadmin");
                roleList.Remove(superadminRole);
            }
            var user = await userManager.FindByIdAsync(employee.EmployeeUserID);
            var userRole = _context.UserRoles.ToList();
            string role = "";
            foreach (var item in userRole)
            {
                if (item.UserId == user.Id)
                {
                    role = item.RoleId;
                }
            }
            var role1 = roleManager.FindByNameAsync(role);
            SelectList roleItems = new SelectList(roleList, "Id", "Name", role1);
            ViewData["EmployeeRole"] = roleItems;
            return View(employee);
        }

        [Authorize(Roles = "Superadmin, Admin")]
        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("EmployeeID,EmployeePassword,EmployeeCurrentPassword,EmployeeEmail,EmployeeName,EmployeeSurname,EmployeeDateOfBirth,EmployeePhoneNumber,EmployeeRole,DepartmentID,ProjectID,IsEdited,IsChecked,IsKierownik,EmployeeUserID")] Employee employee)
        {

            if (id.Equals(employee.EmployeeID))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await userManager.FindByIdAsync(employee.EmployeeUserID);

                    if (employee.EmployeeEmail != null)
                    {
                        user.UserName = employee.EmployeeEmail;
                    }

                    if (employee.EmployeePassword != null)
                    {
                        string NewUserPassword = employee.EmployeePassword;
                        string CurrentUserPassword = employee.EmployeeCurrentPassword;

                        await userManager.ChangePasswordAsync(user, CurrentUserPassword, NewUserPassword);
                    }

                    var roleId = Request.Form["EmployeeRole"];
                    if (roleId == Request.Form["EmployeeRole"])
                    {
                        var role = await roleManager.FindByIdAsync(roleId);
                        var result = await userManager.AddToRoleAsync(user, role.Name);
                        employee.EmployeePassword = "";
                        _context.Update(employee);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeUserID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "DepartmentName", employee.DepartmentID);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ProjectID", "ProjectName", employee.ProjectID);
            return View(employee);
        }

        [Authorize(Roles = "Admin, Superadmin")]
        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Project)
                .FirstOrDefaultAsync(m => m.EmployeeID.Equals(id));
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [Authorize(Roles = "Admin, Superadmin")]
        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            var user = await userManager.FindByIdAsync(employee.EmployeeUserID);
            IdentityResult identityResult = await userManager.DeleteAsync(user);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(string id)
        {
            return _context.Employees.Any(e => e.EmployeeID.Equals(id));
        }
    }
}
