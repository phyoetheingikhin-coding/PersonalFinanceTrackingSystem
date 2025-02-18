using System;
using System.Collections.Generic;

namespace PersonalFinanceTrackingSystem.Database.EfAppDbContextModels;

public partial class Tbl_Transaction
{
    public int Id { get; set; }

    public string TransactionId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public decimal Amount { get; set; }

    public string CategoriesCode { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string? Descriptions { get; set; }

    public string? TransactionType { get; set; }
}
