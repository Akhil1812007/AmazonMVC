using Microsoft.AspNetCore.Mvc;

namespace AmazonMVC.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
