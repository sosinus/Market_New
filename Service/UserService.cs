using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using UnitsOfWork;

namespace Service
{
	public class UserService : IUserService
	{
		private readonly IUnitOfWork _unitOfWork;
		public UserService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public List<AppUser> GetAllUsers()
		{
			return _unitOfWork.UserRepository.All().Include(u => u.Customer).ToList<AppUser>();
		}

		public OperationResult UpdateUser(AppUser user)
		{
			try
			{
				_unitOfWork.UserRepository.Update(user);
				_unitOfWork.Commit();
				return new OperationResult { Succeeded = true };
			}
			catch (Exception ex)
			{
				return new OperationResult { Succeeded = false, Message = ex.Message };
			}
		}

		public OperationResult DeleteUser(string userid)
		{
			try
			{
				var user = _unitOfWork.UserRepository.All().Include(u => u.Customer).Single(u => u.Id == userid);
				_unitOfWork.UserRepository.Delete(user);
				if (user.Customer != null)
					_unitOfWork.CustomerRepository.Delete(user.Customer);
				_unitOfWork.Commit();
				return new OperationResult { Succeeded = true };
			}
			catch (Exception ex)
			{
				return new OperationResult { Succeeded = false, Message = ex.Message };
			}
		}
		
		public class UserAlreadyExistsException : Exception { };

		public OperationResult CreateUser(LoginRegisterModel user, string role)
		{
			AppUser appuser = new AppUser() { UserName = user.UserName, Email = user.Email };

			if (_unitOfWork.UserManager.Users.SingleOrDefault(u => u.UserName == user.UserName) != null)
			{
				return new OperationResult { Message = "Пользователь с таким логином уже существует" };
			}
			else
			{
				IdentityResult userCreation = _unitOfWork.UserManager.CreateAsync(appuser, user.Password).Result;
				if (userCreation.Succeeded)
				{
					IdentityResult inRoleAdding = _unitOfWork.UserManager.AddToRoleAsync(appuser, role).Result;
					if (inRoleAdding.Succeeded)
					{
						return new OperationResult { Message = "Пользователь успешно создан", Data = appuser.Id, Succeeded = true };
					};
				}

				_unitOfWork.UserManager.DeleteAsync(appuser);
				return new OperationResult { Message = "Не удалось создать пользователя" };

			}
		}
	}
}

