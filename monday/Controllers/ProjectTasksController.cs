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
    /// <summary>
    /// Kontroler zadań.
    /// </summary>
    public class ProjectTasksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<IdentityUser> userManager;

        public ProjectTasksController(ApplicationDbContext context, UserManager<IdentityUser> userMgr)
        {
            _context = context;
            userManager = userMgr;
        }

        /// <summary>
        /// Akcja pobierająca zadania z bazy danych.
        /// </summary>
        /// <returns>Widok z listą zadań i pracowników przypisanych do zadań.</returns>
        // GET: ProjectTasks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ProjectTasks.Include(p => p.Employee).Include(p => p.Project);
            return View(await applicationDbContext.ToListAsync());
        }

        /// <summary>
        /// Akcja pobierająca szczegółowe dane o wybranym zadaniu z bazy danych.
        /// </summary>
        /// <param name="id">Numer ID wybranego zadania.</param>
        /// <returns>Widok z szczegółowymi danymi zadania.</returns>
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

        /// <summary>
        /// Akcja przygotowująca formularz do dodania nowego zadania.
        /// </summary>
        /// <returns>Widok z formularzem.</returns>
        // GET: ProjectTasks/Create
        public IActionResult Create()
        {
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "EmployeeNameSurname");
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ProjectID", "ProjectName");
            return View();
        }

        /// <summary>
        /// Akcja dodająca nowe zadanie do bazy danych.
        /// Walidacja formularza.
        /// </summary>
        /// <param name="projectTask">Dane zadania z formularza.</param>
        /// <returns>Widok z listą zadań.</returns>
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

        /// <summary>
        /// Akcja przygotowująca widok z formularzem edycji wybranego zadania.
        /// </summary>
        /// <param name="id">Numer ID wybranego zadania.</param>
        /// <returns>Widok z formularzem edycji wybranego zadania.</returns>
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

        /// <summary>
        /// Akcja edytująca wybrane zadanie.
        /// Zmiany zapisuje w bazie danych.
        /// </summary>
        /// <param name="id">Numer ID edytowanego zadania.</param>
        /// <param name="projectTask">Wybrane zadanie do edycji. Dane z formularza.</param>
        /// <returns>Widok listy z zadaniami.</returns>
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

        /// <summary>
        /// Akcja przygotowująca widok podsumowania wybranego do usunięcia zadania.
        /// </summary>
        /// <param name="id">Numer ID wybranego zadania.</param>
        /// <returns>Widok podsumowania wybranego do usunięcia zadania.</returns>
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

        /// <summary>
        /// Akcja usuwająca wybrane zadanie.
        /// Zmiany są zapisywane w bazie danych.
        /// </summary>
        /// <param name="id">Numer ID usuwanego zadania.</param>
        /// <returns>Widok listy zadań.</returns>
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

        /// <summary>
        /// Metoda sprawdza istnienie zadania w bazie danych.
        /// </summary>
        /// <param name="id">Numer ID wybranego zadania.</param>
        /// <returns>Prawda/Fałsz istnienia zadania w bazie danych.</returns>
        private bool ProjectTaskExists(int id)
        {
            return _context.ProjectTasks.Any(e => e.TaskID == id);
        }
    }
}
