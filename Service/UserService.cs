using Microsoft.EntityFrameworkCore;
using Models.Registration;
using Models.RepositoryResults;
using Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public UserResult UpdateUser(AppUser user)
        {
            UserResult result = new UserResult();
            _unitOfWork.UserRepository.Update(user);
            _unitOfWork.Commit();
            result.succeeded = true;
            return result;
        }

        public CreateUserResult CreateUser(LoginRegisterModel loginRegisterModel, string role)
        {
            var userCreationResult = _unitOfWork.UMRepository.CreateUser(loginRegisterModel, role);
            return userCreationResult;
        }

        public UserResult DeleteUser(string userId)
        {
            UserResult result = new UserResult();
            var user = _unitOfWork.UserRepository.All().Include(u => u.Customer).Single(u => u.Id == userId);
            _unitOfWork.UserRepository.Delete(user);
            if (user.Customer != null)
                _unitOfWork.CustomerRepository.Delete(user.Customer);
            _unitOfWork.Commit();
            result.succeeded = true;
            return result;
        }
    }
}
