using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmazonMVC.Models
{
    public class Feedback
    {
        [Key]
        public int FeedbackId { get; set; }
        public string? FeedbackComment { get; set; }

        [ForeignKey("ProductId")]
        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }

        [ForeignKey("CustomerId")]
        public int? CustomerId { get; set; }

        public virtual Customer? Customer { get; set; }
    }
}
