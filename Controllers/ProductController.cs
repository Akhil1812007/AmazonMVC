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
        //string BaseUrl = "https://localhost:7149/";
        string BaseUrl = "https://app-amazonapi.azurewebsites.net/";
        public async Task<List<Product>> ReturnAllProducts()
        {
            List<Product>? products = new List<Product>();
            using (var Client = new HttpClient())
            {
                Client.BaseAddress = new Uri(BaseUrl);
                Client.DefaultRequestHeaders.Clear();
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await Client.GetAsync("api/Product");
                if (Res.IsSuccessStatusCode)
                {
                    var MerchantResponse = Res.Content.ReadAsStringAsync().Result;
                    products = JsonConvert.DeserializeObject<List<Product>>(MerchantResponse);

                }
                return products;

            }
        }
        public async Task<IActionResult> GetAllProduct()
        {
            ViewBag.Login = "you have successfully sign in";
            ViewBag.CustomerName = HttpContext.Session.GetString("CustomerName"); 
            var products = await  ReturnAllProducts();
            return View(products);

            

        }
        [HttpPost]
        public async Task<Product> GetProductById(int? id)
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
           
            c = await ChooseCategory();
            return View(c);

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
            ViewBag.MerchantEmail = HttpContext.Session.GetString("MerchantEmail"); ;
            var CategoryList = await ChooseCategory();

            ViewBag.CategoryList = new SelectList(CategoryList, "CategoryId", "CategoryName");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            string? token = HttpContext.Session.GetString("token");

            product.MerchantId = HttpContext.Session.GetInt32("MerchantId");
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(BaseUrl);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                StringContent content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("api/Product/product", content);
                return RedirectToAction("GetProductByMerchant");
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
            
            product.MerchantId = HttpContext.Session.GetInt32("MerchantId");
            

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(BaseUrl);
                StringContent content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync("api/Product/"+product, content);
                return RedirectToAction("GetProductByMerchant");
            }
        }
        
        [HttpGet]
        

        public async Task<IActionResult> GetProductByMerchant()
        {
            ViewBag.MerchantEmail = HttpContext.Session.GetString("MerchantEmail");
            string? token = HttpContext.Session.GetString("token");
            var id = HttpContext.Session.GetInt32("MerchantId");
            List<Product>? p = new List<Product>();
            using (var Client = new HttpClient())
            {
                Client.BaseAddress = new Uri(BaseUrl);
                Client.DefaultRequestHeaders.Authorization= new AuthenticationHeaderValue("Bearer", token);
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await Client.GetAsync("api/Merchant/MerchantId?MerchantId=" + id);
                if (Res.IsSuccessStatusCode)
                {
                    var Response = Res.Content.ReadAsStringAsync().Result;
                    p = JsonConvert.DeserializeObject<List<Product>>(Response);

                }
                

            }
            return View(p);

        }
        //delete a product By using productId
        public async Task<IActionResult> DeleteProduct(int? id)
        {


            var Product =await  GetProductById(id);
            return View(Product);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(BaseUrl);
                StringContent content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
                var response = await httpClient.DeleteAsync("api/Product/"+id);
                return RedirectToAction("GetProductByMerchant");
            }
        }


    }
}
