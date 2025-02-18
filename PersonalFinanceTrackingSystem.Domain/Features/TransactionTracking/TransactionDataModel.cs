namespace PersonalFinanceTrackingSystem.Domain.Features.TransactionTracking;

public class TransactionDataModel
{
    public string UserName { get; set; } = null!;

    public decimal? Amount { get; set; }

    public string CategoryName { get; set; } = null!;

    public DateTime? TranDate { get; set; }
    public string FinanceType { get; set; }
    
public string TransactionId { get; set; }
    public string Descriptions { get; set; } = null!;
}