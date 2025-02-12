using System;
using System.Collections.Generic;

namespace PersonalFinanceTrackingSystem.Database.EfAppDbContextModels;

public partial class Tbl_Transaction
{
    public int Id { get; set; }

    public string UserCode { get; set; } = null!;

    public decimal Amount { get; set; }

    public string CategoriesCode { get; set; } = null!;

    public DateTime Date { get; set; }

    public string Descriptions { get; set; } = null!;
}
