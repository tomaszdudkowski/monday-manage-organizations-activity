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

namespace mondayWebApp.Controllers
{
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

        public async Task<IActionResult> IndexAsync()
        {
            if(User.IsInRole("User"))
            {
                var user = await userManager.GetUserAsync(User);
                var employee = _context.Employees.Where(e => e.EmployeeUserID == user.Id).Single();
                var ListOfTasks = _context.ProjectTasks.Where(p => p.EmployeeID == employee.EmployeeID).ToList();
                return View(ListOfTasks);
            }
             
            return View();
        }

        public IActionResult EndTask(int id)
        {
            var projectTask = _context.ProjectTasks.Where(t => t.TaskID == id).Single();
            projectTask.IsEnd = true;
            _context.ProjectTasks.Update(projectTask);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RestoreTask(int id)
        {
            var projectTask = _context.ProjectTasks.Where(t => t.TaskID == id).Single();
            projectTask.IsEnd = false;
            _context.ProjectTasks.Update(projectTask);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
