namespace PersonalFinanceTrackingSystem.Domain.Features.Authentication.Profile;

public class ProfileRequestModel
{
    public string UserId { get; set; } 
    public string ImageUrl { get; set; }
    public string ImageFile { get; set; }
    public string ImageExtension { get; set; }  
}