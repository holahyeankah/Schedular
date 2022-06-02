using System.ComponentModel.DataAnnotations;

namespace SjxLogistics.Models.Request
{
    public class RegisterRequest
    {
        public string PhoneNumber { get; set; }

       
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
        public string Email { get; set; }
       
        public string Password { get; set; }
      
        public string Otp { get; set; }
        public int StateId  { get; set; }
        public int LgaId { get; set; }
    


    }
}
