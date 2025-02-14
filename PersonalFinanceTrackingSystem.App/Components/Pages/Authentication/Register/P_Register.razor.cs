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
        if (_responseModel.Response.IsSuccess)
        {

            Navigation.NavigateTo("/login");
        }
        else if (!_responseModel.Response.IsSuccess)
        {
            _requestModel = new RegisterRequestModel();
            Navigation.NavigateTo("/");
        }
    }
}