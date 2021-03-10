using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using mondayWebApp.Models;
using mondayWebApp.Services;
using mondayWebApp.Data;
using Microsoft.AspNetCore.Identity;
using mondayWebApp.Models.ViewModels;

namespace mondayWebApp.Controllers
{
    /// <summary>
    /// Kontorler głównej strony aplikacji internetowej.
    /// Kontorler użytkownika (pracownika).
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private UserManager<IdentityUser> userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<IdentityUser> userMgr)
        {
            _logger = logger;
            _context = context;
            userManager = userMgr;
        }

        /// <summary>
        /// Akcja przygotowuje widok dla głównej strony aplikacji.
        /// Jeżeli zalogowany zostanie użytkownik z rolą "User", 
        /// to zostanie wygenerowany widok listy zadań (widok użytkownika - pracownika).
        /// </summary>
        /// <returns>Lista zadań użytkownika lub widok strony głównej.</returns>
        public async Task<IActionResult> IndexAsync()
        {
            if(User.IsInRole("User"))
            {
                var user = await userManager.GetUserAsync(User);
                var employee = _context.Employees.Where(e => e.EmployeeUserID == user.Id).Single();
                var ListOfTasks = _context.ProjectTasks.Where(p => p.EmployeeID == employee.EmployeeID).ToList();
                List<ProjectTaskViewModel> ListOfTasksViewModel = new List<ProjectTaskViewModel>();
                foreach (var item in ListOfTasks)
                {
                    ProjectTaskViewModel projectTaskViewModel = new ProjectTaskViewModel();
                    projectTaskViewModel.TaskName = item.TaskName;
                    var CreatedBy = _context.Employees.Where(e => e.EmployeeID == item.TaskCreatedBy).First();
                    projectTaskViewModel.TaskID = item.TaskID;
                    projectTaskViewModel.TaskDeadline = item.TaskDeadline;
                    projectTaskViewModel.TaskCreatedBy = CreatedBy.EmployeeNameSurname;
                    projectTaskViewModel.IsEnd = item.IsEnd;
                    ListOfTasksViewModel.Add(projectTaskViewModel);
                }
                
                return View(ListOfTasksViewModel);
            }
             
            return View();
        }

        /// <summary>
        /// Metoda zmienia status wybranego zadania na zakończone.
        /// Przenosi zadanie do listy zakończone zadania.
        /// </summary>
        /// <param name="id">Numer ID wybranego zadania.</param>
        /// <returns>Widok listy zadań użytkownika.</returns>
        public IActionResult EndTask(int id)
        {
            var projectTask = _context.ProjectTasks.Where(t => t.TaskID == id).Single();
            projectTask.IsEnd = true;
            _context.ProjectTasks.Update(projectTask);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Metoda zmienia status wybranego zadania na nie zakończone.
        /// Przenosi zadanie do listy nie zakończonych zadań.
        /// </summary>
        /// <param name="id">Numer ID wybranego zadania.</param>
        /// <returns>Widok listy zadań użytkownika.</returns>
        public IActionResult RestoreTask(int id)
        {
            var projectTask = _context.ProjectTasks.Where(t => t.TaskID == id).Single();
            projectTask.IsEnd = false;
            _context.ProjectTasks.Update(projectTask);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Akcja zwraca widok strony Prywatność i pliki cookie.
        /// </summary>
        /// <returns>Widok strony prywaność i pliki cookie.</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Akcja zwraca widok błędu.
        /// </summary>
        /// <returns>Widok strony błędu.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
