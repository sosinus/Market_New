using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.RepositoryResults;
using Models.Tables;
using Service;
using System;
using System.IO;
using System.Linq;
using UnitsOfWork;

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
            var items = _itemService.GetAllItems();           
            return Ok(items);
        }



        [HttpPost]
        [Authorize(Roles = "Manager")]
        public IActionResult NewItem(Item item)
        {
            string userId = User.Claims.SingleOrDefault(c => c.Type == "UserID").Value;
            ItemResult result = _itemService.AddNewItem(item, userId);            
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public IActionResult DeleteItem(int id)
        {
            ItemResult result = _itemService.DeleteItem(id);
            string imgPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Images", id.ToString());
            if (Directory.Exists(imgPath))
                Directory.Delete(imgPath, true);
            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = "Manager")]
        public IActionResult UpdateItem(Item item)
        {
            string userId = User.Claims.SingleOrDefault(c => c.Type == "UserID").Value;
            ItemResult result = _itemService.UpdateItem(item, userId);            
            return Ok(result);
        }

    }
}
