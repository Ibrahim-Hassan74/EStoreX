using Microsoft.AspNetCore.Mvc;
using EStoreX.UI.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace EStoreX.UI.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        [Route("/")]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Blog()
        {
            return View();
        }
    }
}
