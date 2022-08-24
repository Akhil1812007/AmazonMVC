   using AmazonMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace AmazonMVC.Controllers
{
    public class CustomerController : Controller
    {
        string BaseUrl = "https://localhost:7149/";
        private object merchant;

        public IActionResult CustomerRegistration()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CustomerRegistration(Customer customer)
        {
            // Send Mail Confirmation for passsword



            var senderEmail = new MailAddress("amazonsignup00@gmail.com", "Amazon");
            var receiverEmail = new MailAddress(customer.CustomerEmail, "Receiver");
            var password = "jmkbxstayccjjnwu";
            String b = "https://localhost:7139/Customer/CustomerLogin";



            var sub = "Hello " + customer.CustomerName + "! Welcome to Amazon";
            var body = "Your User Id: " + customer.CustomerEmail + " And your password is :" + customer.CustomerPassword + " Login link " + b;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address, password)
            };
            using (var mess = new MailMessage(senderEmail, receiverEmail)
            {
                Subject = sub,
                Body = body
            })
            {
                smtp.Send(mess);
                ViewBag.Message = String.Format("Registered Successfully!!\\ Please Check Your Mail to login.");
            }

            //------------------------------------------------------------------------------------------------------------------

            customer.ConfirmPassword = customer.CustomerPassword;
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(BaseUrl);
                StringContent content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("api/Customer", content);
                return NoContent();
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
                    HttpContext.Session.SetString("CustomerName", customer.CustomerName);



                    return RedirectToAction("GetAllProduct", "Product");
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
