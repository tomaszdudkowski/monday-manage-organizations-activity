using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
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
                .FirstOrDefaultAsync(m => m.EmployeeID == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [Authorize(Roles = "Admin")]
        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "DepartmentID");
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ProjectID", "ProjectID");
            return View();
        }

        [Authorize(Roles = "Admin")]
        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeID,EmployeeName,EmployeeSurname,EmployeeDateOfBirth,EmployeePhoneNumber,EmployeeRole,DepartmentID,ProjectID,IsEdited,IsChecked")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "DepartmentID", employee.DepartmentID);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ProjectID", "ProjectID", employee.ProjectID);
            return View(employee);
        }

        [Authorize(Roles = "Admin")]
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
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "DepartmentID", employee.DepartmentID);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ProjectID", "ProjectID", employee.ProjectID);
            return View(employee);
        }

        [Authorize(Roles = "Admin")]
        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeID,EmployeeName,EmployeeSurname,EmployeeDateOfBirth,EmployeePhoneNumber,EmployeeRole,DepartmentID,ProjectID,IsEdited,IsChecked")] Employee employee)
        {
            if (id != employee.EmployeeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeID))
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
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "DepartmentID", employee.DepartmentID);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ProjectID", "ProjectID", employee.ProjectID);
            return View(employee);
        }

        [Authorize(Roles = "Admin")]
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
                .FirstOrDefaultAsync(m => m.EmployeeID == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [Authorize(Roles = "Admin")]
        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeID == id);
        }
    }
}
