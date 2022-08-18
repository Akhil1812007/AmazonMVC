using Microsoft.AspNetCore.Mvc;

namespace AmazonMVC.Controllers
{
    public class BillingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
