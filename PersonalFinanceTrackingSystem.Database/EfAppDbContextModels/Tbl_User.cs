using System;
using System.Collections.Generic;

namespace PersonalFinanceTrackingSystem.Database.EfAppDbContextModels;

public partial class Tbl_User
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Password { get; set; }

    public bool? DelFlag { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
