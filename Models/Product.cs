using System.ComponentModel.DataAnnotations;

namespace AmazonMVC.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Enter the Product Name")]
        [StringLength(50)]
        public string ProductName { get; set; }
    }
}