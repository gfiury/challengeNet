using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using MovieAPI.Controllers;
using MovieModels.Interfaces;
using MovieModels.Models;
using MovieModels.Models.Arguments;
using System.Net;

namespace MovieTests
{
    [TestClass]
    public class MoviesControllerTest
    {
        private Mock<IUserService> _mockUserService;
        private Mock<IPreferenceService> _preferenceService;
        private IOptions<AppSettings> _appSettings;
        private MoviesController _moviesController;

        [TestInitialize]
        public void BeforeTest()
        {
            _mockUserService = new Mock<IUserService>();
            _preferenceService = new Mock<IPreferenceService>();
            _appSettings = Options.Create(new AppSettings() { TMDBApiUrl = "", SecretToken = "", TMDBReadToken = "" });
            _moviesController = new MoviesController(_appSettings, _mockUserService.Object, _preferenceService.Object);
            _moviesController.ControllerContext.HttpContext = new DefaultHttpContext();
        }

        [TestMethod]
        public async Task SavePreferencesUnauthenticated()
        {
            // Preferences
            var preferences = new PreferencesArguments { ReleaseYear = 2005 };

            // The user is not authenticated, returns Unauthorized
            var result = await _moviesController.SaveUpdatePreferences(preferences);
            var actionResult = result as StatusCodeResult;

            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.Unauthorized, actionResult?.StatusCode);
        }

        [TestMethod]
        public async Task SavePreferencesAuthenticated()
        {
            // Default User
            var defaultUser = new User { Id = 1, Email = "challenge@gmail.com", Name = "Challenge", LastName = "Codigo del Sur", Password = "challenge" };

            // Simulating Authentication
            _moviesController.HttpContext.Items.Add("User", defaultUser);

            _mockUserService
                .Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(defaultUser);

            _preferenceService.Setup(x => x.CreatePreferences(It.IsAny<PreferencesArguments>(), defaultUser.Id)).ReturnsAsync(1);

            // Preferences
            var preferences = new PreferencesArguments { ReleaseYear = 2005 };

            // Becasue the user doesn't have Preferences it will be saved
            var result = await _moviesController.SaveUpdatePreferences(preferences);
            var actionResult = result as ObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.OK, actionResult?.StatusCode);
        }
    }
}