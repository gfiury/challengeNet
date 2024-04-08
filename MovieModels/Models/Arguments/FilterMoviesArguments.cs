using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieModels.Models.Arguments
{
    public class FilterMoviesArguments
    {
        [JsonProperty("query")]
        public string Title { get; set; }
        [JsonProperty("year")]
        [DefaultValue(2000)]
        public int ReleaseYear { get; set; }
        public bool ApplyPreferences { get; set; }
    }
}
