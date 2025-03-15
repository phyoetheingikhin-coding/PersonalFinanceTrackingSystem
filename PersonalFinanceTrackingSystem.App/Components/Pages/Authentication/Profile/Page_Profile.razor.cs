using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.IdentityModel.Tokens;
using PersonalFinanceTrackingSystem.App.Service.Security;
using PersonalFinanceTrackingSystem.Domain.Features.Authentication.Profile;

namespace PersonalFinanceTrackingSystem.App.Components.Pages.Authentication.Profile;
public partial class Page_Profile
{
    private Result<ProfileModel> _profile = new();
    private ProfileRequestModel _request = new();
    private UserSessionModel _userSession = new();
    private string profileImage;
    private string _imageBase64Str = string.Empty;
    private string _defaultImage = "images/profile/default-user.png"; 
    private bool disabled = true;
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var customAuthStateProvider = (CustomAuthenticationStateProvider)authStateProvider;
            var authState = await customAuthStateProvider.GetAuthenticationStateAsync();
            if (authState.User.Identity != null && !authState.User.Identity.IsAuthenticated)
            {
                Navigation.NavigateTo("/");
                return;
            }

            _userSession = await customAuthStateProvider.GetUserData();
            _request.UserId = _userSession.UserId;
            await GetProfile();
            StateHasChanged();
        }
    }
    async Task GetProfile()
    {
        try
        {
            _profile = await _profileService.GetProfileAsync(_request);
            if (!_profile.Success)
            {
                await _injectService.ErrorMessage(_profile.Message);
                return;
            }
            if (!_profile.Data.ProfileImage.IsNullOrEmpty())
            {
                _imageBase64Str = "data:image;base64," + _profile.Data.ImageStr;
            }
            else
            {
                _imageBase64Str = _defaultImage;
            }
            // _profile.Data.ProfileImage = _profile.Data.ProfileImage ?? "images/profile/default-user.png";
           // _imageBase64Str = "data:image;base64," + _profile.Data.ImageStr;
            _request.UserName = _profile.Data.UserName;
            _request.Phone = _profile.Data.PhoneNo;
            _request.Email = _profile.Data.Email;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
        }
    }
    public void Clear()
    {
        _request.ImageUrl = null;
        _request.ImageFile = null;
        disabled = true;
        StateHasChanged();
    }

    public async Task Save()
    {
        try
        {
            if (string.IsNullOrEmpty(_request.ImageUrl))
            {
                await _injectService.ErrorMessage("Please upload image.");
                return;
            }
            var result = await _profileService.UpdateProfileAsync(_request);
            if (!result.Success)
            {
                await _injectService.ErrorMessage(result.Message);
            }
            disabled = true;
            await _injectService.SuccessMessage(result.Message);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
        }
    }

    private async Task UploadFiles(IBrowserFile file)
    {
        try
        {
            if (file != null)
            {
                MemoryStream ms = new MemoryStream();
                await file.OpenReadStream().CopyToAsync(ms);
                var bytes = ms.ToArray();
                string fileName = file.Name;
                var _base64Str = Convert.ToBase64String(bytes);
                _request.ImageFile = _base64Str;
                _request.ImageUrl = $"data:{file.ContentType};base64,{_base64Str}";
                disabled = false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
        }
    }
    
    // private async Task OnFileChange(ChangeEventArgs e)
    // {
    //     var files = (e.Value as Microsoft.AspNetCore.Components.Forms.IBrowserFile);
    //     if (files is not null)
    //     {
    //         var buffer = new byte[files.Size];
    //         await files.OpenReadStream().ReadAsync(buffer);
    //         profileImage = $"data:image/png;base64,{Convert.ToBase64String(buffer)}"; // Base64 Encoding
    //         _profile.ProfileImage = profileImage;
    //     }
    // }
    // private async Task SaveProfile()
    // {
    //     var result = await _profileService.UpdateProfileAsync(_profile);
    //     if (result.IsSuccess)
    //     {
    //     }
    // }
}