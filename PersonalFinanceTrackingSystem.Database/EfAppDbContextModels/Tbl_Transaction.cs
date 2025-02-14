using System;
using System.Collections.Generic;

namespace PersonalFinanceTrackingSystem.Database.EfAppDbContextModels;

public partial class Tbl_Transaction
{
    public int Id { get; set; }

    public string? UserId { get; set; }

    public decimal? Amount { get; set; }

    public string? CategoriesCode { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? Descriptions { get; set; }

    public string? TransactionType { get; set; }
}
