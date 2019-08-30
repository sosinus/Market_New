using Models;
using Models.Registration;
using System.Collections.Generic;

namespace Service
{
	public interface IAuthService
	{
		OperationResult GetJwtToken(LoginRegisterModel loginModel);

		List<AppUser> GetAllUsers();

	}
}
