using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieModels.Models.Arguments
{
    public class PreferencesArguments
    {
        [DefaultValue(2000)]
        public required int ReleaseYear { get; set; }
    }
}
