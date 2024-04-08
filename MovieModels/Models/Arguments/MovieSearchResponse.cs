using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MovieModels.Models.Arguments
{
    public class MovieSearchResponse
    {
        [JsonProperty("results")]
        public List<Movie> Movies { get; set; }

        [JsonProperty("page")]
        public int PageNumber { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        [JsonProperty("total_results")]
        public int TotalResults { get; set; }
    }
}
