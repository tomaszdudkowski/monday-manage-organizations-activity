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
    /// <summary>
    /// Kontroler działu firmy.
    /// Autoryzacja dla Superadmistratora i Administratora.
    /// </summary>
    [Authorize(Roles = "Superadmin, Admin")]
    public class DepartmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Akcja pobierająca działy firmy z bazy danych.
        /// </summary>
        /// <returns>Widok z listą działów i managerów działów.</returns>
        // GET: Departments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Departments.Include(d => d.DepartmentManager);
            return View(await applicationDbContext.ToListAsync());
        }

        /// <summary>
        /// Akcja pobierająca szczegółowe dane o wybranym dziale firmy z bazy danych.
        /// </summary>
        /// <param name="id">Numer ID wybranego działu.</param>
        /// <returns>Widok z szczegółowymi danymi działu.</returns>
        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.DepartmentManager)
                .FirstOrDefaultAsync(m => m.DepartmentID == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        /// <summary>
        /// Akcja przygotowująca formularz do dodania nowego działu firmy.
        /// </summary>
        /// <returns>Widok z formularzem</returns>
        // GET: Departments/Create
        public IActionResult Create()
        {
            var emplAdminList = _context.Employees.Where(e => e.EmployeeRole == "Admin").Select(e => e);
            ViewData["DepartmentManagerID"] = new SelectList(emplAdminList, "EmployeeID", "EmployeeNameSurname");
            return View();
        }

        /// <summary>
        /// Akcja dodająca nowy dział firmy do bazy danych.
        /// Walidacja formularza.
        /// </summary>
        /// <param name="department">Dane działu z formularza.</param>
        /// <returns>Widok z listą działów firmy.</returns>
        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DepartmentID,DepartmentName,DepartmentDesc,DepartmentEstablishmentDate,DepartmentManagerID,IsEdited,IsChecked")] Department department)
        {
            if (ModelState.IsValid)
            {
                bool isDepartmentManagerInDB = _context.Departments.Where(d => d.DepartmentManagerID == department.DepartmentManagerID).Any();
                if(isDepartmentManagerInDB == true)
                {
                    return RedirectToAction("Error", "Home");
                }
                _context.Add(department);
                await _context.SaveChangesAsync();
                var employeeManager = _context.Employees.Where(e => e.DepartmentManager.DepartmentManagerID == department.DepartmentManagerID).Single();
                employeeManager.IsDepartmentManager = true;
                employeeManager.DepartmentID = department.DepartmentID;
                _context.Employees.Update(employeeManager);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var emplAdminList = _context.Employees.Where(e => e.EmployeeRole == "Admin").Select(e => e);
            ViewData["DepartmentManagerID"] = new SelectList(emplAdminList, "EmployeeID", "EmployeeNameSurname");
            return View(department);
        }

        /// <summary>
        /// Prywatna zmienna tymczasowa.
        /// </summary>
        private static int? TempBox { get; set; }

        /// <summary>
        /// Akcja przygotowująca widok z formularzem edycji wybranego działu.
        /// </summary>
        /// <param name="id">Numer ID wybranego działu.</param>
        /// <returns>Widok z formularzem edycji wybranego działu.</returns>
        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync(id);
            TempBox = department.DepartmentManagerID;
            if (department == null)
            {
                return NotFound();
            }
            var emplAdminList = _context.Employees.Where(e => e.EmployeeRole == "Admin").Select(e => e);
            ViewData["DepartmentManagerID"] = new SelectList(emplAdminList, "EmployeeID", "EmployeeNameSurname");
            return View(department);
        }

        /// <summary>
        /// Akcja edytująca wybrany dział.
        /// Zmiany zapisuje w bazie danych.
        /// </summary>
        /// <param name="id">Numer ID edytowanego działu.</param>
        /// <param name="department">Wybrany dział do edycji. Dane z formularza.</param>
        /// <returns>Widok listy z działami firmy.</returns>
        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DepartmentID,DepartmentName,DepartmentDesc,DepartmentEstablishmentDate,DepartmentManagerID,IsEdited,IsChecked")] Department department)
        {
            if (id != department.DepartmentID)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    if(department.DepartmentManagerID != null)
                    {
                        if(TempBox == null) // Edycja po usunięciu kierownika działu
                        {
                            _context.Update(department);
                            await _context.SaveChangesAsync();
                            var employeeManager = _context.Employees.Where(e => e.DepartmentManager.DepartmentManagerID == department.DepartmentManagerID).Single();
                            employeeManager.IsDepartmentManager = true;
                            employeeManager.DepartmentID = department.DepartmentID;
                            _context.Employees.Update(employeeManager);
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                        } else if(TempBox != null) // Tylko edytujemy
                        {
                            var oldEmployeeManager = _context.Employees.Where(e => e.EmployeeID == TempBox).Single();
                            oldEmployeeManager.IsDepartmentManager = false;
                            _context.Employees.Update(oldEmployeeManager);
                            await _context.SaveChangesAsync();
                            _context.Update(department);
                            await _context.SaveChangesAsync();
                            var employeeManager = _context.Employees.Where(e => e.DepartmentManager.DepartmentManagerID == department.DepartmentManagerID).Single();
                            employeeManager.IsDepartmentManager = true;
                            employeeManager.DepartmentID = department.DepartmentID;
                            _context.Employees.Update(employeeManager);
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.DepartmentID))
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
            ViewData["DepartmentManagerID"] = new SelectList(emplAdminList, "EmployeeID", "EmployeeNameSurname");
            return View(department);
        }

        /// <summary>
        /// Akcja przygotowująca widok podsumowania wybranego do usunięcia działu firmy.
        /// </summary>
        /// <param name="id">Numer ID wybranego działu.</param>
        /// <returns>Widok podsumowania wybranego do usunięcia działu.</returns>
        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.DepartmentManager)
                .FirstOrDefaultAsync(m => m.DepartmentID == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        /// <summary>
        /// Akcja usuwająca wybrany dział firmy.
        /// Zmiany są zapisywane w bazie danych.
        /// </summary>
        /// <param name="id">Numer ID usuwanego działu.</param>
        /// <returns>Widok listy działów.</returns>
        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            var employeeManager = _context.Employees.Where(e => e.DepartmentManager.DepartmentManagerID == department.DepartmentManagerID).Single();
            employeeManager.IsDepartmentManager = false;
            _context.Employees.Update(employeeManager);
            await _context.SaveChangesAsync();
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Metoda sprawdza istnienie działu w bazie danych.
        /// </summary>
        /// <param name="id">Numer ID wybranego działu.</param>
        /// <returns>Prawda/Fałsz istnienia działu w bazie danych.</returns>
        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.DepartmentID == id);
        }
    }
}
