using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmazonMVC.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        public string? CustomerEmail { get; set; }
        [Required]
        public string? CustomerName { get; set; }


        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone Number Required!")]
        [StringLength(10, MinimumLength = 10)]

        public string? CustomerPhone { get; set; }
        [Required]
        public string? CustomerCity { get; set; }

        [Required]
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
