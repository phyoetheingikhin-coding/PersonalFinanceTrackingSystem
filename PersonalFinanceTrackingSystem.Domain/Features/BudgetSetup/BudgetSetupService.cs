using Microsoft.EntityFrameworkCore;
using PersonalFinanceTrackingSystem.Database.EfAppDbContextModels;
using PersonalFinanceTrackingSystem.Shared.Common;
using System.Collections.Generic;

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
        //    PageSettingResponseModel pageSetting = new();
        try
        {

            var user = await _db.Tbl_Users.AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == requestModel.CurrentUserId.ToString());
            if (user is not null)
            {
                var budgetList = await _db.Tbl_Budgets.AsNoTracking().
                Where(x => x.UserId == requestModel.CurrentUserId.ToString()).
                Select(x => new BudgetSetupDataModel
                {
                    BudgetId = x.BudgetId,
                    //BudgetName = x.BudgetName,
                    CategoryName = x.CategoryName,
                    UserName = user.UserName,
                    FromDate = x.FromDate,
                    ToDate = x.ToDate,
                    LimitAmount = x.LimitAmount,

                }).
                ToListAsync();
                // pageSetting.TotalRowCount = lst.Count;
                //        model.PageSetting = pageSetting;
                //        model.SubjectList = lst.Skip(reqModel.PageSetting.SkipRowCount)
                //            .Take(reqModel.PageSetting.PageSize).ToList();
                model.ListBudget = budgetList;
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

            Tbl_Budget budget = new Tbl_Budget();
            budget.BudgetId = Guid.NewGuid().ToString();
            //budget.BudgetName = requestModel.BudgetName;
            budget.CategoryName = requestModel.CategoryName;
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
            var item = await _db.Tbl_Budgets.AsNoTracking().
                    FirstOrDefaultAsync(x => x.BudgetId == requestModel.BudgetId &&
                    x.UserId == requestModel.CurrentUserId.ToString());
            if (item == null)
            {
                model.Response = SubResponseModel.GetResponseMsg("No Data Found!", false);
                return model;
            }
            //item.BudgetName = requestModel.BudgetName;
            item.CategoryName = requestModel.CategoryName;
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
            var item = await _db.Tbl_Budgets.AsNoTracking().
                    FirstOrDefaultAsync(x => x.BudgetId == requestModel.BudgetId &&
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
            var item = await _db.Tbl_Budgets.AsNoTracking().
                FirstOrDefaultAsync(x => x.BudgetId == budgetId);
            if (item == null)
            {
                model.Response = SubResponseModel.GetResponseMsg("No Data Found!", false);
                return model;
            }
            //dataModel.BudgetName = item.BudgetName;
            dataModel.LimitAmount = item.LimitAmount;
            dataModel.FromDate = item.FromDate;
            dataModel.ToDate = item.ToDate;
            dataModel.CategoryName = item.CategoryName;

            model.BudgetSetup = dataModel;
            model.Response = SubResponseModel.GetResponseMsg("", true);
        }
        catch (Exception ex)
        {
            model.Response = SubResponseModel.GetResponseMsg(ex.ToString(), false);
        }
        return model;
    }

}