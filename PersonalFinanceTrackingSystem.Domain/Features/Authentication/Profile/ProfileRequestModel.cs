namespace PersonalFinanceTrackingSystem.Domain.Features.Authentication.Profile;

public class ProfileRequestModel
{
    public string UserId { get; set; } 
    public string ImageUrl { get; set; }
    public string ImageFile { get; set; }
    public string UserName { get; set; }  
    public string Phone { get; set; }  
    public string Email { get; set; }  
    public string ProfileImage { get; set; }
}