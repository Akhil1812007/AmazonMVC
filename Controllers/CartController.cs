using AmazonMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AmazonMVC.Controllers
{
    public class CartController : Controller
    {
        string BaseUrl = "https://localhost:7149/";

        public IActionResult AddToCart(int id)
        {
            TempData["ProductId"] = id;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart(Cart ct)
        {
            ct.ProductId = (int?)TempData["ProductId"];
            ct.CustomerId = (int?)TempData["CustomerId"];
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(BaseUrl);
                StringContent content = new StringContent(JsonConvert.SerializeObject(ct), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("api/Cart", content);
                return RedirectToAction("GetCartByCustomer", "Cart");
            }

        }
        [HttpGet]
        public async Task<IActionResult> GetCartByCustomer()
        {
            var id = TempData["CustomerId"];
            List<Cart>? c = new List<Cart>();
            using (var Client = new HttpClient())
            {
                Client.BaseAddress = new Uri(BaseUrl);
                Client.DefaultRequestHeaders.Clear();
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await Client.GetAsync("api/Cart"+id);
                if (Res.IsSuccessStatusCode)
                {
                    var Response = Res.Content.ReadAsStringAsync().Result;
                    c = JsonConvert.DeserializeObject<List<Cart>>(Response);

                }


            }
            return View(c);

        }

    }
}
