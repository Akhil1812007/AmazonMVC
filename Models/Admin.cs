using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmazonMVC.Models
{
    public class Admin
    {
        [Key]
        public int AdminId { get; set; }
        [Required]
        public string? AdminName { get; set; }
        [Required]

        [EmailAddress]
        public string? AdminEmail { get; set; }
        public string? AdminPassword { get; set; }

        [NotMapped]

        [Display(Name = "ConfirmPassword")]
        [Compare("AdminPassword", ErrorMessage = "Passwords don not match")]

        public string? AdminConfirmPassword { get; set; }
    }
}
