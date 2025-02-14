using Microsoft.EntityFrameworkCore;
using PersonalFinanceTrackingSystem.Database.EfAppDbContextModels;

namespace PersonalFinanceTrackingSystem.Domain.Features.TransactionTracking;

public class TransactionTrackingService
{
    private readonly AppDbContext _db;

    public TransactionTrackingService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<TrackTransactionResponseModel> TrackTransactionByCategory(TrackTransactionRequestModel requestModel)
    {
        var model = new TrackTransactionResponseModel();
        var category = await _db.Tbl_Categories.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == requestModel.CategoriesType);
        if (category is null)
        {
            goto result;
        }

        var transaction = await _db.Tbl_Transactions.AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == requestModel.UserCode &&
                                      x.CategoriesCode == category.CategoriesCode);
        if (transaction is not null)
        {
           // model.Amount = transaction.Amount;
            model.CategoriesName = category.Name;
           // model.TransactionDate = transaction.CreatedDate;
        }
        result:
        return model;
    }

    public void AddTransaction(TransactionRequestModel requestModel)
    {
        
    }
}