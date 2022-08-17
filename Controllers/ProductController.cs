using AmazonMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AmazonMVC.Controllers
{
    public class ProductController : Controller
    {
        string BaseUrl = "https://localhost:7149/";
        public async Task<IActionResult> GetAllProduct()
        {
            List<Product>? merchants = new List<Product>();
            using (var Client = new HttpClient())
            {
                Client.BaseAddress = new Uri(BaseUrl);
                Client.DefaultRequestHeaders.Clear();
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await Client.GetAsync("api/Product");
                if (Res.IsSuccessStatusCode)
                {
                    var MerchantResponse = Res.Content.ReadAsStringAsync().Result;
                    merchants = JsonConvert.DeserializeObject<List<Product>>(MerchantResponse);

                }
                return View(merchants);

            }

        }
        public IActionResult AddProduct()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> MerchantRegistration(Product product)
        {




            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(BaseUrl);
                StringContent content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("api/Product", content);



                return RedirectToAction("GetAllMerchant");
            }

        }


    }
}
