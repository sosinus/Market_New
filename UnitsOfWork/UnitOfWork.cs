using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Models;
using Models.Registration;
using Models.Tables;
using Repositories;
using Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace UnitsOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private MarketDBContext _context;
        private readonly UserManager<AppUser> _userManager;

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

        public IUMRepository UMRepository
        {
            get
            {
                _UMRepository = _UMRepository ?? new UMRepository(_userManager);
                return _UMRepository;
            }
        }

        private IGenericRepository<Order> _orderRepository;
        private IGenericRepository<OrderItem> _orderItemRepository;
        private IGenericRepository<Item> _itemRepository;
        private IGenericRepository<AppUser> _userRepository;
        private IGenericRepository<Customer> _customerRepository;
        private IUMRepository _UMRepository;


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
