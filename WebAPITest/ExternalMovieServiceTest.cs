using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebAPITest.Services;
using WebAPITest.Utilities;
using WebJetAPI.Models;
using WebJetAPI.Services;
using WebJetAPI.Services.ExternalService;
using WebJetAPI.Utility;

namespace WebAPITest
{
    [TestClass]
    public class ExternalMovieServiceTest
    {
        private static string _baseUrlCinema;

        private static string _getMovieCinema;
        private static string _getMovieDetailCinema;
        private static string _movieId;

        private static Mock<IApiUtilityService> _apiUtilityService;
        private static IMovieHelperService _movieHelperService;


        [ClassInitialize]
        public static void ExternalMovieServiceTestInit(TestContext testContext)
        {
            _movieHelperService = new MovieHelperService();

            _baseUrlCinema = TestAppData.CinemaBaseUrl;
            _getMovieCinema = TestAppData.CinemaGetMoviesUrl;
            _getMovieDetailCinema = TestAppData.CinemaGetMovieDetailUrl;
            _movieId = TestAppData.CinemaTestMovieId;

            _apiUtilityService = new Mock<IApiUtilityService>();
            _apiUtilityService.Setup(x => x.ApiKey).Returns(TestAppData.ApiKey);
        }


        // All Movies with Mock Server

