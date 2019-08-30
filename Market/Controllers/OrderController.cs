using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Tables;
using Service;
using System.Linq;

namespace Market.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		private readonly IOrderService _orderService;
		public OrderController(IOrderService orderService)
		{
			_orderService = orderService;
		}

		[HttpPost]
		[Authorize(Roles = "User")]
		public IActionResult AddOrder(OrderItem[] orderItems)
		{
			string userId = User.Claims.SingleOrDefault(c => c.Type == "UserID").Value;
			var result = _orderService.AddOrder(orderItems, userId);
			return Ok(result);
		}

		[HttpGet]
		[Authorize(Roles = "User")]
		public IActionResult GetOrdersForUser()
		{
			if (User.Claims.SingleOrDefault(c => c.Type == "UserID") != null)
			{
				string userId = User.Claims.SingleOrDefault(c => c.Type == "UserID").Value;
				var orders = _orderService.GetUserOrders(userId);
				return Ok(orders);
			}
			return BadRequest();
		}

		[HttpGet]
		[Route("OrderList")]
		[Authorize(Roles = "Manager")]
		public IActionResult GetOrdersForManager()
		{
			var orders = _orderService.GetAllOrders();
			return Ok(orders);
		}

		[HttpPut]
		[Authorize(Roles = "Manager")]
		public IActionResult UpdateOrder(Order order)
		{
			var result = _orderService.UpdateOrder(order);
			if (result.Succeeded)
				return Ok();
			else
				return BadRequest();
		}

		[HttpPost]
		[Route("Cancel")]
		[Authorize(Roles = "User")]
		public IActionResult CancelOrder(Order order)
		{
			order.Status = "Отменен";
			var result = _orderService.UpdateOrder(order);
			if (result.Succeeded)
				return Ok();
			else
				return BadRequest();
		}

	}
}