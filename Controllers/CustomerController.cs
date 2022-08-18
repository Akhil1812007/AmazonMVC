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




            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(BaseUrl);
                StringContent content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("api/Customer", content);



                return RedirectToAction();
            }

        }
        public IActionResult MerchantLogin()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CustomerLogin(Customer customer)
        {

            using (HttpClient httpClient = new HttpClient())
            {

                customer.ConfirmPassword = customer.CustomerPassword;
                httpClient.BaseAddress = new Uri(BaseUrl);
                StringContent content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("api/Customer/login", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("");
                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid Credentials";
                    return View();
                }

            }

        }
    }
}
