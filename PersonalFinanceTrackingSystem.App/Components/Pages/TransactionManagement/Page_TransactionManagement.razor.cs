using Microsoft.IdentityModel.Tokens;
using MudBlazor;
using PersonalFinanceTrackingSystem.App.Service.Security;
using PersonalFinanceTrackingSystem.Domain.Features.BudgetSetup;


namespace PersonalFinanceTrackingSystem.App.Components.Pages.TransactionManagement;

public partial class Page_TransactionManagement
{
    private TrackTransactionRequestModel _request = new();
    private TrackTransactionResponseModel _response = new();
    private UserSessionModel _userSession = new();
    private EnumFormType _formType = EnumFormType.List;
    private IEnumerable<CategoryDataModel> _lstCategory;
    private bool visible = false;
    private int value = 0;

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
            await List();
            StateHasChanged();
        }
    }

    async Task List()
    {
        _request.CurrentUserId = _userSession.UserId;
        _response = await _TransactionTracking.List(_request);
        if (!_response.Response.IsSuccess)
        {
            await _injectService.ErrorMessage(_response.Response.Message);
            return;
        }
        _formType = EnumFormType.List;
        StateHasChanged();
    }
    
    async Task Save()
    {
        if (!await CheckRequiredFields(_request)) return;

        _request.CurrentUserId = _userSession.UserId;
        if (!_request.TransactionId.IsNullOrEmpty())
        {
            _response = await _TransactionTracking.Update(_request);
        }
        else
        {
            _response = await _TransactionTracking.Create(_request);
        }
        if (!_response.Response.IsSuccess)
        {
            await _injectService.ErrorMessage(_response.Response.Message);
            return;
        }
        await _injectService.SuccessMessage(_response.Response.Message);
        await List();
    }

    async Task Edit(string id)
    {
        _request.TransactionId = id;
        var data = await _TransactionTracking.Edit(id);
        if (!data.Response.IsSuccess)
        {
            await _injectService.ErrorMessage(data.Response.Message);
            return;
        }

        _request.CategoryName = data.TransactionData.CategoryName;
        _request.Amount = (decimal)data.TransactionData.Amount!;
        _request.FinanceType = data.TransactionData.FinanceType!;
        _request.Description = data.TransactionData.Descriptions!;
        // _request.TranDate = data.TransactionData.TranDate;
        _formType = EnumFormType.Edit;
    }
    private async Task Create()
    {
        try
        {
            _request.FinanceType = "Expense";
            await GetCategoryList();
            _formType = EnumFormType.Create;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
    
    private async Task Delete(string id)
    {
        bool isConfirm = await _injectService.ConfirmMessageBox("Are you sure you want to delete");
        if (!isConfirm) return;
        _request.CurrentUserId = _userSession.UserId;
        _request.TransactionId = id;
        var data = await _TransactionTracking.Delete(_request);
        if (!data.Response.IsSuccess)
        {
            await _injectService.ErrorMessage(data.Response.Message);
            return;
        }

        await _injectService.SuccessMessage(data.Response.Message);
        await List();
        StateHasChanged();
    }
    
    async Task Cancel()
    {
        _request = new TrackTransactionRequestModel();
        visible = false;
        await List();
        _formType = EnumFormType.List;
        StateHasChanged();
    }
    
    async Task<bool> CheckRequiredFields(TrackTransactionRequestModel _request)
    {
        if (_request.CategoryCode.IsNullOrEmpty())
        {
            await _injectService.ErrorMessage("CategoryName Field is Required.");
            return false;
        }
        // if (_request.CategoryName.IsNullOrEmpty())
        // {
        //     await _injectService.ErrorMessage("CategoryName Field is Required.");
        //     return false;
        // }

        if (_request.Amount <= 0 )
        {
            await _injectService.ErrorMessage("");
            return false;
        }

        if (_request.Description.IsNullOrEmpty())
        {
            await _injectService.ErrorMessage("");
            return false;
        }

        if (_request.FinanceType.IsNullOrEmpty())
        {
            await _injectService.ErrorMessage("");
            return false;
        }
        return true;
    }
    
    async Task GetCategoryList()
    {
        var result = await _TransactionTracking.GetCategoryList(_request.FinanceType);
        _lstCategory = result.ListCategory;
    }
    private async Task OnFinanceTypeChanged(int value)
    {
        _request.FinanceType = value == 0 ? "Expense" : "Income";
        await GetCategoryList();
    }
}