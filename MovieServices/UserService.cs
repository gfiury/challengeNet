
using MovieEntityFramework;
using MovieEntityFramework.Interfaces;
using MovieModels.Interfaces;
using MovieModels.Models;
using MovieModels.Models.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MovieServices
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> GetByEmailAndPassword(string email, string password)
        {
            return await _userRepository.GetByEmailAndPassword(email, password);
        }

        public async Task<User?> GetById(int id)
        {
            return await _userRepository.GetById(id);
        }

        public async Task<int?> CreateUser(UserArguments userArguments)
        {
            if (!ValidateNewUser(userArguments)) return null;

            var user = new User { Name = userArguments.Name, LastName = userArguments.LastName, Email = userArguments.Email.ToLower(), Password = userArguments.Password };
            return await _userRepository.CreateNewUser(user);
        }

        private bool ValidateNewUser(UserArguments user)
        {
            // Validate Credentials
            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                return false;
            }

            // Validate User Data
            if (string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.LastName))
            {
                return false;
            }

            // Extra validations
            if (user.Password.Length < 4)
            {
                return false;
            }

            return true;
        }
    }
}
