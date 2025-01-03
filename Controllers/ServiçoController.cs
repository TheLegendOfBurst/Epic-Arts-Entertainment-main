using Epic_Arts_Entertainment.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Epic_Arts_Entertainment.Controllers
{
    public class ServišoController : Controller
    {
        private readonly ILogger<ServišoController> _logger;

        public ServišoController(ILogger<ServišoController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
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
