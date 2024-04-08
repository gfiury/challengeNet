using Microsoft.EntityFrameworkCore;
using MovieEntityFramework.Interfaces;
using MovieModels.Models;
using MovieModels.Models.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieEntityFramework
{
    public class PreferenceRepository : IPreferenceRepository
    {
        private readonly IApplicationContext _applicationContext;

        public PreferenceRepository(IApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public async Task<Preferences?> GetById(int id)
        {
            return await _applicationContext.Preferences.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int?> CreatePreferences(Preferences preferences)
        {
            _applicationContext.Preferences.Add(preferences);
            await _applicationContext.SaveChangesAsync();
            return preferences.Id;
        }

        public async Task<int?> UpdatePreferences(PreferencesArguments preferencesArguments, int idPreference)
        {
            var preferences = await GetById(idPreference);
            if (preferences == null) return null;

            preferences.ReleaseYear = preferencesArguments.ReleaseYear;
            await _applicationContext.SaveChangesAsync();
            return preferences.Id;
        }
    }
}
