using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mondayWebApp.Data;
using mondayWebApp.Models;

namespace mondayWebApp.Controllers
{
    public class ProjectTasksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<IdentityUser> userManager;

        public ProjectTasksController(ApplicationDbContext context, UserManager<IdentityUser> userMgr)
        {
            _context = context;
            userManager = userMgr;
        }

        // GET: ProjectTasks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ProjectTasks.Include(p => p.Employee).Include(p => p.Project);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ProjectTasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectTask = await _context.ProjectTasks
                .Include(p => p.Employee)
                .Include(p => p.Project)
                .FirstOrDefaultAsync(m => m.TaskID == id);
            if (projectTask == null)
            {
                return NotFound();
            }

            return View(projectTask);
        }

        // GET: ProjectTasks/Create
        public IActionResult Create()
        {
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "EmployeeNameSurname");
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ProjectID", "ProjectName");
            return View();
        }

        // POST: ProjectTasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TaskID,TaskName,TaskDeadline,ProjectID,EmployeeID,IsEdited,IsChecked")] ProjectTask projectTask)
        {
            if (ModelState.IsValid)
            {
                if (!(User.IsInRole("Superadmin")))
                {
                    var user = await userManager.FindByNameAsync(User.Identity.Name);
                    var employeeUser = _context.Employees.Where(e => e.EmployeeUserID == user.Id);
                    Employee userU = null;
                    foreach (var item in employeeUser)
                    {
                        userU = item;
                    }
                    projectTask.TaskCreatedBy = userU.EmployeeID;
                }
                _context.Add(projectTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "EmployeeName", projectTask.EmployeeID);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ProjectID", "ProjectName", projectTask.ProjectID);
            return View(projectTask);
        }

        // GET: ProjectTasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectTask = await _context.ProjectTasks.FindAsync(id);
            if (projectTask == null)
            {
                return NotFound();
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "EmployeeNameSurname", projectTask.EmployeeID);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ProjectID", "ProjectName", projectTask.ProjectID);
            return View(projectTask);
        }

        // POST: ProjectTasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TaskID,TaskName,TaskDeadline,ProjectID,EmployeeID,IsEdited,IsChecked")] ProjectTask projectTask)
        {
            if (id != projectTask.TaskID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (!(User.IsInRole("Superadmin")))
                    {
                        var user = await userManager.FindByNameAsync(User.Identity.Name);
                        var employeeUser = _context.Employees.Where(e => e.EmployeeUserID == user.Id);
                        Employee userU = null;
                        foreach (var item in employeeUser)
                        {
                            userU = item;
                        }
                        projectTask.TaskCreatedBy = userU.EmployeeID;
                    }
                    _context.Update(projectTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectTaskExists(projectTask.TaskID))
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
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "EmployeeNameSurname", projectTask.EmployeeID);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ProjectID", "ProjectName", projectTask.ProjectID);
            return View(projectTask);
        }

        // GET: ProjectTasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectTask = await _context.ProjectTasks
                .Include(p => p.Employee)
                .Include(p => p.Project)
                .FirstOrDefaultAsync(m => m.TaskID == id);
            if (projectTask == null)
            {
                return NotFound();
            }

            return View(projectTask);
        }

        // POST: ProjectTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projectTask = await _context.ProjectTasks.FindAsync(id);
            _context.ProjectTasks.Remove(projectTask);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectTaskExists(int id)
        {
            return _context.ProjectTasks.Any(e => e.TaskID == id);
        }
    }
}
