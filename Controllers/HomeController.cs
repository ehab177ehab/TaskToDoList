using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TaskToDoList.Models;

namespace TaskToDoList.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TaskContext _context;

        public HomeController(ILogger<HomeController> logger, TaskContext context)
        {
            _logger = logger;
            _context = context;
        }

       public IActionResult Index()
{
    var today = DateTime.Today;

    var dueToday = _context.Tasks
        .Where(t => t.DueDate == today && t.Status != "Completed")
        .ToList();

    var overdue = _context.Tasks
        .Where(t => t.DueDate < today && t.Status != "Completed")
        .ToList();

    var pending = _context.Tasks
        .Where(t => t.Status != "Completed")
        .ToList();

    var completed = _context.Tasks
        .Where(t => t.Status == "Completed")
        .ToList();

    ViewBag.DueToday = dueToday;
    ViewBag.Overdue = overdue;
    ViewBag.Pending = pending;
    ViewBag.Completed = completed;
    ViewData["Date"] = today.ToString("dddd, dd MMMM yyyy");

    return View();
}


        public IActionResult About() => View();

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        
    }
}
