using AmazonMVC.Models;
using AmazonMVC.TokenModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;

namespace AmazonMVC.Controllers
{
    public class MerchantController : Controller
    {
        string BaseUrl = "https://localhost:7149/";
        public async Task<IActionResult> GetAllMerchant()
        {
            List<Merchant>? merchants = new List<Merchant>();
            using(var Client = new HttpClient())
            {
                Client.BaseAddress = new Uri(BaseUrl);
                Client.DefaultRequestHeaders.Clear();
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await Client.GetAsync("api/Merchant");
                if (Res.IsSuccessStatusCode)
                {
                    var MerchantResponse=Res.Content.ReadAsStringAsync().Result;
                    merchants=JsonConvert.DeserializeObject<List<Merchant>>(MerchantResponse);

                }
                return View(merchants);

            }

        }
        [HttpGet]
        public IActionResult MerchantRegistration()
        {
            return  View();
        }
        [HttpPost]
        public async Task<IActionResult> MerchantRegistration(Merchant merchant)
        {
            // Send Mail Confirmation for passsword



            var senderEmail = new MailAddress("amazonsignup00@gmail.com", "Amazon");
            var receiverEmail = new MailAddress(merchant.MerchantEmail, "Receiver");
            var password = "jmkbxstayccjjnwu";
            String b = "https://localhost:7139/Merchant/MerchantLogin";



            var sub = "Hello " + merchant.MerchantName + "! Welcome to Amazon";
            var body = "Your User Id: " + merchant.MerchantEmail + " And your password is :" + merchant.MerchantPassword + " Login link " + b;

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
                //ViewBag.Message = String.Format("Registered Successfully!!\\ Please Check Your Mail to login.");
            }

            //------------------------------------------------------------------------------------------------------------------

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(BaseUrl);
                StringContent content = new StringContent(JsonConvert.SerializeObject(merchant), Encoding.UTF8, "application/json");
                var response=await httpClient.PostAsync("api/Merchant", content);




                return NoContent();
                        
                
            }

        }
        [HttpGet]
        public IActionResult MerchantLogin()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> MerchantLogin(Merchant? merchant)
        {
            MerchantToken? mt = new MerchantToken();
            
            using (HttpClient httpClient = new HttpClient())
            {

                merchant.ConfirmPassword = merchant.MerchantPassword;
                httpClient.BaseAddress = new Uri(BaseUrl);
                StringContent content = new StringContent(JsonConvert.SerializeObject(merchant), Encoding.UTF8, "application/json");
                var response=await httpClient.PostAsync("api/Merchant/MerchantLogin", content);
               
                if (response.IsSuccessStatusCode)
                {
                    var MerchantResponse = response.Content.ReadAsStringAsync().Result;
                    mt = JsonConvert.DeserializeObject<MerchantToken>(MerchantResponse);
                    if(mt == null)
                    {
                        ViewBag.ErrorMessage = "Invalid Credentials";
                        return View();
                    }
                    TempData["MerchantId"] =mt.merchant.MerchantId;
                    HttpContext.Session.SetInt32("MerchantId", mt.merchant.MerchantId);
                    HttpContext.Session.SetString("MerchantEmail", mt.merchant.MerchantEmail);


                    string token = mt.merchantToken;
                    HttpContext.Session.SetString("token",token);

                    return RedirectToAction("GetProductByMerchant","Product");
                }
                else
                {
                    ViewBag.ErrorMessage="Invalid Credentials";
                    return View();
                }
                
                




            }

        }


    }
}
