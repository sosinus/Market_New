using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Tables;
using Service;
using System.Linq;

namespace Market.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CustomerController : Controller
	{
		private readonly ICustomerService _customerService;
		public CustomerController(ICustomerService service)
		{
			_customerService = service;
		}

		[HttpGet]
		public IActionResult GetCustomer()
		{
			string userId = User.Claims.SingleOrDefault(c => c.Type == "UserID").Value;
			Customer result = _customerService.GetCustomer(userId);
			return Ok(result);
		}

		[HttpPost]
		[Authorize]
		public IActionResult AddCustomer(FrontCustomer frontCustomer)
		{
			string userId = User.Claims.SingleOrDefault(c => c.Type == "UserID").Value;
			var result = _customerService.CreateCustomer(frontCustomer, userId);
			if (result)
				return Ok();
			else return BadRequest();
		}

		[HttpGet]
		[Route("GetDiscount")]
		public IActionResult GetDiscount()
		{
			int discount = 0;
			if (User.Claims.Count() > 1)
			{
				var userId = User.Claims.SingleOrDefault(c => c.Type == "UserID").Value;
				discount = _customerService.GetDiscount(userId);
			}
			return Ok(discount);
		}
	}
}