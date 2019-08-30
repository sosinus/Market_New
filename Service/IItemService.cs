using Models;
using Models.Tables;
using System.Collections.Generic;

namespace Service
{
	public interface IItemService
	{
		List<Item> GetAllItems();
		OperationResult AddNewItem(Item item, string userId);
		OperationResult DeleteItem(int id);
		OperationResult UpdateItem(Item item, string userId);
	}
}
