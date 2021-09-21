using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrsWebApi2.Models
{
    public class Request
    {
        public int Id { get; set; }
        public virtual User user { get; set; }
        public int UserId { get; set; }

        [StringLength(100), Required]
        public string Description { get; set; }
        [StringLength(255), Required]
        public string Justification { get; set; }
        public DateTime DateNeeded { get; set; }
        [StringLength(25), Required]
        public string DeliveryMode { get; set; }
        [StringLength(20), Required]
        public string Status { get; set; }
        [Column(TypeName = "decimal(11,2)")]
        public decimal Total { get; set; }
        public DateTime SubmittedDate { get; set; } = DateTime.Now;
        [StringLength(100)]
        public string ReasonForRejection { get; set; }

    }
}
