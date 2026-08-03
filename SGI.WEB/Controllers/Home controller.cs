using Microsoft.AspNetCore.Mvc;
using SGIbiblioteca.Domain.Interfaces;
using System.Diagnostics;

namespace SGI.WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILoggerService _loggerService;

        public HomeController(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }

        public IActionResult Index()
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
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
