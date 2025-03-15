namespace PersonalFinanceTrackingSystem.Domain.Features.Authentication.Profile;

public class ProfileModel
{
    public string UserName { get; set; }
    public string PhoneNo { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string ProfileImage { get; set; }
    public string? ImagePath { get; set; }
    public string? ImageStr { get; set; }
}