using PersonalFinanceTrackingSystem.Shared.Common;

namespace PersonalFinanceTrackingSystem.Domain.Features.Authentication.Login
{
    public class LoginResponseModel
    {
        public string UserId { get; set; }    
        public string? UserCode {  get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string PhoneNo { get; set; }
        public ResponseModel Response { get; set; }
    }
}
