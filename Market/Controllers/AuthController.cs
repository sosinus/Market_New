using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Registration;
using Models.RepositoryResults;
using Models.Tables;
using Service;
using System.Linq;


namespace Market.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _service;
        private readonly IUserService _userService;

        public AuthController(IAuthService service, IUserService userService)
        {
            _service = service;
            _userService = userService;
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginRegisterModel model)
        {
            var result = _service.GetJwtToken(model);
            return Ok(result);

        }

        [HttpGet]
        public IActionResult LoadPage()
        {
            LoadPageResult result = new LoadPageResult();
            if (_service.GetAllUsers().Count == 0)
                result.HasDefaultUser = false;
            else
                result.HasDefaultUser = true;
            return Ok(result);
        }

        [HttpPost]
        [Route("CreateUser")]
        public IActionResult Register(LoginRegisterModel user)
        {
            CreateUserResult result = _userService.CreateUser(user, "User");
            return Ok(result);
        }

        [HttpPost]
        [Route("CreateManager")]
        public IActionResult RegisterManager(LoginRegisterModel user)
        {
            CreateUserResult result = new CreateUserResult();
            if (_userService.GetAllUsers().Count == 0)
                result = _userService.CreateUser(user, "Manager");
            return Ok(result);
        }



    }
}