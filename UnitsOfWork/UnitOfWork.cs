using Microsoft.AspNetCore.Identity;
using Models;
using Models.Registration;
using Models.Tables;
using Repositories.GenericRepository;
using System;


namespace UnitsOfWork
{
	public class UnitOfWork : IUnitOfWork
	{
		private MarketDBContext _context;

		public UnitOfWork(MarketDBContext context, UserManager<AppUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public void Commit()
		{
			_context.SaveChanges();
		}

		public IGenericRepository<AppUser> UserRepository
		{
			get
			{
				_userRepository = _userRepository ?? new GenericRepository<AppUser>(_context);
				return _userRepository;
			}
		}

		public IGenericRepository<Item> ItemRepository
		{
			get
			{
				_itemRepository = _itemRepository ?? new GenericRepository<Item>(_context);
				return _itemRepository;
			}
		}

		public IGenericRepository<Order> OrderRepository
		{
			get
			{
				_orderRepository = _orderRepository ?? new GenericRepository<Order>(_context);
				return _orderRepository;
			}
		}

		public IGenericRepository<OrderItem> OrderItemRepository
		{
			get
			{
				_orderItemRepository = _orderItemRepository ?? new GenericRepository<OrderItem>(_context);
				return _orderItemRepository;
			}
		}

		public IGenericRepository<Customer> CustomerRepository
		{
			get
			{
				_customerRepository = _customerRepository ?? new GenericRepository<Customer>(_context);
				return _customerRepository;
			}
		}

		public UserManager<AppUser> UserManager
		{
			get
			{
				return _userManager;
			}
		}


		private IGenericRepository<Order> _orderRepository;
		private IGenericRepository<OrderItem> _orderItemRepository;
		private IGenericRepository<Item> _itemRepository;
		private IGenericRepository<AppUser> _userRepository;
		private IGenericRepository<Customer> _customerRepository;
		private UserManager<AppUser> _userManager;


		#region Dispose
		private bool disposed = false;
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					_context.Dispose();
				}
			}
			this.disposed = true;
		}
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion
	}

}
