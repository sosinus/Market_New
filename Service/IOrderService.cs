using Models.RepositoryResults;
using Models.Tables;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
   public interface IOrderService
    {
        AddOrderResult AddOrder(OrderItem[] orderItems, string userId);

        List<Order> GetUserOrders(string userId);

        List<Order> GetAllOrders();

        bool UpdateOrder(Order order);
    }
}
