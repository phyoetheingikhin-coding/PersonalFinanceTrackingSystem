﻿using Microsoft.EntityFrameworkCore;
using PersonalFinanceTrackingSystem.Database.EfAppDbContextModels;
using PersonalFinanceTrackingSystem.Shared.Common;
using System.Text;
using System.Security.Cryptography;
using PersonalFinanceTrackingSystem.Shared;
using PersonalFinanceTrackingSystem.Domain.Features.Authentication.Login;


namespace PersonalFinanceTrackingSystem.Domain.Features.Authentication.Register;

public class RegisterService
{
    private readonly AppDbContext _db;

    public RegisterService(AppDbContext db)
    {
        _db = db;
    }
    public async Task<LoginResponseModel> Register(LoginRequestModel requestModel)
    {
        var model = new LoginResponseModel();

        try
        {
            #region Check Duplicate User With Phone 

            var user = await _db.Tbl_Users.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Phone == requestModel.PhoneNo &&
                x.DelFlag == false);
            if (user is not null)
            {
                model.Response = SubResponseModel.GetResponseMsg("PhoneNo is already exist", false);
                return model;
            }

            #endregion

            #region Save User

            //var passwordHashValue = HashPassword(requestModel.Password);
            string hashPassword =
                               requestModel.Password.ToSHA256HexHashString(requestModel.UserName);
            var newUser = new Tbl_User
            {
                UserId = Guid.NewGuid().ToString(),
                UserName = requestModel.UserName,
                Password = hashPassword,
                Phone = requestModel.PhoneNo,
                DelFlag = false,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
                
            };
            await _db.Tbl_Users.AddAsync(newUser);
            await _db.SaveChangesAsync();
            model.Response = SubResponseModel.GetResponseMsg("Your registration is successful", true);

            #endregion

        }
        catch (Exception ex)
        {

        }

        return model;
    }

    //public string HashPassword(string password)
    //{
    //    using var sha256 = SHA256.Create();
    //    byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
    //    return Convert.ToBase64String(hashedBytes);
    //}

}
