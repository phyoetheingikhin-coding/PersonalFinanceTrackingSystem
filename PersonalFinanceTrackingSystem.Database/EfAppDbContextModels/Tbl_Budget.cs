using System;
using System.Collections.Generic;

namespace PersonalFinanceTrackingSystem.Database.EfAppDbContextModels;

public partial class Tbl_Budget
{
    public int Id { get; set; }

    public string? BudgetId { get; set; }

    public string? UserId { get; set; }

    public string? CategoryName { get; set; }

    public string? CategoriesCode { get; set; }

    public decimal? LimitAmount { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
