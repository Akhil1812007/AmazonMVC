using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmazonMVC.Models
{
    public class OrderMaster
    {
        [Key]
        public int OrderMasterId { get; set; }

        [DataType(DataType.Date)]
        public DateTime? OrderDate { get; set; }
        [Range(0, int.MaxValue)]
        public int? total { get; set; }
        public int? CardNumber { get; set; }
        [Range(0, int.MaxValue)]
        public int? AmountPaid { get; set; }

        [ForeignKey("CustomerId")]
        public int? CustomerId { get; set; }

        public virtual Customer? Customer { get; set; }
        public List<OrderDetail>? OrderDetails { get; set; }
    }
}

