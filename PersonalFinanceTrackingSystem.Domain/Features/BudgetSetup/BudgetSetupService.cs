using Microsoft.EntityFrameworkCore;
using PersonalFinanceTrackingSystem.Database.EfAppDbContextModels;
using PersonalFinanceTrackingSystem.Shared.Common;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;

namespace PersonalFinanceTrackingSystem.Domain.Features.BudgetSetup;

public class BudgetSetupService
{
    private readonly AppDbContext _db;

    public BudgetSetupService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<BudgetSetupResponseModel> List(BudgetSetupRequestModel requestModel)
    {
        BudgetSetupResponseModel model = new BudgetSetupResponseModel();
        PageSettingResponseModel pageSetting = new();
        try
        {
            var user = await _db.Tbl_Users.AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == requestModel.CurrentUserId);
            if (user is not null)
            {
                // var query = _db.Tbl_Budgets.AsNoTracking()
                //     .Where(x => x.UserId == requestModel.CurrentUserId);
                //
                // var budgetList = await query
                //     .Skip((requestModel.PageNumber - 1) * requestModel.PageSize)
                //     .Take(requestModel.PageSize)
                //     .Select(x => new BudgetSetupDataModel
                //     {
                //         BudgetId = x.BudgetId,
                //         BudgetName = x.BudgetName,
                //         CategoryName = x.CategoryName,
                //         UserName = user.UserName,
                //         FromDate = x.FromDate,
                //         ToDate = x.ToDate,
                //         LimitAmount = x.LimitAmount,
                //     })
                //     .ToListAsync();
                // model.ListBudget = budgetList;
                // model.TotalRecords = await query.CountAsync();
                // model.Response = SubResponseModel.GetResponseMsg("", true);
                var budgetList = await _db.Tbl_Budgets.AsNoTracking()
                    .Where(x => x.UserId == requestModel.CurrentUserId)
                    .Select(x => new BudgetSetupDataModel
                    {
                        BudgetId = x.BudgetId,
                        BudgetName = x.BudgetName,
                        CategoryName = x.CategoryName,
                        UserName = user.UserName,
                        FromDate = x.FromDate,
                        ToDate = x.ToDate,
                        LimitAmount = x.LimitAmount,
                    }).ToListAsync();
                pageSetting.TotalRowCount = budgetList.Count;
                model.PageSetting = pageSetting;
                model.ListBudget = budgetList
                    .Skip(requestModel.PageSetting.SkipRowCount)
                    .Take(requestModel.PageSetting.PageSize).ToList();
               // model.ListBudget = budgetList;
                model.Response = SubResponseModel.GetResponseMsg("", true);
            }
        }
        catch (Exception ex)
        {
            model.Response = SubResponseModel.GetResponseMsg(ex.ToString(), false);
        }

        return model;
    }

    public async Task<BudgetSetupResponseModel> Create(BudgetSetupRequestModel requestModel)
    {
        BudgetSetupResponseModel model = new BudgetSetupResponseModel();
        try
        {
            #region Check user

            var user = await _db.Tbl_Users.AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == requestModel.CurrentUserId.ToString());
            if (user is null)
            {
                model.Response = SubResponseModel.GetResponseMsg("User does not exist!", false);
                return model;
            }

            #endregion

            #region Check Category Code

            var category = await _db.Tbl_Categories.AsNoTracking()
                .FirstOrDefaultAsync(x => x.CategoriesCode == requestModel.CategoryCode);
            if (category is null)
            {
                model.Response = SubResponseModel.GetResponseMsg("Category does not exist!", false);
                return model;
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
            model.Response = SubResponseModel.GetResponseMsg("Budget Setup Successful", true);
        }
        catch (Exception ex)
        {
            model.Response = SubResponseModel.GetResponseMsg(ex.ToString(), false);
        }

        return model;
    }

    public async Task<BudgetSetupResponseModel> Update(BudgetSetupRequestModel requestModel)
    {
        BudgetSetupResponseModel model = new BudgetSetupResponseModel();
        try
        {
            var item = await _db.Tbl_Budgets.AsNoTracking().FirstOrDefaultAsync(x =>
                x.BudgetId == requestModel.BudgetId &&
                x.UserId == requestModel.CurrentUserId.ToString());
            if (item == null)
            {
                model.Response = SubResponseModel.GetResponseMsg("No Data Found!", false);
                return model;
            }

            #region Check Category Code

            var category = await _db.Tbl_Categories.AsNoTracking()
                .FirstOrDefaultAsync(x => x.CategoriesCode == requestModel.CategoryCode);
            if (category is null)
            {
                model.Response = SubResponseModel.GetResponseMsg("Category does not exist!", false);
                return model;
            }

            #endregion

            item.BudgetName = requestModel.BudgetName;
            item.CategoryName = category.Name;
            item.LimitAmount = requestModel.LimitAmount;
            item.FromDate = requestModel.FromDate;
            item.ToDate = requestModel.ToDate;
            _db.Entry(item).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            model.Response = SubResponseModel.GetResponseMsg("Updated Successful!", true);
        }
        catch (Exception ex)
        {
            model.Response = SubResponseModel.GetResponseMsg($"{ex.Message}", false);
        }

        return model;
    }

    public async Task<BudgetSetupResponseModel> Delete(BudgetSetupRequestModel requestModel)
    {
        BudgetSetupResponseModel model = new BudgetSetupResponseModel();
        try
        {
            var item = await _db.Tbl_Budgets.AsNoTracking().FirstOrDefaultAsync(x =>
                x.BudgetId == requestModel.BudgetId &&
                x.UserId == requestModel.CurrentUserId.ToString());
            if (item == null)
            {
                model.Response = SubResponseModel.GetResponseMsg("No Data Found!", false);
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

    public async Task<BudgetSetupResponseModel> Edit(string budgetId)
    {
        BudgetSetupResponseModel model = new BudgetSetupResponseModel();
        BudgetSetupDataModel dataModel = new BudgetSetupDataModel();
        try
        {
            var item = await _db.Tbl_Budgets.AsNoTracking()
                .FirstOrDefaultAsync(x => x.BudgetId == budgetId);
            if (item == null)
            {
                model.Response = SubResponseModel.GetResponseMsg("No Data Found!", false);
                return model;
            }

            dataModel.BudgetName = item.BudgetName;
            dataModel.LimitAmount = item.LimitAmount;
            dataModel.FromDate = item.FromDate;
            dataModel.ToDate = item.ToDate;
            dataModel.CategoryName = item.CategoryName;
            dataModel.CategoryCode = item.CategoriesCode;


            model.BudgetSetup = dataModel;
            model.Response = SubResponseModel.GetResponseMsg("", true);
        }
        catch (Exception ex)
        {
            model.Response = SubResponseModel.GetResponseMsg(ex.ToString(), false);
        }

        return model;
    }

    public async Task<BudgetSetupResponseModel> GetCategoryList(string financeType)
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