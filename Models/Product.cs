using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmazonMVC.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Enter the Product Name")]
        [StringLength(50)]
        public string? ProductName { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Value must be greater than 0")]
        public int? UnitPrice { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Value must be greater than 0")]

        public int? ProductQnt { get; set; }
        public string? ProductImage { get; set; }

        [ForeignKey("CategoryId")]
        public int? CategoryId { get; set; }

        public virtual Category? Category { get; set; }

        [ForeignKey("MerchantId")]
        public int? MerchantId { get; set; }
        public virtual Merchant? Merchant { get; set; }

        public virtual ICollection<Feedback>? Feedbacks { get; set; }
        public virtual ICollection<Cart>? Carts { get; set; }
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}