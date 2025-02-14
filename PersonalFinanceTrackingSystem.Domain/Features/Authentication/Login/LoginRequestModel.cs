namespace PersonalFinanceTrackingSystem.Domain.Features.Authentication.Login
{
    public class LoginRequestModel
    {
        //[Required(ErrorMessage = "Username is required.")]
        public string? UserName { get; set; } 
        //[Required(ErrorMessage = "Password is required.")]
        public string? Password { get; set; } 
        
        public string UserId { get; set; }
    }
}
