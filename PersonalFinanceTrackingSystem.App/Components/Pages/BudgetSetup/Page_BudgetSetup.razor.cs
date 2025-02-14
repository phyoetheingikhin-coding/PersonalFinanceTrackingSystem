using PersonalFinanceTrackingSystem.Database.EfAppDbContextModels;

namespace PersonalFinanceTrackingSystem.App.Components.Pages.BudgetSetup;

public partial class Page_BudgetSetup
{
    private readonly AppDbContext _db;

    public Page_BudgetSetup(AppDbContext db)
    {
        _db = db;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            
        }
    }
}