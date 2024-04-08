using MovieModels.Models;
using MovieModels.Models.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieModels.Interfaces
{
    public interface IPreferenceService
    {
        Task<int?> CreatePreferences(PreferencesArguments preferences, int idUser);
        Task<int?> UpdatePreferences(PreferencesArguments preferences, int idPreferences);
    }
}
