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
        private readonly ActivityManagerWebContext _context;

        public ActivitiesController(ActivityManagerWebContext context)
        {
            _context = context;
        }

        // GET: Activities
        public async Task<IActionResult> Index(Guid id)
        {
            if (_context.ActivityType == null)
            {
                return NotFound();
            }

            var activityType = GetActivityTypeWithItsActivities(id);
            if (activityType == null)
            {
                return NotFound();
            }

            return View(activityType);
        }

        // GET: Activities/Details/5
        /*public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Activity == null)
            {
                return NotFound();
            }

            var activity = await _context.Activity
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }*/

        // GET: Activities/Create
        /*public IActionResult Create()
        {
            return View();
        }*/

        // POST: Activities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Start(Guid id)     //[Bind("Id,StartTime,EndTime,Duration,Note")] Activity activity)
        {
            /*var activityType = await _context.ActivityType.FindAsync(id);
            if (activityType == null)
            {
                return NotFound();
            }*/
            var activity = new Activity
            {
                Id = Guid.NewGuid(),
                StartTime = DateTime.Now,
                ActivityTypeId = id
            };
            //activityType.Activities.Add(activity);
            //_context.Update(activityType);
            //await _context.SaveChangesAsync();
            _context.Add(activity);
            await _context.SaveChangesAsync();
            return View("Index", GetActivityTypeWithItsActivities(id));
            /*Activity activity = new();
            activity.Id = Guid.NewGuid();
            activity.StartTime = DateTime.Now;
            activity.ActivityTypeId = activityType.Id;
            activity.ActivityType = activityType;
            activityType.Activities.Add(activity);
            _context.Update(activityType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), activityType);*/
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Stop(Guid id)
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

            activity.EndTime = DateTime.Now;
            TimeSpan d = (TimeSpan)(activity.EndTime - activity.StartTime);
            activity.Duration = Math.Round(d.TotalMinutes, 2);

            try
            {
                _context.Update(activity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActivityExists(activity.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return View("Index", GetActivityTypeWithItsActivities(activity.ActivityTypeId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(Guid id, string note)
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

            activity.Note = note;
            activity.IsSaved = true;

            try
            {
                _context.Update(activity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActivityExists(activity.Id))
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







        // GET: Activities/Edit/5
        /*public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Activity == null)
            {
                return NotFound();
            }

            var activity = await _context.Activity.FindAsync(id);
            if (activity == null)
            {
                return NotFound();
            }
            return View(activity);
        }*/

        // POST: Activities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,StartTime,EndTime,Duration,Note")] Activity activity)
        {
            if (id != activity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityExists(activity.Id))
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
            return View(activity);
        }*/

        // GET: Activities/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Activity == null)
            {
                return NotFound();
            }

            var activity = await _context.Activity
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Activity == null)
            {
                return Problem("Entity set 'ActivityManagerWebContext.Activity'  is null.");
            }
            var activity = await _context.Activity.FindAsync(id);
            if (activity != null)
            {
                _context.Activity.Remove(activity);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ActivityType GetActivityTypeWithItsActivities(Guid id)
        {
            ActivityType activityType = _context.ActivityType.Find(id);

            foreach (Activity activity in _context.Activity)
            {
                if (activity.Id == id) //???
                {
                    activityType.Activities.Add(activity);
                }
            }

            return activityType;
        }

        private bool ActivityExists(Guid id)
        {
          return (_context.Activity?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}