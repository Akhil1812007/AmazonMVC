using AmazonMVC.Models;

namespace AmazonMVC.TokenModel
{
    public class MerchantToken
    {
        public Merchant? merchant { get; set; }
        public string? merchantToken { get; set; }
    }
}
