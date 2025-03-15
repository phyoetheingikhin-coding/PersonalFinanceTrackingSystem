using Microsoft.EntityFrameworkCore;
using PersonalFinanceTrackingSystem.Database.EfAppDbContextModels;
using PersonalFinanceTrackingSystem.Shared.Common;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PersonalFinanceTrackingSystem.Shared.DapperService;

namespace PersonalFinanceTrackingSystem.Domain.Features.BudgetSetup;

public class BudgetSetupService
{
    private readonly AppDbContext _db;
    private readonly DapperService _dapper;

    public BudgetSetupService(AppDbContext db, DapperService dapper)
    {
        _db = db;
        _dapper = dapper;
    }

    public async Task<Result<BudgetSetupResponseModel>> List(BudgetSetupRequestModel requestModel)
    {
        BudgetSetupResponseModel model = new BudgetSetupResponseModel();
        PageSettingResponseModel pageSetting = new();
        try
        {
            var user = await _db.Tbl_Users.AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == requestModel.CurrentUserId);
            if (user is not null)
            {
                // var budgetList = await _db.Tbl_Budgets.AsNoTracking()
                //     .Where(x => x.UserId == requestModel.CurrentUserId)
                //     .Select(x => new BudgetSetupDataModel
                //     {
                //         BudgetId = x.BudgetId,
                //         BudgetName = x.BudgetName,
                //         CategoryName = x.CategoryName,
                //         UserName = user.UserName,
                //         FromDate = x.FromDate,
                //         ToDate = x.ToDate,
                //         LimitAmount = x.LimitAmount,
                //     }).ToListAsync();
                // pageSetting.TotalRowCount = budgetList.Count;
                // model.PageSetting = pageSetting;
                // model.ListBudget = budgetList
                //     .Skip(requestModel.PageSetting.SkipRowCount)
                //     .Take(requestModel.PageSetting.PageSize).ToList();
                // model.ListBudget = budgetList;

                var query = @"SELECT  
    b.BudgetName,
    b.LimitAmount, 
    b.FromDate,
    b.ToDate,
    t.TransactionType AS FinanceType,
    SUM(CASE 
        WHEN t.TransactionType = 'Expense' THEN t.Amount  
        WHEN t.TransactionType = 'Income' THEN t.Amount  
        ELSE 0 
    END) AS NewUsedAmount
FROM 
    Tbl_Transactions t 
INNER JOIN 
    Tbl_Budgets b ON b.CategoriesCode = t.CategoriesCode
INNER JOIN 
    Tbl_Categories cat ON cat.CategoriesCode = t.CategoriesCode
WHERE 
    b.UserId = @CurrentUserId 
    AND t.UserId = @CurrentUserId
    AND CONVERT(DATE, t.CreatedDate) BETWEEN CONVERT(DATE, b.FromDate) AND CONVERT(DATE, b.ToDate)
GROUP BY 
    b.BudgetName, b.LimitAmount, b.FromDate, b.ToDate,t.TransactionType";
                var result = _dapper.Query<BudgetSetupDataModel>(query, requestModel);
                pageSetting.TotalRowCount = result.Count;
                model.PageSetting = pageSetting;
                model.ListBudget = result
                    .Skip(requestModel.PageSetting.SkipRowCount)
                    .Take(requestModel.PageSetting.PageSize).ToList();

            }
        }
        catch (Exception ex)
        {
            return Result<BudgetSetupResponseModel>.FailureResult("Fail!");
        }

