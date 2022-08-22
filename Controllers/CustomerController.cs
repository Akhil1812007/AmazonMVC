   using AmazonMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace AmazonMVC.Controllers
{
    public class CustomerController : Controller
    {
        string BaseUrl = "https://localhost:7149/";

        public IActionResult CustomerRegistration()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CustomerRegistration(Customer customer)
        {
            customer.ConfirmPassword = customer.CustomerPassword;
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(BaseUrl);
                StringContent content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("api/Customer", content);
                return RedirectToAction("CustomerLogin");
            }

        }
        public IActionResult CustomerLogin()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CustomerLogin(Customer? customer)
        {
            using (HttpClient httpClient = new HttpClient())
            {

                customer.ConfirmPassword = customer.CustomerPassword;
                httpClient.BaseAddress = new Uri(BaseUrl);
                StringContent content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("api/Customer/login", content);
                 if (response.IsSuccessStatusCode)
                {
                    var CustomerResponse = response.Content.ReadAsStringAsync().Result;
                     customer= JsonConvert.DeserializeObject<Customer>(CustomerResponse);
                    HttpContext.Session.SetInt32("CustomerId", customer.CustomerId);

                    return RedirectToAction("GetAllProduct", "Product");
                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid Credentials";
                    return View();
                }

            }

        }
        public async Task<IActionResult> Logout()
        {
           return  RedirectToAction("index", "Home");
        }
    }
}
