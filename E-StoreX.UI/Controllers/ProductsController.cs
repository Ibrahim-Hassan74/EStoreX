using Microsoft.AspNetCore.Mvc;

namespace EStoreX.UI.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
