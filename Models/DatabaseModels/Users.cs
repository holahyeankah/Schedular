using Schedular.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChurchAdmin.Models
{
    public class Users
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string MiddleName { get; set; }
        [Required]
        public string SurName { get; set; }

       

        public string Gender { get; set; }
        public string tagCode { get; set; }

        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
       
        public string Password { get; set; }
       
        public bool Appearance { get; set; } = false;
        [Required]
        public string Province { get; set; }
        [Required]
        public string Parish { get; set; }
        [Required]
        public string ProgramType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; }
        public string virtualCode { get; set; }

        public string Role { get; set; }  
    }
}
