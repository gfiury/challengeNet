using MovieEntityFramework;
using MovieEntityFramework.Interfaces;
using MovieModels.Interfaces;
using MovieModels.Models;
using MovieModels.Models.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieServices
{
    public class PreferenceService : IPreferenceService
    {
        private readonly IPreferenceRepository _preferenceRepository;

        public PreferenceService(IPreferenceRepository preferenceRepository)
        {
            _preferenceRepository = preferenceRepository;
        }

        public async Task<int?> CreatePreferences(PreferencesArguments preferenceArguments, int userId)
        {
            var preferences = new Preferences { ReleaseYear = preferenceArguments.ReleaseYear, IdUser = userId };
            return await _preferenceRepository.CreatePreferences(preferences);
        }

        public async Task<int?> UpdatePreferences(PreferencesArguments preferenceArguments, int preferencesId)
        {
            return await _preferenceRepository.UpdatePreferences(preferenceArguments, preferencesId);
        }
    }
}
