using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.Registration;
using Models.RepositoryResults;
using Models.Tables;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IUMRepository
    {
        CreateUserResult CreateUser(LoginRegisterModel user, string role);
    }

    public class UMRepository : IUMRepository
    {
        private readonly UserManager<AppUser> _userManager;              
        public UMRepository(UserManager<AppUser> userManager)
        {
            _userManager = userManager; 
        }

        public CreateUserResult CreateUser(LoginRegisterModel user, string role)
        {
            CreateUserResult result = new CreateUserResult();
            AppUser appuser = new AppUser() { UserName = user.UserName, Email = user.Email };
            try
            {
                if (_userManager.Users.SingleOrDefault(u => u.UserName == user.UserName) != null)
                {
                    result.Message = "Пользователь с таким логином уже существует";
                    result.Success = false;
                    result.IsAlreadyExist = true;
                }
                else
                {
                    IdentityResult userCreation = _userManager.CreateAsync(appuser, user.Password).Result;
                    IdentityResult inRoleAdding = _userManager.AddToRoleAsync(appuser, role).Result;
                    if (userCreation.Succeeded)
                    {
                        if (inRoleAdding.Succeeded)
                        {
                            result.IsAlreadyExist = false;
                            result.Success = true;
                            result.Message = "Пользователь успешно создан";
                            result.UserId = appuser.Id;
                        }
                        else
                        {
                            AppUser appUser = _userManager.Users.SingleOrDefault(u => u.UserName == user.UserName);
                            _userManager.DeleteAsync(appUser);
                            result.IsAlreadyExist = false;
                            result.Other = true;
                            result.Message = "Не удалось создать пользователя";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = "Не удалось создать пользователя " + ex;
            }
            return result;
        }


    }

}


