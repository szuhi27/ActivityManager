using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ActivityManager.Web.Data;
using ActivityManager.Web.Models;

namespace ActivityManager.Web.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ActivityManagerWebContext _context;
        ActivityType _activityType = new();

        public ActivitiesController(ActivityManagerWebContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Activities
        public async Task<IActionResult> Index(Guid id)
        {
            if (_context.ActivityType == null)
            {
                return NotFound();
            }

            var activityType = await _context.ActivityType
                .Include(m => m.Activities)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (activityType == null)
            {
                return NotFound();
            }

            return View(activityType);
        }

        [HttpPost]
        public async Task<IActionResult> Start(Guid id)
        {
            var activity = new Activity
            {
                Id = Guid.NewGuid(),
                StartTime = DateTime.Now,
                ActivityTypeId = id
            };
            _context.Add(activity);
            await _context.SaveChangesAsync();

            var activityType = await _context.ActivityType
                .Include(m => m.Activities)
                .FirstOrDefaultAsync(m => m.Id == id);

            return View("Index", activityType);
        }

        [HttpPost]
        public async Task<IActionResult> Stop(Guid id)
        {

            if (_context.Activity == null)
            {
                return NotFound();
            }


            var activityType = await _context.ActivityType
                .Include(m => m.Activities)
                .FirstOrDefaultAsync(m => m.Id == id);

            var activity = ReturnOpenActivity(activityType);

            if (activity == null)
            {
                return NotFound();
            }

            activity.EndTime = DateTime.Now;
            TimeSpan d = (TimeSpan)(activity.EndTime - activity.StartTime);
            activity.Duration = Math.Round(d.TotalMinutes, 2);

            await _context.SaveChangesAsync();

            return View("Index", activityType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(Guid id, string note)
        {
            if (_context.Activity == null)
            {
                return NotFound();
            }

            var activityType = await _context.ActivityType
                .Include(m => m.Activities)
                .FirstOrDefaultAsync(m => m.Id == id);

            var activity = ReturnOpenActivity(activityType);

            if (activity == null)
            {
                return NotFound();
            }

            activity.Note = note;
            activity.IsSaved = true;

            await _context.SaveChangesAsync();

            return View("Index", activityType);
        }

        Activity? ReturnOpenActivity(ActivityType at)
        {
            foreach (var a in at.Activities)
            {
                if (!a.IsSaved)
                {
                    return a;
                }
            }
            return null;
        }

        // GET: Activities/Delete/5
        public async Task<IActionResult> Delete(Guid id, Guid typeId)
        {
            if (_context.Activity == null)
            {
                return NotFound();
            }

            var activity = await _context.Activity.FindAsync(id);

            if (activity == null)
            {
                return NotFound();
            }

            _context.Activity.Remove(activity);
            await _context.SaveChangesAsync();

            var activityType = await _context.ActivityType
                .Include(m => m.Activities)
                .FirstOrDefaultAsync(m => m.Id == typeId);

            return View("Index", activityType);
        }

        private bool ActivityExists(Guid id)
        {
          return (_context.Activity?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}