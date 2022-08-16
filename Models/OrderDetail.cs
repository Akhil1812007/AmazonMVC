using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmazonMVC.Models
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailId { get; set; }

        [ForeignKey("ProductId")]
        public int? ProductId { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Value must be greater than 0")]
        public int? ProductRate { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Value must be greater than 0")]
        public int? ProductQuantity { get; set; }
        [ForeignKey("OrderMasterId")]
        public int? OrderMasterId { get; set; }
        public virtual Product? Product { get; set; }
        public virtual OrderMaster? OrderMaster { get; set; }
    }
}
