using Microsoft.EntityFrameworkCore;
using PersonalFinanceTrackingSystem.Database.EfAppDbContextModels;

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
        var user = await _db.Tbl_Users.AsNoTracking()
            .FirstOrDefaultAsync(
            x => x.UserName == requestModel.UserName &&
            x.Password == requestModel.Password

            );
        // if (user is not null)
        // {
        //     model.UserCode = user.UserCode;
        // }
        return model;
    }
}
