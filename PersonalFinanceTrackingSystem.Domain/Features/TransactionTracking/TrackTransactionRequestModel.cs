using PersonalFinanceTrackingSystem.Shared.Common;

namespace PersonalFinanceTrackingSystem.Domain.Features.TransactionTracking;

public class TrackTransactionRequestModel: BaseRequestModel
{
    public string UserCode { get; set; }
    public string CategoryName { get; set; }
    public string TransactionId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public string FinanceType { get; set; }


}