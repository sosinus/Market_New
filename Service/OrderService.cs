using Microsoft.EntityFrameworkCore;
using Models;
using Models.Tables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnitsOfWork;

namespace Service
{
	public class OrderService : IOrderService
	{
		private readonly IUnitOfWork _unitOfWork;
		public OrderService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public OperationResult AddOrder(OrderItem[] orderItems, string userId)
		{
			var customer = _unitOfWork.UserRepository.All().Include(u => u.Customer).Single(u => u.Id == userId).Customer;

			Order order = new Order()
			{
				Customer_Id = customer.Id,
				Order_Date = DateTime.Now.Date,
				Status = "Новый"
			};
			order = _unitOfWork.OrderRepository.Add(order);

			int orderId = order.Id;

			foreach (var orderItem in orderItems)
			{
				orderItem.Order_Id = orderId;
				orderItem.Item = null;
				var discount = customer.Discount;
				var price = orderItem.Item_Price;
				orderItem.Item_Price = price - (orderItem.Item_Price * discount) / 100;
			}
			_unitOfWork.OrderItemRepository.AddRange(orderItems);
			try
			{
				_unitOfWork.Commit();
				return new OperationResult { Succeeded = true };
			}
			catch (Exception ex)
			{
				return new OperationResult { Succeeded = false, Message = ex.Message };
			}

		}


		public List<Order> GetUserOrders(string userId)
		{
			var customerId = _unitOfWork.UserRepository.All().Single(u => u.Id == userId).Customer_Id;
			var orders = _unitOfWork.OrderRepository.All().Include(o => o.OrderItems)
				.Where(o => o.Customer_Id == customerId)
				  .Include(o => o.OrderItems)
				  .ThenInclude(oi => oi.Item)
				  .ToList<Order>();
			attachImages(orders);
			return orders;
		}

		public List<Order> GetAllOrders()
		{
			var orders = _unitOfWork.OrderRepository.All()
				  .Include(o => o.OrderItems)
				  .ThenInclude(oi => oi.Item)
				  .ToList<Order>();
			attachImages(orders);
			return orders;
		}

		List<Order> attachImages(List<Order> orders)
		{
			foreach (var order in orders)
			{
				foreach (var orderItem in order.OrderItems)
				{
					string folderName = Path.Combine("Resources", "Images", orderItem.Item.Id.ToString());
					orderItem.Item.Image = Directory.GetFiles(folderName)[0];
				}

			}
			return orders;
		}

		public OperationResult UpdateOrder(Order order)
		{
			foreach (var orderItem in order.OrderItems)
			{
				orderItem.Id = default;
				orderItem.Item = null;
				orderItem.Order = null;
			}
			var orderItems = _unitOfWork.OrderItemRepository.All().Where(o => o.Order_Id == order.Id).ToList();
			foreach (var orderIt in orderItems)
			{
				orderIt.Order = null;
			}
			_unitOfWork.OrderItemRepository.RemoveRange(orderItems.ToArray());
			try
			{
				_unitOfWork.Commit();
				_unitOfWork.OrderRepository.Update(order);
				_unitOfWork.Commit();
				return new OperationResult { Succeeded = true };
			}
			catch (Exception ex)
			{
				return new OperationResult { Succeeded = false, Message = ex.Message };
			}
		}
	}
}
