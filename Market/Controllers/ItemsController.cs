using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Tables;
using Service;
using System.IO;
using System.Linq;

namespace Market.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ItemsController : ControllerBase
	{
		private readonly IItemService _itemService;
		public ItemsController(IItemService itemService)
		{

			_itemService = itemService;
		}

		[HttpGet]
		public IActionResult Get()
		{
			return Ok(_itemService.GetAllItems());
		}

		[HttpPost]
		[Authorize(Roles = "Manager")]
		public IActionResult NewItem(Item item)
		{
			string userId = User.Claims.SingleOrDefault(c => c.Type == "UserID").Value;
			var result = _itemService.AddNewItem(item, userId);
			return Ok(result);
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = "Manager")]
		public IActionResult DeleteItem(int id)
		{
			var result = _itemService.DeleteItem(id);			
			return Ok(result);
		}

		[HttpPut]
		[Authorize(Roles = "Manager")]
		public IActionResult UpdateItem(Item item)
		{
			string userId = User.Claims.SingleOrDefault(c => c.Type == "UserID").Value;
			var result = _itemService.UpdateItem(item, userId);
			return Ok(result);
		}

	}
}
