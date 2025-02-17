using PersonalFinanceTrackingSystem.Shared.Common;

namespace PersonalFinanceTrackingSystem.Domain.Features.BudgetSetup;

public class BudgetSetupRequestModel: BaseRequestModel
{   
    public string? BudgetId {  get; set; }
    public string CategoryName { get; set; }    
    public decimal LimitAmount { get; set; }
    public DateTime? FromDate { get; set; }= DateTime.Now;
    public DateTime? ToDate { get; set; }=DateTime.Now;
    
}