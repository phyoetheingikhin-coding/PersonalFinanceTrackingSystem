using System;
using System.Collections.Generic;

namespace PersonalFinanceTrackingSystem.Database.EfAppDbContextModels;

public partial class Tbl_Budget
{
    public int Id { get; set; }

    public string UserCode { get; set; } = null!;

    public string CategoriesCode { get; set; } = null!;

    public decimal LimitAmount { get; set; }

    public string Month { get; set; } = null!;

    public string Year { get; set; } = null!;
}
