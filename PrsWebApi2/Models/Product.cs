using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrsWebApi2.Models
{
    public class Product
    {
        public int Id { get; set; }

        public virtual Vendor vendor { get; set; }
        public int VendorId { get; set; }

        [StringLength(30), Required]
        public string PartNumber { get; set; }
        [StringLength(150), Required]
        public string Name { get; set; }
        [Column(TypeName = "decimal(11,2)")]
        public decimal Price { get; set; }
        [StringLength(100)]
        public string Unit { get; set; }
        [StringLength(255)]
        public string PhotoPath { get; set; }
    }
}
