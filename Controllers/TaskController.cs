using Microsoft.AspNetCore.Mvc;
using TaskToDoList.Models;
using Microsoft.EntityFrameworkCore;

namespace TaskToDoList.Controllers
{
    public class TaskController : Controller
    {
        private readonly TaskContext _context;

        public TaskController(TaskContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString, string statusFilter, string priorityFilter)
        {
            var tasks = from t in _context.Tasks select t;

            if (!string.IsNullOrEmpty(searchString))
            {
                tasks = tasks.Where(t =>
                    t.Title.Contains(searchString) ||
                    t.Description.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(statusFilter))
            {
                tasks = tasks.Where(t => t.Status == statusFilter);
            }

            if (!string.IsNullOrEmpty(priorityFilter))
            {
                tasks = tasks.Where(t => t.Priority == priorityFilter);
            }

            return View(await tasks.ToListAsync());
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskModel task)
        {
            if (ModelState.IsValid)
            {
                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return NotFound();

            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TaskModel task)
        {
            if (id != task.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var task = await _context.Tasks.FirstOrDefaultAsync(m => m.Id == id);
            if (task == null) return NotFound();

            return View(task);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // ✅ NEW: Chart Data Class
        public class ChartData
        {
            public string Label { get; set; } = "";
            public int Count { get; set; }
        }

        // ✅ Analytics Page
        public IActionResult Analytics()
        {
            var statusCounts = _context.Tasks
                .GroupBy(t => t.Status)
                .Select(g => new ChartData { Label = g.Key, Count = g.Count() })
                .ToList();

            var priorityCounts = _context.Tasks
                .GroupBy(t => t.Priority)
                .Select(g => new ChartData { Label = g.Key, Count = g.Count() })
                .ToList();

            ViewBag.StatusData = statusCounts;
            ViewBag.PriorityData = priorityCounts;

            return View();
        }
    }
}
