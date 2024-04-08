using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieModels.Models
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }


        public UserResponse(User user, string token)
        {
            Id = user.Id;
            Name = user.Name;
            LastName = user.LastName;
            Email = user.Email;
            Token = token;
        }
    }
}
