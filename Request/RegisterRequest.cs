using System.ComponentModel.DataAnnotations;

namespace SjxLogistics.Models.Request
{
    public class RegisterRequest
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string MiddleName { get; set; }
        public string SurName { get; set; }
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
        public string Email { get; set; }
        public string tagCode { get; set; }

        public string Province { get; set; }
        public string virtualCodes { get; set; }
        public bool Appearance { get; set; }

        public string Parish { get; set; }

        public string ProgramType { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public string Title { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }
        public int RoleId { get; set; }




    }
}
