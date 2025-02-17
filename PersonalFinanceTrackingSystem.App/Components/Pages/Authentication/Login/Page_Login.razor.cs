using Microsoft.IdentityModel.Tokens;
using PersonalFinanceTrackingSystem.App.Service.Security;
using PersonalFinanceTrackingSystem.Domain.Features.Authentication.Login;
using PersonalFinanceTrackingSystem.Domain.Features.Authentication.Register;
using PersonalFinanceTrackingSystem.Shared.Common;

namespace PersonalFinanceTrackingSystem.App.Components.Pages.Authentication.Login;

public partial class Page_Login
{
    private LoginRequestModel _requestModel = new LoginRequestModel();
    private bool _isRegister = false;
    private async Task Login()
    {
        if (!await CheckRequiredField(_requestModel, _isRegister)) return;
        try
        {
            var result = await _loginService.Login(_requestModel);
            if (!result.Response.IsSuccess)
            {
                await _injectService.ErrorMessage(result.Response.Message);
                return;
            }
            var userSessionModel = new UserSessionModel
            {
                UserId = result.UserId,
                UserName = result.UserName,
                Role = "user"
            };
            var customAuthStateProvider = (CustomAuthenticationStateProvider)authStateProvider;
            await customAuthStateProvider.UpdateAuthenticationState(userSessionModel);
            Navigation.NavigateTo("/dashboard", forceLoad: true);
        }
        catch (Exception ex)
        {
        }
    }

    private async Task Register()
    {
        if (!await CheckRequiredField(_requestModel,_isRegister)) return;
        var result = await _registerService.Register(_requestModel);
        if (!result.Response.IsSuccess)
        {
            await _injectService.ErrorMessage(result.Response.Message);
            return;
        }
        await _injectService.SuccessMessage(result.Response.Message);
        _isRegister = false;
        _requestModel = new();
        StateHasChanged();
    }

    #region Check Required Field

    async Task<bool> CheckRequiredField(LoginRequestModel request, bool isRegister)
    {
        

        if (request.UserName.IsNullOrEmpty())
        {
            await _injectService.ErrorMessage("UserName Field is Required.");
            return false;
        }
        if (request.Password.IsNullOrEmpty())
        {
            await _injectService.ErrorMessage("Password Field is Required.");
            return false;
        }
        if (isRegister) {
            if (request.PhoneNo.IsNullOrEmpty())
            {
                await _injectService.ErrorMessage("PhoneNo Field is Required.");
                return false;
            }
            if (request.ConfirmPassword.IsNullOrEmpty())
            {
                await _injectService.ErrorMessage("ConfirmPassword Field is Required.");
                return false;
            }
        }
            
        return true;
    }

    //        if (string.IsNullOrEmpty(_requestModel.Name))
    //        {
    //            errorMessage = "Username is required!";
    //            return;
    //        }

    //        if (string.IsNullOrEmpty(_requestModel.Password))
    //        {
    //            errorMessage = "Password is required!";
    //            return;
    //        }

    //        if (_requestModel.Password != _requestModel.ConfirmPassword)
    //{
    //    errorMessage = "Passwords do not match!";
    //    return;
    //}

    #endregion

    private void ChangePage()
    {
        _isRegister = _isRegister ? false : true;
        _requestModel = new();
        StateHasChanged();
    }

}
