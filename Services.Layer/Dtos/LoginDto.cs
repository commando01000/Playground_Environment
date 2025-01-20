using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Layer.Dtos
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*]).{8,32}$", ErrorMessage = "Password must contain 1 uppercase letter, 1 lowercase letter, 1 number, and 1 special character, and be between 8 and 32 characters")]
        public string Password { get; set; }
    }
}
