using Microsoft.AspNetCore.Identity;
using Models.Registration;
using Models.Tables;
using Repositories.GenericRepository;


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

		UserManager<AppUser> UserManager { get; }
	}
}
