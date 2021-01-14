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
    [Authorize(Roles = "Superadmin, Admin")]
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Projects.Include(p => p.ProjectManager);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.ProjectManager)
                .FirstOrDefaultAsync(m => m.ProjectID == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            var emplAdminList = _context.Employees.Where(e => e.EmployeeRole == "Admin").Select(e => e);
            ViewData["ProjectManagerID"] = new SelectList(emplAdminList, "EmployeeID", "EmployeeNameSurname");
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectID,ProjectName,ProjectDesc,ProjectBrief,ProjectDeadline,ProjectManagerID,IsEdited,IsChecked")] Project project)
        {
            if (ModelState.IsValid)
            {
                bool isProjectManagerInDB = _context.Projects.Where(p => p.ProjectManagerID == project.ProjectManagerID).Any();
                if (isProjectManagerInDB == true)
                {
                    return RedirectToAction("Error", "Home");
                }
                _context.Add(project);
                await _context.SaveChangesAsync();
                var employeeManager = _context.Employees.Where(e => e.ProjectManager.ProjectManagerID == project.ProjectManagerID).Single();
                employeeManager.IsProjectManager = true;
                employeeManager.ProjectID = project.ProjectID;
                _context.Employees.Update(employeeManager);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var emplAdminList = _context.Employees.Where(e => e.EmployeeRole == "Admin").Select(e => e);
            ViewData["ProjectManagerID"] = new SelectList(emplAdminList, "EmployeeID", "EmployeeNameSurname");
            return View(project);
        }

        private static int? TempBox { get; set; }
        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            TempBox = project.ProjectManagerID;
            if (project == null)
            {
                return NotFound();
            }

            var emplAdminList = _context.Employees.Where(e => e.EmployeeRole == "Admin").Select(e => e);
            ViewData["ProjectManagerID"] = new SelectList(emplAdminList, "EmployeeID", "EmployeeNameSurname");
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectID,ProjectName,ProjectDesc,ProjectBrief,ProjectDeadline,ProjectManagerID,IsEdited,IsChecked")] Project project)
        {
            if (id != project.ProjectID)
            {
                return NotFound();
            }

            if(project.ProjectManagerID == null)
            {
                var oldEmployeeManager = _context.Employees.Where(e => e.EmployeeID == TempBox).Single();
                oldEmployeeManager.IsProjectManager = false;
                _context.Employees.Update(oldEmployeeManager);
                await _context.SaveChangesAsync();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                    var employeeManager = _context.Employees.Where(e => e.ProjectManager.ProjectManagerID == project.ProjectManagerID).Single();
                    employeeManager.IsProjectManager = true;
                    employeeManager.ProjectID = project.ProjectID;
                    _context.Employees.Update(employeeManager);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.ProjectID))
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
            var emplAdminList = _context.Employees.Where(e => e.EmployeeRole == "Admin").Select(e => e);
            ViewData["ProjectManagerID"] = new SelectList(emplAdminList, "EmployeeID", "EmployeeNameSurname");
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.ProjectManager)
                .FirstOrDefaultAsync(m => m.ProjectID == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            var employeeManager = _context.Employees.Where(e => e.ProjectManager.ProjectManagerID == project.ProjectManagerID).Single();
            employeeManager.IsProjectManager = false;
            _context.Employees.Update(employeeManager);
            await _context.SaveChangesAsync();
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.ProjectID == id);
        }
    }
}
