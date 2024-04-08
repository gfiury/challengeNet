using MovieModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieEntityFramework.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAndPassword(string email, string password);
        Task<User?> GetById(int id);
        Task<int?> CreateNewUser(User user);
    }
}
