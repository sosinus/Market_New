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
    public interface IUnitOfWork 
    {
		void Commit();

        IGenericRepository<AppUser> UserRepository { get; }

        IGenericRepository<Customer> CustomerRepository { get; }

        IGenericRepository<Order> OrderRepository { get; }

        IGenericRepository<OrderItem> OrderItemRepository { get; }

        IGenericRepository<Item> ItemRepository { get; }

        IUMRepository UMRepository { get; }
	}
}
