using Microsoft.AspNetCore.Mvc;
using Models.Registration;
using Service;


namespace Market.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : Controller
	{
		private readonly IAuthService _authService;
		private readonly IUserService _userService;
		
		public AuthController(IAuthService authService, IUserService userService)
		{
			_authService = authService;
			_userService = userService;
		}

		[HttpPost]
		[Route("Login")]
		public IActionResult Login(LoginRegisterModel model)
		{
			return Ok(_authService.GetJwtToken(model));
		}

		[HttpGet]
		public IActionResult LoadPage()
		{
			return Ok(_authService.GetAllUsers().Count != 0);
		}

		[HttpPost]
		[Route("CreateUser")]
		public IActionResult Register(LoginRegisterModel user)
		{
			return Ok(_userService.CreateUser(user, "User"));
		}

		[HttpPost]
		[Route("CreateManager")]
		public IActionResult RegisterManager(LoginRegisterModel user)
		{
			return Ok(_userService.CreateUser(user, "Manager"));
		}
	}
}