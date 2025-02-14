using System;
using System.Collections.Generic;

namespace PersonalFinanceTrackingSystem.Database.EfAppDbContextModels;

public partial class Tbl_ExpenseCategory
{
    public int ExpenseCatId { get; set; }

    public string? ExpenseCatName { get; set; }

    public DateTime? CreatedDate { get; set; }
}
