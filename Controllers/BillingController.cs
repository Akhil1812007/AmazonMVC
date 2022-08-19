using AmazonMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AmazonMVC.Controllers
{
    public class BillingController : Controller
    {
        string BaseUrl = "https://localhost:7149/";
        [HttpPost]
        public async Task<IActionResult> Buy()// for filling the Order master 
        {
            var cid = HttpContext.Session.GetInt32("CustomerId");
            OrderMaster? om = new OrderMaster();
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(BaseUrl);
                // StringContent content = new StringContent(JsonConvert.SerializeObject(cid), Encoding.UTF8, "application/json");
                StringContent c = null;
                HttpResponseMessage Res = await httpClient.PostAsync("api/Order/"+cid,c);
                if (Res.IsSuccessStatusCode)
                {
                    var Response = Res.Content.ReadAsStringAsync().Result;
                    om = JsonConvert.DeserializeObject<OrderMaster>(Response);
                }
            }
            
            return RedirectToAction("Payment", new { id = om.OrderMasterId });


        }
        [HttpPost]
        public async Task<OrderMaster> GetOrderMasterByID(int id) // getting cart list of a particular customer 
        {

            OrderMaster? om = new OrderMaster();
            using (var Client = new HttpClient())
            {
                Client.BaseAddress = new Uri(BaseUrl);
                Client.DefaultRequestHeaders.Clear();
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await Client.GetAsync("api/Order/"+id);
                if (Res.IsSuccessStatusCode)
                {
                    var Response = Res.Content.ReadAsStringAsync().Result;
                    om = JsonConvert.DeserializeObject<OrderMaster>(Response);
                }
            }
            return om;

        }
        [HttpGet]
        public async Task<IActionResult> Payment(int? id)
        {
            OrderMaster orderMaster = await GetOrderMasterByID((int)id);
            return View(orderMaster);

        }
        [HttpPost]
        public async Task<IActionResult> Payment(OrderMaster orderMaster)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(BaseUrl);
                StringContent content = new StringContent(JsonConvert.SerializeObject(orderMaster), Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync("api/Order/OrderMaster", content);
                return RedirectToAction("GetAllProduct","Product");
            }
        }
    }
}
