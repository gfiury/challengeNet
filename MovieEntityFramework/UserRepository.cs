using Microsoft.EntityFrameworkCore;
using MovieEntityFramework.Interfaces;
using MovieModels.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieEntityFramework
{
    public class UserRepository : IUserRepository
    {
        private readonly IApplicationContext _applicationContext;

        public UserRepository(IApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
            var defaultUser = new User { Email = "challenge@gmail.com", Name = "Challenge", LastName = "Codigo del Sur", Password = "challenge" };
            _applicationContext.Users.Add(defaultUser);
            _applicationContext.SaveChanges();
        }

        public async Task<User?> GetByEmailAndPassword(string email, string password)
        {
            return await _applicationContext.Users.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(email.ToLower()) && x.Password.Equals(password));
        }

        public async Task<User?> GetById(int id)
        {
            return await _applicationContext.Users.Include(x => x.Preferences).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int?> CreateNewUser(User user)
        {
            _applicationContext.Users.Add(user);
            await _applicationContext.SaveChangesAsync();
            return user.Id;
        }

    }
}
