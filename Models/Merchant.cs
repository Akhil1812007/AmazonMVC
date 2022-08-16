using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmazonMVC.Models
{
    public class Merchant
    {
        [Key]
        public int MerchantId { get; set; }

        [Required(ErrorMessage = "Field can't be empty")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string? MerchantEmail { get; set; }
        [Required]
        public string? MerchantName { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone Number Required!")]

        public string? MerchantPhoneNumber { get; set; }
        [Required(ErrorMessage = "Please enter password"), MaxLength(20)]
        public string? MerchantPassword { get; set; }
        [NotMapped]

        [Display(Name = "ConfirmPassword")]
        [Compare("MerchantPassword", ErrorMessage = "Passwords don not match")]
        public string? ConfirmPassword { get; set; }
        public ICollection<Product>? Products { get; set; }

    }
}
