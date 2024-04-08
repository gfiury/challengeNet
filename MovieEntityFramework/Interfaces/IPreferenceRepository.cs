using MovieModels.Models;
using MovieModels.Models.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieEntityFramework.Interfaces
{
    public interface IPreferenceRepository
    {
        Task<Preferences?> GetById(int id);
        Task<int?> CreatePreferences(Preferences preferences);
        Task<int?> UpdatePreferences(PreferencesArguments preferencesArguments, int idPreference);
    }
}
