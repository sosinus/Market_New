using IdentityCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.Registration;
using Models.RepositoryResults;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UnitsOfWork;

namespace Service
{
	public interface IAuthService
    {
        GetJwtResult GetJwtToken(LoginRegisterModel loginModel);

        List<AppUser> GetAllUsers();

	}
}
