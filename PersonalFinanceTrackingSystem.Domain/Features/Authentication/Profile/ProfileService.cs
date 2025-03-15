using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceTrackingSystem.Database.EfAppDbContextModels;
using PersonalFinanceTrackingSystem.Shared.Common;

namespace PersonalFinanceTrackingSystem.Domain.Features.Authentication.Profile;

public class ProfileService
{
    private readonly AppDbContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProfileService(AppDbContext db, IHttpContextAccessor httpContextAccessor)
    {
        _db = db;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<ProfileModel>> GetProfileAsync(ProfileRequestModel request)
    {
        ProfileModel model = new ProfileModel();

        // Get logged-in user phone number from session or claims
        //var userId = _httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
        // var cookie = _httpContextAccessor
        //     .HttpContext!
        //     .Request
        //     .Cookies;
        // userId = cookie.GetValue(SessionConstant.UserId);
        // sessionId = cookie.GetValue(SessionConstant.SessionId);
        // var accessToken = cookie.GetValue(SessionConstant.AccessToken);
        // if (!accessToken.IsNullOrEmpty())
        // {
        //     _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        // }

        // var sessionResult = _localStorage.GetAsync<MerchantSessionModel>("Key");
        // var sessionResult = await _localStorage.GetAsync<MerchantSessionModel>("MerchantSession");
        //
        // var session = sessionResult.Value;
        //
        // userId = session!.UserId!;
        // sessionId = session.SessionId!;
        // var accessToken = session.AccessToken!;
        try
        {
            var user = await _db.Tbl_Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == request.UserId && x.DelFlag == false);

            if (user == null)
            {
                return Result<ProfileModel>.FailureResult("User Not Found");
            }

            model.UserName = user.UserName;
            model.PhoneNo = user.Phone;
            model.Email = user.Email;
            model.ProfileImage = user.ProfileImage;

            if (System.IO.File.Exists(model.ProfileImage))
            {
                var imageBytes = System.IO.File.ReadAllBytes(model.ProfileImage);
                model.ImageStr = Convert.ToBase64String(imageBytes);
            }

            return Result<ProfileModel>.SuccessResult(model, "Successful");
        }
        catch (Exception ex)
        {
            return Result<ProfileModel>.FailureResult(ex.ToString());
        }
    }

    public async Task<Result<ProfileModel>> UpdateProfileAsync(ProfileRequestModel request)
    {
        ProfileModel model = new ProfileModel();
        var user = await _db.Tbl_Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == request.UserId && x.DelFlag == false);

        if (user == null)
        {
            return Result<ProfileModel>.FailureResult("User Not Found");
        }

        #region Write Image File

        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "C:\\FinanceTrack\\data");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // var fileName = $"{Guid.NewGuid()}{request.ImageExtension}";
        var fileName = Guid.NewGuid() + ".png";
        var filePath = Path.Combine(folderPath, fileName);

        var fileData = Convert.FromBase64String(request.ImageFile);
        await File.WriteAllBytesAsync(filePath, fileData);

        #endregion

        #region Save Image Path

        user.ProfileImage = filePath;
        _db.Entry(user).State = EntityState.Modified;
        await _db.SaveChangesAsync();

        #endregion

        return Result<ProfileModel>.SuccessResult(model, "Profile updated successfully!");

        // var user = await _db.Tbl_Users.FirstOrDefaultAsync(x => x.Phone == model.PhoneNo && x.DelFlag == false);
        //
        // if (user == null)
        //     return new ResponseModel { IsSuccess = false, Message = "User not found!" };
        //
        // // Update user details
        // user.UserName = model.UserName;
        // user.Email = model.Email;
        // // user.Address = model.Address;
        // user.ProfileImage = model.ProfileImage;
        //
        // _db.Tbl_Users.Update(user);
        // await _db.SaveChangesAsync();
        //
        // return new ResponseModel { IsSuccess = true, Message = "Profile updated successfully!" };
    }
}