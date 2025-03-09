using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PersonalFinanceTrackingSystem.Database.EfAppDbContextModels;
using PersonalFinanceTrackingSystem.Domain.Features.BudgetSetup;
using PersonalFinanceTrackingSystem.Shared.Common;
using PersonalFinanceTrackingSystem.Shared.DapperService;

namespace PersonalFinanceTrackingSystem.Domain.Features.TransactionTracking;

public class TransactionTrackingService
{
    private readonly AppDbContext _db;
    private readonly DapperService _dapper;

    public TransactionTrackingService(AppDbContext db, DapperService dapper)
    {
        _db = db;
        _dapper = dapper;
    }

    public async Task<TrackTransactionResponseModel> List(TrackTransactionRequestModel request)
    {
        TrackTransactionResponseModel model = new TrackTransactionResponseModel();
        try
        {
            #region Check User

            var user = await _db.Tbl_Users.AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == request.CurrentUserId);
            if (user is null)
            {
                model.Response = SubResponseModel.GetResponseMsg("", false);
                return model;
            }

            #endregion
            
           
            
            string query = @" SELECT 
     t.TransactionId,
    t.CreatedDate AS TranDate,
    t.Amount,
    t.TransactionType AS FinanceType,
    t.Descriptions,
    c.Name AS CategoryName 
FROM 
    Tbl_Transactions t 
INNER JOIN 
    Tbl_Categories c 
ON 
    t.CategoriesCode = c.CategoriesCode
WHERE 
    t.UserId = @CurrentUserId";
            var result = _dapper.Query<TransactionDataModel>(query, request);
            //var transaction = result.ToList();

            // var transaction = await _db.Tbl_Transactions.AsNoTracking()
            //     .Where(x => x.UserId == user.UserId)
            //     .Select(x => new TransactionDataModel()
            //     {
            //         TranDate = x.CreatedDate,
            //         Amount = x.Amount,
            //         UserName = user.UserName,
            //         FinanceType = x.TransactionType,
            //         //Description = x.Note,
            //         //CategoryName = x.CategoriesName
            //     })
            //     .ToListAsync();
            //model.TransactionList = transaction;
            model.TransactionList = result;
            model.Response = SubResponseModel.GetResponseMsg("Success", true);
        }
        catch (Exception ex)
        {
            model.Response = SubResponseModel.GetResponseMsg("Error", false);
        }

