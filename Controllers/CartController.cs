using Microsoft.AspNetCore.Mvc;

namespace AmazonMVC.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
