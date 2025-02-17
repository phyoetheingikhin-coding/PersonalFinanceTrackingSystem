using Microsoft.EntityFrameworkCore;
using PersonalFinanceTrackingSystem.Database.EfAppDbContextModels;
using PersonalFinanceTrackingSystem.Shared;
using PersonalFinanceTrackingSystem.Shared.Common;

namespace PersonalFinanceTrackingSystem.Domain.Features.Authentication.Login;

public class LoginService
{
    private readonly AppDbContext _db;

    public LoginService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<LoginResponseModel> Login(LoginRequestModel requestModel)
    {
        var model = new LoginResponseModel();
        try
        {
            string hashPassword = requestModel.Password.ToSHA256HexHashString(requestModel.UserName);
            var user = await _db.Tbl_Users.AsNoTracking()
                .FirstOrDefaultAsync(
                x => x.UserName == requestModel.UserName &&
                x.Password == hashPassword

                );
            if (user is null)
            {
                model.Response = SubResponseModel.GetResponseMsg("UserName and Password is wrong", false);
                return model;
            }
            model.UserId = user.UserId;
            model.UserName = user.UserName;
            model.Response = SubResponseModel.GetResponseMsg("Login Successful", true);
        }
        catch (Exception ex)
        {
            model.Response = SubResponseModel.GetResponseMsg(ex.ToString(), false);
        }

        return model;
    }
}
