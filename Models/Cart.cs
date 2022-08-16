using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmazonMVC.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        [ForeignKey("ProductId")]
        public int? ProductId { get; set; }
        public virtual Product? Product { get; set; }
        [Required]
        [Range(0, 5, ErrorMessage = "Enter a valid Number")]
        public int? ProductQuantity { get; set; }


        [ForeignKey("CustomerId")]
        public int? CustomerId { get; set; }


        public virtual Customer? Customer { get; set; }
    }
}
