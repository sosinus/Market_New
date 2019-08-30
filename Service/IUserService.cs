using Models;
using Models.Registration;
using System.Collections.Generic;

namespace Service
{
	public interface IUserService
	{
		List<AppUser> GetAllUsers();

		OperationResult UpdateUser(AppUser user);

		OperationResult CreateUser(LoginRegisterModel loginRegisterModel, string Role);

		OperationResult DeleteUser(string userId);
	}
}
