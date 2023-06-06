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

        public IActionResult Delete(Guid? id)
        {
            return View();
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