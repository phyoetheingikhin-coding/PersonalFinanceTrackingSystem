using System;
using System.Collections.Generic;

namespace PersonalFinanceTrackingSystem.Database.EfAppDbContextModels;

public partial class Tbl_Budget
{
    public int BudgetsId { get; set; }

    public string? UserCode { get; set; }

    public string? CategoriesCode { get; set; }

    public decimal? LimitAmount { get; set; }

    public string? PeriodType { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
