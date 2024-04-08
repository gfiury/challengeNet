using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieModels.Models
{
    public class UserRequest
    {
        [DefaultValue("Empty")]
        public required string Email { get; set; }
        [DefaultValue("Empty")]
        public required string Password { get; set; }
    }
}
