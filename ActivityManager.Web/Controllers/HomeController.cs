using ActivityManager.Web.Data;
using ActivityManager.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ActivityManager.Web.Controllers
{
    public class HomeController : Controller
    {
       // private readonly ILogger<HomeController> _logger;

        private readonly ActivityManagerWebContext _context;

        public HomeController(ActivityManagerWebContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return _context.ActivityType != null ?
                        View(await _context.ActivityType.ToListAsync()) :
                        Problem("Entity set 'ActivityManagerWebContext.ActivityType'  is null.");
        }

        /*public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }*/

        /*public IActionResult Index()
        {
            return View();
        }*/

        public IActionResult ListItemClick() 
        {
            return View("Index"); //todo view
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string activityNameInput)//[Bind("Id,Name")] ActivityType activityType)
        {
            ActivityType activityType = new();
         //   if (ModelState.IsValid)
           // {
            if(activityNameInput != null)
            {
                activityType.Id = Guid.NewGuid();
                activityType.Name = activityNameInput;
                _context.Add(activityType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            //}
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.ActivityType == null)
            {
                return NotFound();
            }

            var activityType = await _context.ActivityType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activityType == null)
            {
                return NotFound();
            }

            return View(activityType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.ActivityType == null)
            {
                return Problem("Entity set 'ActivityManagerWebContext.ActivityType'  is null.");
            }
            var activityType = await _context.ActivityType.FindAsync(id);
            if (activityType != null)
            {
                _context.ActivityType.Remove(activityType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return base.View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}