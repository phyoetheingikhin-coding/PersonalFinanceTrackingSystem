using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceTrackingSystem.Domain.Features.Authentication.Login
{
    public class LoginRequestModel
    {
        public string? UserName { get; set; } 
        public string? Password { get; set; }
        public string PhoneNo { get; set; }
        public string ConfirmPassword { get; set; } = string.Empty;

    }
}
