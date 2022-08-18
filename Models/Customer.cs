using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmazonMVC.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        [Required]
        public string? CustomerEmail { get; set; }
        
        public string? CustomerName { get; set; }


        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        
        [StringLength(10, MinimumLength = 10)]

        public string? CustomerPhone { get; set; }
        
        public string? CustomerCity { get; set; }

        [StringLength(6, MinimumLength = 6)]

        public string? CustomerPincode { get; set; }

        [Required(ErrorMessage = "Please enter password"), MaxLength(20)]
        public string? CustomerPassword { get; set; }
        [NotMapped]

        [Display(Name = "ConfirmPassword")]
        [Compare("CustomerPassword", ErrorMessage = "Passwords don not match")]

        public string? ConfirmPassword { get; set; }
        public ICollection<Feedback>? Feedbacks { get; set; }
        public ICollection<Cart>? Carts { get; set; }
        public ICollection<OrderMaster>? OrderMasters { get; set; }
    }
}
