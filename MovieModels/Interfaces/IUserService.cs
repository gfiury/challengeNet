using MovieModels.Models;
using MovieModels.Models.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieModels.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetById(int id);
        Task<User?> GetByEmailAndPassword(string email, string password);
        Task<int?> CreateUser(UserArguments user);
    }
}
