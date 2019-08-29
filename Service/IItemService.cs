using Models.RepositoryResults;
using Models.Tables;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
    public interface IItemService
    {
        List<Item> GetAllItems();
        ItemResult AddNewItem(Item item, string userId);
        ItemResult DeleteItem(int id);
        ItemResult UpdateItem(Item item, string userId);
    }
}
