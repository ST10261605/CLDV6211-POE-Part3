using Microsoft.AspNetCore.Mvc;
using MVCKhumaloCraftFinal4.Data;
using MVCKhumaloCraftFinal4.Models;
using System.Diagnostics;

namespace MVCKhumaloCraftFinal2.Controllers
{
    public class HomeController : Controller
    {
        private readonly MVCKhumaloCraftFinal4Context _context;

        public HomeController(MVCKhumaloCraftFinal4Context context)
        {
            _context = context;
        }

        //home page -- typically call Home page -> Index (default)
        public IActionResult Index()
        {
            return View();
        }

        //about us page
        public IActionResult About()
        {
            return View();
        }

        //contact us page
        public IActionResult Contact()
        {
            return View();
        }

        //My work page
        public IActionResult Work()
        {
            return View();
        }

        //privacy policy page
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Cart()
        {
            return View();
        }

        //Login/register error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
