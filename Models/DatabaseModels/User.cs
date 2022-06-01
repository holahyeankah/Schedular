
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChurchAdmin.Models
{
    public class User
    {
        public int Id { get; set; }


        [Required]
        public string FirstName { get; set; }

        [Required] 
        public string LastName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Otp { get; set; }
        public int State { get; set; }
        public int Lga { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; }
        
    }
}
