using AmazonMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
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
        [HttpPost]
        public async Task<Product> GetProductById(int id)
        {
            Product? p = new Product();
            using (var Client = new HttpClient())
            {
                Client.BaseAddress = new Uri(BaseUrl);
                Client.DefaultRequestHeaders.Clear();
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await Client.GetAsync("api/Product/"+id);
                if (Res.IsSuccessStatusCode)
                {
                    var Response = Res.Content.ReadAsStringAsync().Result;
                    p = JsonConvert.DeserializeObject<Product>(Response);

                }
                return p;

            }

        }

        public async Task<IActionResult> GetAllCategory()
        {
            List<Category>? c = new List<Category>();
            using (var Client = new HttpClient())
            {
                Client.BaseAddress = new Uri(BaseUrl);
                Client.DefaultRequestHeaders.Clear();
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await Client.GetAsync("api/Product/GetProductId");
                if (Res.IsSuccessStatusCode)
                {
                    var Response = Res.Content.ReadAsStringAsync().Result;
                    c = JsonConvert.DeserializeObject<List<Category>>(Response);

                }
                return View(c);

            }

        }
        [HttpPost]
        public async Task<List<Category>> ChooseCategory()
        {
            List<Category> ?c = new List<Category>();
            using (var Client = new HttpClient())
            {
                Client.BaseAddress = new Uri(BaseUrl);
                Client.DefaultRequestHeaders.Clear();
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await Client.GetAsync("api/Admin/GetAllCategory");
                if (Res.IsSuccessStatusCode)
                {
                    var Response = Res.Content.ReadAsStringAsync().Result;
                    c = JsonConvert.DeserializeObject<List<Category>>(Response);

                }
                return c;

            }

        }
        public async Task<IActionResult> AddProduct()
        {
            var CategoryList = await ChooseCategory();

            ViewBag.CategoryList = new SelectList(CategoryList, "CategoryId", "CategoryName");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {




            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(BaseUrl);
                StringContent content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("api/Product/product", content);



                return RedirectToAction("GetAllProduct");
            }

        }
        public async Task<IActionResult> UpdateProduct(int id)
        {
            Product p = await GetProductById(id);
            var CategoryList = await ChooseCategory();

            ViewBag.CategoryList = new SelectList(CategoryList, "CategoryId", "CategoryName");
            return View(p);
        }
        [HttpPost]
        public  async Task<IActionResult> UpdateProduct(Product product)
        {
            product.MerchantId = Convert.ToInt32(TempData["MerchantId"]);
            
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(BaseUrl);
                StringContent content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync("api/Product/"+product, content);



                return RedirectToAction("GetAllProduct");
            }
           

            
        }


    }
}
