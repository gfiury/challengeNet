using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieModels.Models
{
    public class AppSettings
    {
        public required string SecretToken { get; set; }
        public required string TMDBApiUrl { get; set; }
        public required string TMDBReadToken { get; set; }
    }
}
