using IdentityCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
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
	public class AuthService : IAuthService
    {
		private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<AppUser> _userManager;
		private readonly ApplicationSettings _appSettings;

		public AuthService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IOptions<ApplicationSettings> applicationSettings)
		{
			_unitOfWork = unitOfWork;
			_userManager = userManager;
			_appSettings = applicationSettings.Value;
		}



		public GetJwtResult GetJwtToken(LoginRegisterModel loginModel)
		{
            var user = _unitOfWork.UserRepository
                .All()
                .Where(x => x.UserName == loginModel.UserName)
                .Single();

            var isPassvaordValid = _userManager.CheckPasswordAsync(user, loginModel.Password).Result;

            if (!isPassvaordValid)
                return new GetJwtResult
                {
                    Success = false
                };


            var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();

            IdentityOptions _options = new IdentityOptions();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim("UserID", user.Id.ToString()),
                    new Claim(_options.ClaimsIdentity.RoleClaimType, role)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWTSecret)),
                SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);


            return new GetJwtResult
			{
				Success = true,
				Token = token
			};
		}

        public List<AppUser> GetAllUsers()
        {
         return _unitOfWork.UserRepository.All().ToList<AppUser>();
        }
	}
}
