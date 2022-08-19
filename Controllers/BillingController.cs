using AmazonMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace AmazonMVC.Controllers
{
    public class BillingController : Controller
    {
        string BaseUrl = "https://localhost:7149/";
        [HttpPost]
        public async Task<OrderMaster> Buy()// for filling the Order master 
        {
            var cid = HttpContext.Session.GetInt32("CustomerId");
            OrderMaster? om = new OrderMaster();
            using (var Client = new HttpClient())
            {
                Client.BaseAddress = new Uri(BaseUrl);
                Client.DefaultRequestHeaders.Clear();
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await Client.PostAsync("api/Order/Buy/"+cid);
                if (Res.IsSuccessStatusCode)
                {
                    var Response = Res.Content.ReadAsStringAsync().Result;
                    om = JsonConvert.DeserializeObject<OrderMaster>(Response);

                }
                return om;

            }



        }
    }
}
