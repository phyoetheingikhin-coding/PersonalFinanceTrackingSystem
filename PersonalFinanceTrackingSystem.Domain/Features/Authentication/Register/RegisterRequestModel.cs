using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTrackingSystem.Domain.Features.Authentication.Register
{
    public class RegisterRequestModel
    {
        public string Name {  get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
