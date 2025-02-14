namespace PersonalFinanceTrackingSystem.Domain.Features.TransactionTracking;

public class TransactionTrackingResponseModel
{
    public string UserName { get; set; }
    public string CategoriesName { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }
    
}