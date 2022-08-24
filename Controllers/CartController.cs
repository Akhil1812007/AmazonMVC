using AmazonMVC.Models;
using Microsoft.AspNetCore.Http;
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
            ct.CustomerId = (int)HttpContext.Session.GetInt32("CustomerId");
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(BaseUrl);
                StringContent content = new StringContent(JsonConvert.SerializeObject(ct), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("api/Cart", content);
                return RedirectToAction("CartByCustomerId", "Cart");
            }

        }
        [HttpPost]
        public async Task<List<Cart>> GetCartByCustomer(int id) // getting cart list of a particular customer 
        {
           
            List<Cart>? c = new List<Cart>();
            using (var Client = new HttpClient())
            {
                Client.BaseAddress = new Uri(BaseUrl);
                Client.DefaultRequestHeaders.Clear();
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await Client.GetAsync("api/Cart/"+id);
                if (Res.IsSuccessStatusCode)
                {
                    var Response = Res.Content.ReadAsStringAsync().Result;
                    c = JsonConvert.DeserializeObject<List<Cart>>(Response);
                }
            }
            return c;

        }
        [HttpGet]
        public async Task<IActionResult> CartByCustomerId()
        {
            int id = (int)HttpContext.Session.GetInt32("CustomerId");
            List<Cart> c = await GetCartByCustomer(id);
            return View(c);

        }
        public async Task<IActionResult> DeleteCart(int? id) //delete cart by cart Id id=cart id
        {
            int cid = (int)HttpContext.Session.GetInt32("CustomerId");
            List<Cart> CustomerCarts = (List<Cart>)await GetCartByCustomer(cid);
            var TempCart=new Cart();
            foreach (var cart in CustomerCarts)
            {
                if (cart.CartId == id)
                {
                    TempCart = cart;
                    break;
                }
            }
            
            return View(TempCart);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCart(int id)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(BaseUrl);
                StringContent content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
                var response = await httpClient.DeleteAsync("api/Cart/" + id);
                return RedirectToAction("CartByCustomerId");
            }
        }
      
        public async Task<IActionResult> UpdateCart(int id)
        {
            int cid = (int)HttpContext.Session.GetInt32("CustomerId");
            List<Cart> CustomerCarts = (List<Cart>)await GetCartByCustomer(cid);
            var TempCart = new Cart();
            foreach (var cart in CustomerCarts)
            {
                if (cart.CartId == id)
                {
                    TempCart = cart;
                    HttpContext.Session.SetInt32("ProductId", (int)TempCart.ProductId);
                    break;
                }
            }

            return View(TempCart);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCart(Cart c)
        {

            c.CustomerId = HttpContext.Session.GetInt32("CustomerId");
            c.ProductId = HttpContext.Session.GetInt32("ProductId");


            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(BaseUrl);
                StringContent content = new StringContent(JsonConvert.SerializeObject(c), Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync("api/Cart/" + c.CartId, content);
                return RedirectToAction("CartByCustomerId");
            }
        }


    }
}
