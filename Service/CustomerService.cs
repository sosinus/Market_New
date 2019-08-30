using Models;
using Models.Tables;
using System;
using System.Linq;
using UnitsOfWork;

namespace Service
{

	public class CustomerService : ICustomerService
	{
		private readonly IUnitOfWork _unitOfWork;
		public CustomerService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public Customer GetCustomer(string userId)
		{
			var customerId = _unitOfWork.UserRepository.All().Single(u => u.Id == userId).Customer_Id;
			return _unitOfWork.CustomerRepository.All().SingleOrDefault(c => c.Id == customerId);
		}

		public bool CreateCustomer(FrontCustomer frontCustomer, string userId)
		{
			Customer customer = new Customer()
			{
				Name = frontCustomer.Name,
				Address = frontCustomer.Address,
				Code = "0"
			};

			try
			{
				var custr = _unitOfWork.CustomerRepository.Add(customer);
				_unitOfWork.Commit();
				custr.Code = string.Format("{0:0000}", custr.Id) + "-" + DateTime.Now.Year.ToString();
				_unitOfWork.CustomerRepository.Update(custr);
				var user = _unitOfWork.UserRepository.All().Single(u => u.Id == userId);
				user.Customer_Id = custr.Id;
				_unitOfWork.UserRepository.Update(user);
				_unitOfWork.Commit();
				return true;
			}
			catch
			{
				return false;
			}
		}

		public int GetDiscount(string userId)
		{
			var customerId = _unitOfWork.UserRepository.All().Single(u => u.Id == userId).Customer_Id;
			return _unitOfWork.CustomerRepository.All().SingleOrDefault(c => c.Id == customerId)?.Discount ?? 0;
		}

	}

}

