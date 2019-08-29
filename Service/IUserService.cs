using Models.Registration;
using Models.RepositoryResults;
using Models.Tables;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
   public interface IUserService
    {
        List<AppUser> GetAllUsers();

        UserResult UpdateUser(AppUser user);

        CreateUserResult CreateUser(LoginRegisterModel loginRegisterModel, string Role);

        UserResult DeleteUser(string userId);
    }
}
