using PersonalFinanceTrackingSystem.Shared.Common;

namespace PersonalFinanceTrackingSystem.Domain.Features.TransactionTracking;

public class TrackTransactionResponseModel
{
    public List<TransactionDataModel>TransactionList { get; set; }
    
    public TransactionDataModel TransactionData { get; set; }
    public ResponseModel Response { get; set; }
    public string UserName { get; set; }
    public string CategoriesName { get; set; }
    public decimal? Amount { get; set; }
    public DateTime? TransactionDate { get; set; }
    public string FinanceType { get; set; }
    
    public string Description { get; set; }
}