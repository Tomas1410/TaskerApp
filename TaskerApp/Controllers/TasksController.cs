using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskerApp.Data;
using TaskerApp.Models;

namespace TaskerApp.Controllers
{
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: Tasks
        [Authorize]
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            var taskQuery = from x in _context.Task
                            where (x.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier))
                            select x;
            switch (sortOrder)
            {
                case "name_desc":
                    taskQuery = taskQuery.OrderByDescending(s => s.Description);
                    break;
                case "Date":
                    taskQuery = taskQuery.OrderBy(s => s.CreateDate);
                    break;
                case "date_desc":
                    taskQuery = taskQuery.OrderByDescending(s => s.CreateDate);
                    break;
                default:
                    taskQuery = taskQuery.OrderBy(s => s.Description);
                    break;
            }

            return View(await taskQuery.AsNoTracking().ToListAsync());
        }

        // GET: Tasks/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var taskQuery = from x in _context.Task
                            where (x.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier))
                            select x;
            var task = await taskQuery.FirstOrDefaultAsync(x => x.Id == id);
            //var task = await _context.Task
            //    .FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }
               
            return View(task);
        }

        // GET: Tasks/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,isFinished")] Models.Task task)
        {
            if (ModelState.IsValid)
            {
                task.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                task.CreateDate = DateTime.Now; 
                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        // GET: Tasks/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var taskQuery = from x in _context.Task
                            where (x.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier))
                            select x;
            var task = await taskQuery.FirstAsync(x => x.Id == id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description")] Models.Task task, bool FinishStatus)
        {
            if (id != task.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var taskQuery = from x in _context.Task
                                    where (x.Id == id)
                                    select x;
                    var item = await taskQuery.FirstAsync();
                    item.IsFinished = FinishStatus;
                    item.Description = task.Description;
                    item.FinishDate = FinishStatus == true ? DateTime.Now : DateTime.MinValue;
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(task.Id))
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
            return View(task);
        }

        // GET: Tasks/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskQuery = from x in _context.Task
                                 where (x.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier))
                                 select x;
            var task = await taskQuery.FirstOrDefaultAsync(x => x.Id == id);
            //var task = await _context.Task
            //    .FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }
        [Authorize]
        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taskQuery = from x in _context.Task
                            where (x.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier))
                            select x;
            var task = await taskQuery.FirstOrDefaultAsync(x => x.Id == id);
            
            _context.Task.Remove(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskExists(int id)
        {
            var taskExistQuery = from x in _context.Task
                                where (x.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier))
                                select x;
            return taskExistQuery.Any(x => x.Id == id);
            //return _context.Task.Any(e => e.Id == id);
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
