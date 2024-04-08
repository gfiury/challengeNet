using Microsoft.AspNetCore.Mvc;
using MovieModels.Interfaces;
using System.Net.Http.Headers;
using System.Net;
using System.Runtime;
using MovieModels.Models;
using Microsoft.Extensions.Options;
using MovieModels.Models.Arguments;
using Newtonsoft.Json;
using MovieAPI.Security;
using MovieServices;
using System.Security.Claims;
using Swashbuckle.AspNetCore.Annotations;

namespace MovieAPI.Controllers
{
    [AuthorizeRequired]
    [ApiController]
    [Route("api/movies")]
    public class MoviesController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        public readonly IUserService _userService;
        private readonly IPreferenceService _preferenceService;
        public MoviesController(IOptions<AppSettings> appSettings, IUserService userService, IPreferenceService preferenceService)
        {
            _appSettings = appSettings.Value;
            _userService = userService;
            _preferenceService = preferenceService;
        }

        // Create Client to connect to the TMDB
        protected HttpClient CreateTMDBClient()
        {
            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = false,
                UseCookies = false,
                UseDefaultCredentials = true,
                AutomaticDecompression = DecompressionMethods.GZip,
            };

            var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _appSettings.TMDBReadToken);
            client.BaseAddress = new Uri(_appSettings.TMDBApiUrl);

            return client;
        }

        // Helper function to Join the Query params to the TMDB Endpoint
        protected string CreateEndpointWithQueryParams(string endpoint, IDictionary<string, object> filters)
        {
            string paramsQuery = filters.Any()
                ? string.Join("&", filters.Select(x => $"{x.Key}={WebUtility.UrlEncode(x.Value.ToString())}"))
                : string.Empty;

            if (string.IsNullOrWhiteSpace(paramsQuery) == false)
            {
                endpoint += $"?{paramsQuery}";
            }

            return endpoint;
        }

        [HttpPost, Route("searchTitleYear")]
        [SwaggerOperation(Summary = "Search Movie Titles from TMDB",
                          Description = "Pick a Title Name and a Release Year to Filter"
        )]
        [SwaggerResponse(200, "Returns a List of Pages and a list of Movies")]
        [SwaggerResponse(400, "")]
        public async Task<IActionResult> SearchByTitleAndYear([FromBody] FilterMoviesArguments arguments)
        {
            try
            {
                using HttpClient clientTMDB = CreateTMDBClient();

                const string tmdbEndpoint = "search/movie";

                if (arguments.ApplyPreferences)
                {
                    var user = (User?)HttpContext.Items["User"];
                    if (user?.Preferences != null)
                    {
                        var fullUser = await _userService.GetById(user.Id);
                        arguments.ReleaseYear = fullUser?.Preferences?.ReleaseYear ?? arguments.ReleaseYear;
                    }
                    else
                    {
                        // Continue without Preferences
                    }
                }

                var argumentsJson = JsonConvert.SerializeObject(arguments);
                var filters = JsonConvert.DeserializeObject<IDictionary<string, object>>(argumentsJson);
                filters?.Remove(nameof(arguments.ApplyPreferences));

                string query = CreateEndpointWithQueryParams(tmdbEndpoint, filters);

                HttpResponseMessage response = await clientTMDB.GetAsync(query).ConfigureAwait(false);

                string jsonResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.IsSuccessStatusCode == false)
                {
                    throw new Exception($"Error from TMDB response");
                }

                var result = JsonConvert.DeserializeObject<MovieSearchResponse>(jsonResponse);

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Needs loggin the error
                return Problem($"message=Unknown error while trying to search for Titles, error={ex.Message}");
            }
            
        }

        [HttpPost, Route("savePreferences")]
        [SwaggerOperation(Summary = "Saves or Updates Users Preferences to use later in others Endpoints",
                          Description = "Saves the Release Year as a Preference to use in Filters"
        )]
        [SwaggerResponse(200, "Saving or Updating the Preferences were successully")]
        [SwaggerResponse(401, "Unauthorized, an issue with the Session User")]
        public async Task<IActionResult> SaveUpdatePreferences([FromBody] PreferencesArguments arguments)
        {
            try
            {
                var user = (User?)HttpContext.Items["User"];

                if (user == null)
                    return Unauthorized();

                var fullUser = await _userService.GetById(user.Id);
                if (fullUser != null)
                {
                    if (fullUser.Preferences == null)
                    {
                        var idPreference = await _preferenceService.CreatePreferences(arguments, user.Id);
                        if (idPreference == null) return BadRequest();
                        return Ok(new { id = idPreference });
                    }
                    else
                    {
                        var idPreference = await _preferenceService.UpdatePreferences(arguments, user.Preferences.Id);
                        if (idPreference == null) return BadRequest();
                        return Ok(new { id = idPreference });
                    }

                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                // Needs loggin the error
                return Problem($"message=Unknown error while trying to save or update the preferences, error={ex.Message}");
            }
        }
    }
}
