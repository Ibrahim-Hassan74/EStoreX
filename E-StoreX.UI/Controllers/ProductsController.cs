using Microsoft.AspNetCore.Mvc;

namespace EStoreX.UI.Controllers
{
    public class ProductsController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(Guid id)
        {
            return View();
        }

        public IActionResult Search(string term)
        {
            return View();
        }

        public IActionResult FilterByCategory(Guid category)
        {
            return View();
        }



    }
}
