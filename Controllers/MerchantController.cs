﻿using AmazonMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
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
            



            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(BaseUrl);
                StringContent content = new StringContent(JsonConvert.SerializeObject(merchant), Encoding.UTF8, "application/json");
                var response=await httpClient.PostAsync("api/Merchant", content);

               

                return RedirectToAction("GetAllMerchant");
            }

        }
        [HttpGet]
        public IActionResult MerchantLogin()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> MerchantLogin(Merchant merchant)
        {
            //merchant.ConfirmPassword = merchant.MerchantPassword;
            //merchant.MerchantPhoneNumber = null;
            //merchant.MerchantName = null;
            //merchant.Products = null;
            using (HttpClient httpClient = new HttpClient())
            {

                merchant.ConfirmPassword = merchant.MerchantPassword;
                httpClient.BaseAddress = new Uri(BaseUrl);
                StringContent content = new StringContent(JsonConvert.SerializeObject(merchant), Encoding.UTF8, "application/json");
                var response=await httpClient.PostAsync("api/Merchant/MerchantLogin", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("GetAllMerchant");
                }
                else
                {
                    ViewBag.ErrorMessage("Invalid Credentials");
                    return View(merchant);
                }
                
                




            }

        }


    }
}