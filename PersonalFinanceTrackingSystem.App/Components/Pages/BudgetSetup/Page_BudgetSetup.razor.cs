using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using PersonalFinanceTrackingSystem.App.Service.Security;
using PersonalFinanceTrackingSystem.Domain.Features.BudgetSetup;

namespace PersonalFinanceTrackingSystem.App.Components.Pages.BudgetSetup;

public partial class Page_BudgetSetup
{
    private BudgetSetupRequestModel _request = new();
    private BudgetSetupResponseModel _response = new();
    private UserSessionModel _userSession = new();
    private EnumFormType _formType = EnumFormType.List;
    private IEnumerable<CategoryDataModel> _lstCategory;
    private PageSettingModel ps = new();
    bool visible = false;
    private int count;
    private int value = 0;
    private int _pageNumber = 1;
    private int _pageSize = 10;

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
            await List(ps);
            //await GetCategoryList();
            StateHasChanged();
        }
    }

    async Task List(PageSettingModel ps)
    {
        _request.CurrentUserId = _userSession.UserId;
        _request.PageSetting = ps;
        _request.PageNumber = _pageNumber;
        _request.PageSize = _pageSize;
        _response = await _budgetSetupService.List(_request);
        if (!_response.Response.IsSuccess)
        {
            await _injectService.ErrorMessage(_response.Response.Message);
            return;
        }
        count = _response.PageSetting.TotalPageNo;
        _formType = EnumFormType.List;
        StateHasChanged();
    }

    async Task Save()
    {
        if (!await CheckRequiredFields(_request)) return;

        _request.CurrentUserId = _userSession.UserId;
        if (!_request.BudgetId.IsNullOrEmpty())
        {
            _response = await _budgetSetupService.Update(_request);
        }
        else
        {
            _response = await _budgetSetupService.Create(_request);
        }

        if (!_response.Response.IsSuccess)
        {
            await _injectService.ErrorMessage(_response.Response.Message);
            return;
        }

        await _injectService.SuccessMessage(_response.Response.Message);
        await List(ps);
    }

    private async Task Delete(string id)
    {
        bool isConfirm = await _injectService.ConfirmMessageBox("Are you sure you want to delete");
        if (!isConfirm) return;
        _request.CurrentUserId = _userSession.UserId;
        _request.BudgetId = id;
        var data = await _budgetSetupService.Delete(_request);
        if (!data.Response.IsSuccess)
        {
            await _injectService.ErrorMessage(data.Response.Message);
            return;
        }

        await _injectService.SuccessMessage(data.Response.Message);
        await List(ps);
        StateHasChanged();
    }

    private async Task Edit(string id)
    {
        _request.BudgetId = id;
        var data = await _budgetSetupService.Edit(id);
        if (!data.Response.IsSuccess)
        {
            await _injectService.ErrorMessage(data.Response.Message);
            return;
        }

        _request.CategoryName = data.BudgetSetup.CategoryName;
        _request.CategoryCode = data.BudgetSetup.CategoryCode;
        _request.LimitAmount = (decimal)data.BudgetSetup.LimitAmount!;
        _request.FromDate = (DateTime)data.BudgetSetup.FromDate!;
        _request.ToDate = (DateTime)data.BudgetSetup.ToDate!;
        _request.BudgetName = data.BudgetSetup.BudgetName;
        _request.FinanceType = data.BudgetSetup.FinanceType;
        _formType = EnumFormType.Edit;
    }

    private async Task Create()
    {
        try
        {
            _request.FinanceType = "Expense";
            await GetCategoryList();
            _formType = EnumFormType.Create;
            _request = new BudgetSetupRequestModel();
            StateHasChanged();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
    private async Task OnFinanceTypeChanged(int value)
    {
        _request.FinanceType = value == 0 ? "Expense" : "Income";
        await GetCategoryList();
    }

    async Task Cancel()
    {
        _request = new BudgetSetupRequestModel();
        visible = false;
        await List(ps);
        _formType = EnumFormType.List;
        StateHasChanged();
    }

    async Task GetCategoryList()
    {
        var result = await _budgetSetupService.GetCategoryList(_request.FinanceType);
        _lstCategory = result.ListCategory;
    }

    async Task<bool> CheckRequiredFields(BudgetSetupRequestModel _request)
    {
        if (_request.CategoryCode.IsNullOrEmpty())
        {
            await _injectService.ErrorMessage("CategoryName Field is Required.");
            return false;
        }

        if (_request.FromDate == null)
        {
            await _injectService.ErrorMessage("Choose a Start Date.");
            return false;
        }

        if (_request.ToDate == null)
        {
            await _injectService.ErrorMessage("Choose a End Date.");
            return false;
        }

        if (_request.FromDate > _request.ToDate)
        {
            await _injectService.ErrorMessage("Start Date must be less than End Date.");
            return false;
        }

        if (_request.ToDate < _request.FromDate)
        {
            await _injectService.ErrorMessage("End Date must be greater than Start Date.");
            return false;
        }

        return true;
    }
    
    private async Task PageChanged(int i)
    {
        ps.PageNo = i;
        await List(ps);
    }

    // private async Task NextPage()
    // {
    //     _pageNumber++;
    //     await List();
    // }
    // private async Task PreviousPage()
    // {
    //     if (_pageNumber > 1)
    //     {
    //         _pageNumber--;
    //         await List();
    //     }
    // }
}