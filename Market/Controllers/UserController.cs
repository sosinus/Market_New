using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Registration;
using Models.RepositoryResults;
using Models.Tables;
using Service;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnitsOfWork;


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

        // GET: /<controller>/
        [HttpGet]
        // [Authorize(Roles = "Manager")]
        public IActionResult GetUsers()
        {
            var users = _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpPut]
        public IActionResult UpdateUser(AppUser appUser)
        {
            var result = _userService.UpdateUser(appUser);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateUser(FrontUser frontUser)
        {
            CreateUserResult result = new CreateUserResult();
            LoginRegisterModel loginRegisterModel = new LoginRegisterModel() { UserName = frontUser.UserName, Password = frontUser.Password };
            FrontCustomer frontCustomer = new FrontCustomer() { Address = frontUser.Address, Name = frontUser.Name };
            var userCreationResult = _userService.CreateUser(loginRegisterModel, "User");
            var customerCreationResult = _customerService.CreateCustomer(frontCustomer, userCreationResult.UserId);
            if (customerCreationResult.succeeded)
            {
                result.Success = true;
            }
            return Ok(result);
        }

        [HttpDelete("{userId}")]
        public IActionResult DeleteUser(string userId)
        {
            var result = _userService.DeleteUser(userId);
            return Ok(result);
        }


    }
}
