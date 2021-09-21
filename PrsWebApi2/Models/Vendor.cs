using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrsWebApi2.Models
{
    public class Vendor
    {
        public int Id { get; set; }
        [StringLength(10), Required]
        public string Code { get; set; }
        [StringLength(100), Required]
        public string Name { get; set; }
        [StringLength(255), Required]
        public string Address { get; set; }
        [StringLength(50), Required]
        public string City { get; set; }
        [StringLength(2), Required]
        public string State { get; set; }
        public int Zip { get; set; }
        [StringLength(12), Required]
        public string Phone { get; set; }
        [StringLength(100), Required]
        public string Email { get; set; }
    }
}
