namespace PersonalFinanceTrackingSystem.Domain.Features.TransactionTracking;

public class TransactionRequestModel
{
    public string UserCode { get; set; } = null!;

    public decimal Amount { get; set; }

    public string CategoriesCode { get; set; } = null!;

    public DateTime Date { get; set; }

    public string Descriptions { get; set; } = null!;
}