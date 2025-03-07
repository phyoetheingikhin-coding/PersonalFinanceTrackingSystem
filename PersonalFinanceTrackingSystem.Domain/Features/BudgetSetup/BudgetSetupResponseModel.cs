using PersonalFinanceTrackingSystem.Shared.Common;

namespace PersonalFinanceTrackingSystem.Domain.Features.BudgetSetup;

public class BudgetSetupResponseModel
{
    public List<BudgetSetupDataModel> ListBudget {  get; set; }
    
    public List<CategoryDataModel>ListCategory { get; set; }
    public BudgetSetupDataModel BudgetSetup { get; set; }
    public ResponseModel Response { get; set; }
    
    public int TotalRecords { get; set; }
    public PageSettingResponseModel PageSetting { get; set; }
}

public class CategoryDataModel
{
    public string CategoryName { get; set; }
    public string CategoryCode { get; set; }
    
}