        [TestMethod]
        public async Task GetAllMovies_MockServer_NotNull()
        {
            var httpMessageHandler = _movieHelperService.GetMockHttpMessageHandler(TestAppData.CinemaWorldAllMoviesFilePath, HttpStatusCode.OK).Object;
            var apiHelper = _movieHelperService.GetMockApiHelperService(httpMessageHandler, _baseUrlCinema).Object;
            var externalService = new ExternalMovieService(_apiUtilityService.Object, apiHelper);
            var result = await externalService.GetAllMoviesAsync(_baseUrlCinema, _getMovieCinema);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetAllMovies_MockServer_CorrectCount()
        {
            var httpMessageHandler = _movieHelperService.GetMockHttpMessageHandler(TestAppData.CinemaWorldAllMoviesFilePath, HttpStatusCode.OK).Object;
            var apiHelper = _movieHelperService.GetMockApiHelperService(httpMessageHandler, _baseUrlCinema).Object;
            var externalService = new ExternalMovieService(_apiUtilityService.Object, apiHelper);
            var result = await externalService.GetAllMoviesAsync(_baseUrlCinema, _getMovieCinema);

            Assert.AreEqual(7, result.MovieCollection.Count);
        }

        [TestMethod]
        public async Task GetAllMovies_MockServer_CorrectData()
        {
            var httpMessageHandler = _movieHelperService.GetMockHttpMessageHandler(TestAppData.CinemaWorldAllMoviesFilePath, HttpStatusCode.OK).Object;
            var apiHelper = _movieHelperService.GetMockApiHelperService(httpMessageHandler, _baseUrlCinema).Object;
            var moviesReadFromFile = _movieHelperService.ReadAllMoviesFromFile(Providers.CinemaWorld);

            var moviesFromExternalSystem = new ExternalMovieService(_apiUtilityService.Object, apiHelper);
            var moviesFromApiCall = await moviesFromExternalSystem.GetAllMoviesAsync(_baseUrlCinema, _getMovieCinema);

            foreach (var movie in moviesReadFromFile.MovieCollection)
            {
                foreach (var resultMove in moviesFromApiCall.MovieCollection.Where(resultMove => movie.ID == resultMove.ID))
                {
                    Assert.AreEqual(movie.Poster, resultMove.Poster);
                    Assert.AreEqual(movie.Title, resultMove.Title);
                    Assert.AreEqual(movie.Type, resultMove.Type);
                    Assert.AreEqual(movie.Year, resultMove.Year);
                }
            }
        }

        [TestMethod]
        public async Task GetAllMovies_MockServer_HandleErrorStatus()
        {
            var httpMessageHandler = _movieHelperService.GetMockHttpMessageHandler(TestAppData.CinemaWorldAllMoviesFilePath, HttpStatusCode.BadRequest).Object;
            var apiHelper = _movieHelperService.GetMockApiHelperService(httpMessageHandler, _baseUrlCinema).Object;
            var externalService = new ExternalMovieService(_apiUtilityService.Object, apiHelper);
            var result = await externalService.GetAllMoviesAsync(_baseUrlCinema, _getMovieCinema);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetAllMovie_MockServer_ExceptionHandle()
        {
            var httpMessageHandler = _movieHelperService.GetMockHttpMessageHandler(new HttpRequestException()).Object;
            var apiHelper = _movieHelperService.GetMockApiHelperService(httpMessageHandler, _baseUrlCinema).Object;
            var externalService = new ExternalMovieService(_apiUtilityService.Object, apiHelper);
            var result = await externalService.GetAllMoviesAsync(_baseUrlCinema, _getMovieDetailCinema);

            Assert.IsNull(result);
        }


        // movie detail with Mock Server


        [TestMethod]
        public async Task GetMovieDetail_MockServer_NotNull()
        {
            var httpMessageHandler = _movieHelperService.GetMockHttpMessageHandler(TestAppData.CinemaMovieDetailFilePath, HttpStatusCode.OK).Object;
            var apiHelper = _movieHelperService.GetMockApiHelperService(httpMessageHandler, _baseUrlCinema).Object;

            var externalService = new ExternalMovieService(_apiUtilityService.Object, apiHelper);
            var result = await externalService.GetMovieDetailAsync(_baseUrlCinema, _getMovieDetailCinema, _movieId);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetMovieDetail_MockServer_CorrectData()
        {
            var httpMessageHandler = _movieHelperService.GetMockHttpMessageHandler(TestAppData.CinemaMovieDetailFilePath, HttpStatusCode.OK).Object;
            var apiHelper = _movieHelperService.GetMockApiHelperService(httpMessageHandler, _baseUrlCinema).Object;
            var moviesReadFromFile = JsonConvert.DeserializeObject<MovieDetail>(File.ReadAllText(TestAppData.CinemaMovieDetailFilePath));

            var externalService = new ExternalMovieService(_apiUtilityService.Object, apiHelper);
            var result = await externalService.GetMovieDetailAsync(_baseUrlCinema, _getMovieDetailCinema, _movieId);

            Assert.AreEqual(moviesReadFromFile.ID, result.ID);
            Assert.AreEqual(moviesReadFromFile.Actors, result.Actors);
            Assert.AreEqual(moviesReadFromFile.Awards, result.Awards);
            Assert.AreEqual(moviesReadFromFile.Country, result.Country);
            Assert.AreEqual(moviesReadFromFile.Director, result.Director);
            Assert.AreEqual(moviesReadFromFile.Genre, result.Genre);
            Assert.AreEqual(moviesReadFromFile.Language, result.Language);
            Assert.AreEqual(moviesReadFromFile.Metascore, result.Metascore);
            Assert.AreEqual(moviesReadFromFile.Plot, result.Plot);
            Assert.AreEqual(moviesReadFromFile.Poster, result.Poster);
            Assert.AreEqual(moviesReadFromFile.Price, result.Price);
            Assert.AreEqual(moviesReadFromFile.Votes, result.Votes);
            Assert.AreEqual(moviesReadFromFile.Rating, result.Rating);
            Assert.AreEqual(moviesReadFromFile.Type, result.Type);
            Assert.AreEqual(moviesReadFromFile.Writer, result.Writer);
            Assert.AreEqual(moviesReadFromFile.Rating, result.Rating);
            Assert.AreEqual(moviesReadFromFile.Title, result.Title);
            Assert.AreEqual(moviesReadFromFile.Rated, result.Rated);
            Assert.AreEqual(moviesReadFromFile.Released, result.Released);
            Assert.AreEqual(moviesReadFromFile.Runtime, result.Runtime);

        }

        [TestMethod]
        public async Task GetMovieDetail_MockServer_HandleErrorStatus()
        {
            var httpMessageHandler = _movieHelperService.GetMockHttpMessageHandler(TestAppData.CinemaMovieDetailFilePath, HttpStatusCode.BadRequest).Object;
            var apiHelper = _movieHelperService.GetMockApiHelperService(httpMessageHandler, _baseUrlCinema).Object;
            var externalService = new ExternalMovieService(_apiUtilityService.Object, apiHelper);
            var result = await externalService.GetMovieDetailAsync(_baseUrlCinema, _getMovieDetailCinema, _movieId);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetMovieDetail_MockServer_ExceptionHandle()
        {
            var httpMessageHandler = _movieHelperService.GetMockHttpMessageHandler(new HttpRequestException()).Object;
            var apiHelper = _movieHelperService.GetMockApiHelperService(httpMessageHandler, _baseUrlCinema).Object;
            var externalService = new ExternalMovieService(_apiUtilityService.Object, apiHelper);
            var result = await externalService.GetMovieDetailAsync(_baseUrlCinema, _getMovieDetailCinema, _movieId);

            Assert.IsNull(result);
        }


    }
}
