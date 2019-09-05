using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.Registration;
using Service;
using System;
using System.Text;
using UnitsOfWork;

namespace Market.App_Start
{
	public static class Bootstraper
	{
		public static void RegisterServices(this IServiceCollection services)
		{
			services.AddScoped<IItemService, ItemService>();
			services.AddScoped<IAuthService, AuthService>();
			services.AddScoped<ICustomerService, CustomerService>();
			services.AddScoped<IOrderService, OrderService>();
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IImageService, ImageService>();
		}

		public static void RegisterRepositories(this IServiceCollection services)
		{

		}

		public static void RegisterUnitOfWork(this IServiceCollection services)
		{
			services.AddScoped<IUnitOfWork, UnitOfWork>();
		}

		public static void RegisterIdentity(this IServiceCollection services, IConfiguration Configuration)
		{
			services
				.AddDefaultIdentity<AppUser>()
				.AddRoles<IdentityRole>()
				.AddEntityFrameworkStores<MarketDBContext>();
			services.Configure<IdentityOptions>(options =>
			{
				options.Password.RequireDigit = false;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireLowercase = false;
				options.Password.RequireUppercase = false;
				options.Password.RequiredLength = 4;
			});

			var key = Encoding.UTF8.GetBytes(Configuration["ApplicationSettings:JWTSecret"]);
			services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(x =>
			{
				x.RequireHttpsMetadata = false;
				x.SaveToken = false;
				x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false,
					ClockSkew = TimeSpan.Zero
				};
			});
		}
	}
}
