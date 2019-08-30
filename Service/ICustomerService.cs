using Models;
using Models.Tables;

namespace Service
{
	public interface ICustomerService
	{
		Customer GetCustomer(string userId);

		bool CreateCustomer(FrontCustomer frontCustomer, string userId);

		int GetDiscount(string userId);
	}

}
