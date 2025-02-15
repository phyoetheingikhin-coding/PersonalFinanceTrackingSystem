using Microsoft.JSInterop;
using PersonalFinanceTrackingSystem.Domain.Features.Authentication.Register;

namespace PersonalFinanceTrackingSystem.App.Components.Pages.Authentication.Register;

public partial class P_Register
{
    private RegisterRequestModel _requestModel = new RegisterRequestModel();
    private RegisterResponseModel _responseModel = new RegisterResponseModel();
    private string errorMessage;

    private async void Register()
    {

        #region Check Required Field

        if (string.IsNullOrEmpty(_requestModel.Name))
        {
            errorMessage = "Username is required!";
            return;
        }

        if (string.IsNullOrEmpty(_requestModel.Password))
        {
            errorMessage = "Password is required!";
            return;
        }

        if (_requestModel.Password != _requestModel.ConfirmPassword)
        {
            errorMessage = "Passwords do not match!";
            return;
        }

        #endregion

        _responseModel = await _registerService.Register(_requestModel);
        if (!_responseModel.Response.IsSuccess)
        {
            //await JsRuntime.InvokeVoidAsync("showSweetAlert", "success", _responseModel.Response.Message);
            //await Task.Delay(2000); // Short delay before redirecting
            //Navigation.NavigateTo("/login"); // Redirect to login

            await _injectService.ErrorMessage(_responseModel.Response.Message);
            return;
        }
        await _injectService.SuccessMessage(_responseModel.Response.Message);
        Navigation.NavigateTo("/login");
        StateHasChanged();
    }
}