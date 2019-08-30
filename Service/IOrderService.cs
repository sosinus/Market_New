using Models;
using Models.Tables;
using System.Collections.Generic;

namespace Service
{
	public interface IOrderService
	{
		OperationResult AddOrder(OrderItem[] orderItems, string userId);

		List<Order> GetUserOrders(string userId);

		List<Order> GetAllOrders();

		OperationResult UpdateOrder(Order order);
	}
}
