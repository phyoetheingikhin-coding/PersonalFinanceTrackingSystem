using PersonalFinanceTrackingSystem.Shared.Common;

namespace PersonalFinanceTrackingSystem.Domain.Features.BudgetSetup;

public class BudgetSetupResponseModel
{
    public List<BudgetSetupDataModel> ListBudget {  get; set; }
    public BudgetSetupDataModel BudgetSetup { get; set; }
    public ResponseModel Response { get; set; }
}