using System;
using System.Collections.Generic;

namespace PersonalFinanceTrackingSystem.Database.EfAppDbContextModels;

public partial class Tbl_Category
{
    public int Id { get; set; }

    public string CategoriesCode { get; set; } = null!;

    public string UserCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Type { get; set; } = null!;
}
