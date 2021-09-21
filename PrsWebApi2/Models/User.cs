using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrsWebApi2.Models
{
    public class User
    {
        public int Id { get; set; }
        [StringLength(20), Required]
        public string Username { get; set; }
        [StringLength(20), Required]
        public string Password { get; set; }
        [StringLength(20), Required]
        public string FirstName { get; set; }
        [StringLength(20), Required]
        public string LastName { get; set; }
        [StringLength(12), Required]
        public string Phone { get; set; }
        [StringLength(50), Required]
        public string Email { get; set; }

        public bool Reviewer { get; set; } = false;
        public bool Admin { get; set; } = false;


    }
}