        return model;
    }

    public async Task<TrackTransactionResponseModel> Update(TrackTransactionRequestModel request)
    {
        var model = new TrackTransactionResponseModel();
        try
        {
            var transaction = await _db.Tbl_Transactions.AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.TransactionId == request.TransactionId &&
                    x.UserId == request.CurrentUserId);

            if (transaction is null)
            {
                model.Response = SubResponseModel.GetResponseMsg("No Data found", false);
                return model;
            }

            transaction.Descriptions = request.Description;
            transaction.TransactionType = request.FinanceType;
            transaction.Amount = request.Amount;
            transaction.CategoriesCode = request.CategoryName;
            //transaction.UpdatedDate = DateTime.Now;
            _db.Entry(transaction).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            model.Response = SubResponseModel.GetResponseMsg("Updated Successful!", true);
        }
        catch (Exception ex)
        {
            model.Response = SubResponseModel.GetResponseMsg($"{ex.Message}", false);
        }

        return model;
    }

    public async Task<TrackTransactionResponseModel> Edit(string tranId)
    {
        TrackTransactionResponseModel model = new TrackTransactionResponseModel();
        TransactionDataModel dataModel = new TransactionDataModel();
        try
        {
            var item = await _db.Tbl_Transactions.AsNoTracking()
                .FirstOrDefaultAsync(x => x.TransactionId == tranId
                );
            if (item == null)
            {
                model.Response = SubResponseModel.GetResponseMsg("No Data Found!", false);
                return model;
            }
            
            #region Check Category
            
            var category = await _db.Tbl_Categories.AsNoTracking()
                .FirstOrDefaultAsync(x => x.CategoriesCode == item.CategoriesCode);
            if (category is null)
            {
                model.Response = SubResponseModel.GetResponseMsg("Category does not exist!", false);
                return model;
            }
            
            #endregion
            
            dataModel.CategoryName = category.Name;
            dataModel.Amount = item.Amount;
            dataModel.Descriptions = item.Descriptions;
            dataModel.TranDate = item.CreatedDate;
            dataModel.FinanceType = item.TransactionType;
            dataModel.TransactionId = item.TransactionId;

            model.TransactionData = dataModel;
            model.Response = SubResponseModel.GetResponseMsg("", true);
        }
        catch (Exception ex)
        {
            model.Response = SubResponseModel.GetResponseMsg(ex.ToString(), false);
        }

        return model;
    }

    public async Task<TrackTransactionResponseModel> Delete(TrackTransactionRequestModel request)
    {
        TrackTransactionResponseModel model = new TrackTransactionResponseModel();
        try
        {
            var item = await _db.Tbl_Transactions.AsNoTracking()
                .FirstOrDefaultAsync(x => x.TransactionId == request.TransactionId &&
                                          x.UserId == request.CurrentUserId);
            if (item is null)
            {
                model.Response = SubResponseModel.GetResponseMsg("No Data Found", false);
                return model;
            }

            //item.DelFlag = true;
            //_db.Entry(item).State = EntityState.Modified;
            _db.Remove(item);
            await _db.SaveChangesAsync();
            model.Response = SubResponseModel.GetResponseMsg("Deleted Successful", true);
        }
        catch (Exception ex)
        {
            model.Response = SubResponseModel.GetResponseMsg(ex.ToString(), false);
        }

        return model;
    }

    public async Task<TrackTransactionResponseModel> Create(TrackTransactionRequestModel request)
    {
        var model = new TrackTransactionResponseModel();
        try
        {
            #region Check user

            var user = await _db.Tbl_Users.AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == request.CurrentUserId.ToString());
            if (user is null)
            {
                model.Response = SubResponseModel.GetResponseMsg("User does not exist!", false);
                return model;
            }

            #endregion

            #region Check Category

            var category = await _db.Tbl_Categories.AsNoTracking()
                .FirstOrDefaultAsync(x => x.CategoriesCode == request.CategoryCode);
            if (category is null)
            {
                model.Response = SubResponseModel.GetResponseMsg("Category does not exist!", false);
                return model;
            }

            #endregion

            Tbl_Transaction item = new Tbl_Transaction()
            {
                //TransactionId = Ulid.NewUlid().ToString(),
                TransactionId = Guid.NewGuid().ToString(),
                CategoriesCode = category.CategoriesCode,
                Descriptions = request.Description,
                UserId = request.CurrentUserId,
                Amount = request.Amount,
                CreatedDate = request.TranDate,
                TransactionType = request.FinanceType
                // FinanceType = request.FinanceType
            };
            await _db.AddAsync(item);
            await _db.SaveChangesAsync();
            model.Response = SubResponseModel.GetResponseMsg("Budget Setup Successful", true);
        }
        catch (Exception ex)
        {
            model.Response = SubResponseModel.GetResponseMsg(ex.ToString(), false);
        }

        return model;
    }

    public async Task<TrackTransactionResponseModel> TrackTransactionByCategory(
        TrackTransactionRequestModel requestModel)
    {
        var model = new TrackTransactionResponseModel();
        var category = await _db.Tbl_Categories.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == requestModel.CategoryName);
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

    public void AddTransaction(TransactionDataModel dataModel)
    {
    }

    public async Task<TrackTransactionResponseModel> GetCategoryList(string financeType)
    {
        TrackTransactionResponseModel model = new TrackTransactionResponseModel();
        CategoryDataModel catData = new CategoryDataModel();
        try
        {
            var item = await _db.Tbl_Categories.AsNoTracking()
                .Where(x => x.Type == financeType)
                .Select(x => new CategoryDataModel
                {
                    CategoryCode = x.CategoriesCode,
                    CategoryName = x.Name
                }).ToListAsync();
            if (!item.IsNullOrEmpty())
            {
                model.ListCategory = item;
                model.Response = SubResponseModel.GetResponseMsg("", true);
            }
        }
        catch (Exception ex)
        {
            model.Response = SubResponseModel.GetResponseMsg(ex.ToString(), false);
        }

        return model;
    }
}