        return Result<BudgetSetupResponseModel>.SuccessResult(model, "Successful");
    }

    public async Task<Result<BudgetSetupResponseModel>> Create(BudgetSetupRequestModel requestModel)
    {
        BudgetSetupResponseModel model = new BudgetSetupResponseModel();
        try
        {
            #region Check user

            var user = await _db.Tbl_Users.AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == requestModel.CurrentUserId.ToString());
            if (user is null)
            {
                return Result<BudgetSetupResponseModel>.FailureResult("User does not exist!");
            }

            #endregion

            #region Check Category Code

            var category = await _db.Tbl_Categories.AsNoTracking()
                .FirstOrDefaultAsync(x => x.CategoriesCode == requestModel.CategoryCode);
            if (category is null)
            {
                return Result<BudgetSetupResponseModel>.FailureResult("Category does not exist!");
            }

            #endregion

            Tbl_Budget budget = new Tbl_Budget();
            budget.BudgetId = Guid.NewGuid().ToString();
            budget.BudgetName = requestModel.BudgetName;
            budget.CategoryName = category.Name;
            budget.CategoriesCode = requestModel.CategoryCode;
            budget.FromDate = requestModel.FromDate;
            budget.ToDate = requestModel.ToDate;
            budget.LimitAmount = requestModel.LimitAmount;
            budget.CreatedDate = DateTime.Now;
            budget.UserId = requestModel.CurrentUserId;

            await _db.AddAsync(budget);
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Result<BudgetSetupResponseModel>.SuccessResult(model, "Budget Setup Failed");
        }

        return Result<BudgetSetupResponseModel>.SuccessResult(model, "Budget Setup Successful");
    }

    public async Task<Result<BudgetSetupResponseModel>> Update(BudgetSetupRequestModel requestModel)
    {
        BudgetSetupResponseModel model = new BudgetSetupResponseModel();
        try
        {
            var item = await _db.Tbl_Budgets.AsNoTracking().FirstOrDefaultAsync(x =>
                x.BudgetId == requestModel.BudgetId &&
                x.UserId == requestModel.CurrentUserId.ToString());
            if (item == null)
            {
                return Result<BudgetSetupResponseModel>.FailureResult("No Data Found!");
            }

            #region Check Category Code

            var category = await _db.Tbl_Categories.AsNoTracking()
                .FirstOrDefaultAsync(x => x.CategoriesCode == requestModel.CategoryCode);
            if (category is null)
            {
                return Result<BudgetSetupResponseModel>.FailureResult("Category does not exist!");
            }

            #endregion

            item.BudgetName = requestModel.BudgetName;
            item.CategoryName = category.Name;
            item.LimitAmount = requestModel.LimitAmount;
            item.FromDate = requestModel.FromDate;
            item.ToDate = requestModel.ToDate;
            _db.Entry(item).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Result<BudgetSetupResponseModel>.FailureResult($"{ex.Message}");
        }

        return Result<BudgetSetupResponseModel>.SuccessResult(model, "Updated Successful!");
    }

    public async Task<Result<BudgetSetupResponseModel>> Delete(BudgetSetupRequestModel requestModel)
    {
        BudgetSetupResponseModel model = new BudgetSetupResponseModel();
        try
        {
            var item = await _db.Tbl_Budgets.AsNoTracking().FirstOrDefaultAsync(x =>
                x.BudgetId == requestModel.BudgetId &&
                x.UserId == requestModel.CurrentUserId.ToString());
            if (item == null)
            {
                return Result<BudgetSetupResponseModel>.FailureResult("No Data Found!");
            }

            //item.DelFlag = true;
            //_db.Entry(item).State = EntityState.Modified;
            _db.Remove(item);
            await _db.SaveChangesAsync();
            model.Response = SubResponseModel.GetResponseMsg("Deleted Successful", true);
        }
        catch (Exception ex)
        {
            return Result<BudgetSetupResponseModel>.FailureResult($"{ex.Message}");
        }

        return Result<BudgetSetupResponseModel>.SuccessResult("Deleted Successful");
    }

    public async Task<Result<BudgetSetupResponseModel>> Edit(string budgetId)
    {
        BudgetSetupResponseModel model = new BudgetSetupResponseModel();
        BudgetSetupDataModel dataModel = new BudgetSetupDataModel();
        try
        {
            var item = await _db.Tbl_Budgets.AsNoTracking()
                .FirstOrDefaultAsync(x => x.BudgetId == budgetId);
            if (item == null)
            {
                return Result<BudgetSetupResponseModel>.FailureResult("No Data Found!");
            }

            dataModel.BudgetName = item.BudgetName;
            dataModel.LimitAmount = item.LimitAmount;
            dataModel.FromDate = item.FromDate;
            dataModel.ToDate = item.ToDate;
            dataModel.CategoryName = item.CategoryName;
            dataModel.CategoryCode = item.CategoriesCode;
            model.BudgetSetup = dataModel;
        }
        catch (Exception ex)
        {
            return Result<BudgetSetupResponseModel>.FailureResult($"{ex.Message}");
        }

        return Result<BudgetSetupResponseModel>.SuccessResult(model, "Successful");
    }

    public async Task<Result<BudgetSetupResponseModel>> GetCategoryList(string financeType)
    {
        BudgetSetupResponseModel model = new BudgetSetupResponseModel();
        CategoryDataModel catData = new CategoryDataModel();
        try
        {
            var item = await _db.Tbl_Categories.AsNoTracking()
                .Where(x => x.Type.ToLower() == financeType.ToLower())
                .Select(x => new CategoryDataModel
                {
                    CategoryCode = x.CategoriesCode,
                    CategoryName = x.Name
                }).ToListAsync();
            if (!item.IsNullOrEmpty())
            {
                model.ListCategory = item;
            }
        }
        catch (Exception ex)
        {
            return Result<BudgetSetupResponseModel>.FailureResult($"{ex.Message}");
        }

        return Result<BudgetSetupResponseModel>.SuccessResult(model,"Successful");
    }
}