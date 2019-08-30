using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Registration;
using Service;
using System;

namespace Market.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;
		private readonly ICustomerService _customerService;

		public UserController(IUserService userService, ICustomerService customerService)
		{
			_userService = userService;
			_customerService = customerService;
		}

		[HttpGet]
		[Authorize(Roles = "Manager")]
		public IActionResult GetUsers()
		{
			return Ok(_userService.GetAllUsers());
		}

		[HttpPut]
		[Authorize(Roles = "Manager")]
		public IActionResult UpdateUser(AppUser appUser)
		{
			var result = _userService.UpdateUser(appUser);
			return Ok(result);
		}

		[HttpPost]
		[Authorize(Roles = "Manager")]
		public IActionResult CreateUser(FrontUser frontUser)
		{

			LoginRegisterModel loginRegisterModel = new LoginRegisterModel() { UserName = frontUser.UserName, Password = frontUser.Password };
			FrontCustomer frontCustomer = new FrontCustomer() { Address = frontUser.Address, Name = frontUser.Name };

			string userId = Convert.ToString(_userService.CreateUser(loginRegisterModel, "User").Data);
			return Ok(_customerService.CreateCustomer(frontCustomer, userId));
		}

		[HttpDelete("{userId}")]
		[Authorize(Roles = "Manager")]
		public IActionResult DeleteUser(string userId)
		{
			var result = _userService.DeleteUser(userId);
			return Ok(result);
		}
	}
}
