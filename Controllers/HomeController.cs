using AmazonMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AmazonMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        
        
      
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
       
       
        public async Task<IActionResult> Logout()
        {
            return RedirectToAction("index", "Home");
        }
        public IActionResult Search()
        {
            return View();
        }
        ProductController p=new ProductController();
        public async Task<IActionResult> SearchResult(string SearchPhrase, string option)
        {
            //p.Name.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0



            List<Product> result = new List<Product>();



            var products = await p.ReturnAllProducts();
            foreach (var product in products)
            {
                if (option.Equals("ProductName") && product.ProductName.IndexOf(SearchPhrase, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    result.Add(product);
                }
                else if (option.Equals("CategoryName") && product.Category.CategoryName.IndexOf(SearchPhrase, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    result.Add(product);
                }
                else
                {
                    ViewBag.ErrorMessage = "No Result Found";
                }
            }
            return View(result);
        }


    }
}