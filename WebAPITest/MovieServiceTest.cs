
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebAPITest.Services;
using WebAPITest.Utilities;
using WebJetAPI.Models;
using WebJetAPI.Models.Interfaces;
using WebJetAPI.Services;

using WebJetAPI.Utility;


namespace WebAPITest
{
    [TestClass]
    public class MovieServiceTest
    {
        private static Mock<IApiUtilityService> _apiUtilityService;
        private static IMovieHelperService _movieHelperService;

        [ClassInitialize]
        public static void MovieServiceTestInit(TestContext testContext)
        {
            _movieHelperService = new MovieHelperService();

            var cinemaWorld = new Mock<IApiUtility>();
            cinemaWorld.Setup(x => x.BaseUrl).Returns(TestAppData.CinemaBaseUrl);
            cinemaWorld.Setup(x => x.GetMovieDetail).Returns(TestAppData.CinemaGetMovieDetailUrl);
            cinemaWorld.Setup(x => x.GetMovies).Returns(TestAppData.CinemaGetMoviesUrl);

            var filmWorld = new Mock<IApiUtility>();
            filmWorld.Setup(x => x.BaseUrl).Returns(TestAppData.FilmBaseUrl);
            filmWorld.Setup(x => x.GetMovieDetail).Returns(TestAppData.FilmGetMovieDetailUrl);
            filmWorld.Setup(x => x.GetMovies).Returns(TestAppData.FilmGetMoviesUrl);

            _apiUtilityService = new Mock<IApiUtilityService>();
            _apiUtilityService.Setup(x => x.FilmWorld).Returns(filmWorld.Object);
            _apiUtilityService.Setup(x => x.CinemaWorld).Returns(cinemaWorld.Object);
            _apiUtilityService.Setup(x => x.ApiKey).Returns(TestAppData.ApiKey);

        }





        // All Movies

        [TestMethod]
        public async Task GetAllMovies_MockServer_CorrectCount()
        {
            var result = await GetMovieService().GetMovieSummaryListAsync();
            Assert.AreEqual(7, result.Count);
        }

        [TestMethod]
        public async Task GetAllMovies_MockServer_CorrectData_WhenBothServersReturnData()
        {
            var moviesFromService = await GetMovieService().GetMovieSummaryListAsync();
            var moviesFromFile = _movieHelperService.ReadAllMoviesFromFile(Providers.Combine);

            foreach (var movie in moviesFromService)
            {
                foreach (var resultMove in moviesFromFile.MovieCollection.Where(resultMove => movie.ID == resultMove.ID))
                {
                    Assert.AreEqual(movie.Title, resultMove.Title);
                    Assert.AreEqual(movie.Type, resultMove.Type);
                    Assert.AreEqual(movie.Year, resultMove.Year);
                }
            }
        }

        [TestMethod]
        public async Task GetAllMovies_MockServer_CorrectData_WhenBothServersNotReturnData()
        {
            var externalMovieService = _movieHelperService.GetMockExternalMovieServiceWithGetAllMoviesAsync(null, null);
            var movieService = new MovieService(externalMovieService, _apiUtilityService.Object); ; //GetMockExternalMovieServiceWithGetAllMoviesAsync(null,null);
            var moviesFromService = await movieService.GetMovieSummaryListAsync();

            Assert.AreEqual(0, moviesFromService.Count);
        }

        [TestMethod]
        public async Task GetAllMovies_MockServer_CorrectData_WhenOnlyCinemaServerReturnData()
        {
            var externalMovieService = _movieHelperService.GetMockExternalMovieServiceWithGetAllMoviesAsync(_movieHelperService.ReadAllMoviesFromFile(Providers.CinemaWorld), null);
            var movieService = new MovieService(externalMovieService, _apiUtilityService.Object);
            var moviesFromService = await movieService.GetMovieSummaryListAsync();

            Assert.AreEqual(7, moviesFromService.Count);
        }

        [TestMethod]
        public async Task GetAllMovies_MockServer_CorrectData_WhenOnlyFilmServerReturnData()
        {
            var externalMovieService = _movieHelperService.GetMockExternalMovieServiceWithGetAllMoviesAsync(null,_movieHelperService.ReadAllMoviesFromFile(Providers.FilmWorld));
            var movieService = new MovieService(externalMovieService, _apiUtilityService.Object);
            var moviesFromService = await movieService.GetMovieSummaryListAsync();

            Assert.AreEqual(6, moviesFromService.Count);
        }

        // MovieDetails

        [TestMethod]
        public async Task GetMovieDetail_MockServer_CorrectData_WhenBothServersNotReturnData()
        {
            var externalMovieService = _movieHelperService.GetMovieServiceWithMockGetMovieDetailAsync(null, null);
            var movieService = new MovieService(externalMovieService, _apiUtilityService.Object);
            var moviesFromService = await movieService.GetMovieDetailAsync(TestAppData.TestMovieIdCommon);

            Assert.IsNull(moviesFromService);
        }

        [TestMethod]
        public async Task GetMovieDetail_MockServer_CorrectData_WhenBothServersReturnData()
        {
            var movieDetailFromFile = _movieHelperService.ReadMovieDetailFromFile(Providers.Combine);
            var externalMovieService = _movieHelperService.GetMovieServiceWithMockGetMovieDetailAsync(_movieHelperService.ReadMovieDetailFromFile(Providers.CinemaWorld), _movieHelperService.ReadMovieDetailFromFile(Providers.FilmWorld));
            var movieService = new MovieService(externalMovieService, _apiUtilityService.Object);
            var movieDetailFromServer = await movieService.GetMovieDetailAsync(TestAppData.TestMovieIdCommon);

            Assert.AreEqual(movieDetailFromFile.ID, movieDetailFromServer.ID);
            Assert.AreEqual(movieDetailFromFile.Actors, movieDetailFromServer.Actors);
            Assert.AreEqual(movieDetailFromFile.Awards, movieDetailFromServer.Awards);
            Assert.AreEqual(movieDetailFromFile.Country, movieDetailFromServer.Country);
            Assert.AreEqual(movieDetailFromFile.Director, movieDetailFromServer.Director);
            Assert.AreEqual(movieDetailFromFile.Genre, movieDetailFromServer.Genre);
            Assert.AreEqual(movieDetailFromFile.Language, movieDetailFromServer.Language);
            Assert.AreEqual(movieDetailFromFile.Metascore, movieDetailFromServer.Metascore);
            Assert.AreEqual(movieDetailFromFile.Plot, movieDetailFromServer.Plot);
            Assert.AreEqual(movieDetailFromFile.Poster, movieDetailFromServer.Poster);
            Assert.AreEqual(movieDetailFromFile.Votes, movieDetailFromServer.Votes);
            Assert.AreEqual(movieDetailFromFile.Rating, movieDetailFromServer.Rating);
            Assert.AreEqual(movieDetailFromFile.Type, movieDetailFromServer.Type);
            Assert.AreEqual(movieDetailFromFile.Writer, movieDetailFromServer.Writer);
            Assert.AreEqual(movieDetailFromFile.Rating, movieDetailFromServer.Rating);
            Assert.AreEqual(movieDetailFromFile.Title, movieDetailFromServer.Title);
            Assert.AreEqual(movieDetailFromFile.Rated, movieDetailFromServer.Rated);
            Assert.AreEqual(movieDetailFromFile.Released, movieDetailFromServer.Released);
            Assert.AreEqual(movieDetailFromFile.Runtime, movieDetailFromServer.Runtime);
            Assert.AreEqual(movieDetailFromFile.Price, movieDetailFromServer.Price);
        }

        [TestMethod]
        public async Task GetMovieDetail_MockServer_CorrectData_WhenOnlyFilmServerReturnData()
        {
            var movieDetailFromFile = _movieHelperService.ReadMovieDetailFromFile(Providers.FilmWorld);
            var externalMovieService = _movieHelperService.GetMovieServiceWithMockGetMovieDetailAsync(null, _movieHelperService.ReadMovieDetailFromFile(Providers.FilmWorld));
            var movieService = new MovieService(externalMovieService, _apiUtilityService.Object);
            var movieDetailFromServer = await movieService.GetMovieDetailAsync(TestAppData.TestMovieIdCommon);

            Assert.AreEqual(GetMovieDigitId(movieDetailFromFile.ID), movieDetailFromServer.ID);
            Assert.AreEqual(movieDetailFromFile.Actors, movieDetailFromServer.Actors);
            Assert.AreEqual(movieDetailFromFile.Awards, movieDetailFromServer.Awards);
            Assert.AreEqual(movieDetailFromFile.Country, movieDetailFromServer.Country);
            Assert.AreEqual(movieDetailFromFile.Director, movieDetailFromServer.Director);
            Assert.AreEqual(movieDetailFromFile.Genre, movieDetailFromServer.Genre);
            Assert.AreEqual(movieDetailFromFile.Language, movieDetailFromServer.Language);
            Assert.AreEqual(movieDetailFromFile.Metascore, movieDetailFromServer.Metascore);
            Assert.AreEqual(movieDetailFromFile.Plot, movieDetailFromServer.Plot);
            Assert.AreEqual(movieDetailFromFile.Poster, movieDetailFromServer.Poster);
            Assert.AreEqual(movieDetailFromFile.Votes, movieDetailFromServer.Votes);
            Assert.AreEqual(movieDetailFromFile.Rating, movieDetailFromServer.Rating);
            Assert.AreEqual(movieDetailFromFile.Type, movieDetailFromServer.Type);
            Assert.AreEqual(movieDetailFromFile.Writer, movieDetailFromServer.Writer);
            Assert.AreEqual(movieDetailFromFile.Rating, movieDetailFromServer.Rating);
            Assert.AreEqual(movieDetailFromFile.Title, movieDetailFromServer.Title);
            Assert.AreEqual(movieDetailFromFile.Rated, movieDetailFromServer.Rated);
            Assert.AreEqual(movieDetailFromFile.Released, movieDetailFromServer.Released);
            Assert.AreEqual(movieDetailFromFile.Runtime, movieDetailFromServer.Runtime);
            Assert.AreEqual(movieDetailFromFile.Price, movieDetailFromServer.Price);
        }


        [TestMethod]
        public async Task GetMovieDetail_MockServer_CorrectData_WhenOnlyCinemaServerReturnData()
        {
            var movieDetailFromFile = _movieHelperService.ReadMovieDetailFromFile(Providers.CinemaWorld);
            var externalMovieService = _movieHelperService.GetMovieServiceWithMockGetMovieDetailAsync(null, _movieHelperService.ReadMovieDetailFromFile(Providers.CinemaWorld));
            var movieService = new MovieService(externalMovieService, _apiUtilityService.Object);
            var movieDetailFromServer = await movieService.GetMovieDetailAsync(TestAppData.TestMovieIdCommon);

            Assert.AreEqual(GetMovieDigitId(movieDetailFromFile.ID), movieDetailFromServer.ID);
            Assert.AreEqual(movieDetailFromFile.Actors, movieDetailFromServer.Actors);
            Assert.AreEqual(movieDetailFromFile.Awards, movieDetailFromServer.Awards);
            Assert.AreEqual(movieDetailFromFile.Country, movieDetailFromServer.Country);
            Assert.AreEqual(movieDetailFromFile.Director, movieDetailFromServer.Director);
            Assert.AreEqual(movieDetailFromFile.Genre, movieDetailFromServer.Genre);
            Assert.AreEqual(movieDetailFromFile.Language, movieDetailFromServer.Language);
            Assert.AreEqual(movieDetailFromFile.Metascore, movieDetailFromServer.Metascore);
            Assert.AreEqual(movieDetailFromFile.Plot, movieDetailFromServer.Plot);
            Assert.AreEqual(movieDetailFromFile.Poster, movieDetailFromServer.Poster);
            Assert.AreEqual(movieDetailFromFile.Votes, movieDetailFromServer.Votes);
            Assert.AreEqual(movieDetailFromFile.Rating, movieDetailFromServer.Rating);
            Assert.AreEqual(movieDetailFromFile.Type, movieDetailFromServer.Type);
            Assert.AreEqual(movieDetailFromFile.Writer, movieDetailFromServer.Writer);
            Assert.AreEqual(movieDetailFromFile.Rating, movieDetailFromServer.Rating);
            Assert.AreEqual(movieDetailFromFile.Title, movieDetailFromServer.Title);
            Assert.AreEqual(movieDetailFromFile.Rated, movieDetailFromServer.Rated);
            Assert.AreEqual(movieDetailFromFile.Released, movieDetailFromServer.Released);
            Assert.AreEqual(movieDetailFromFile.Runtime, movieDetailFromServer.Runtime);
            Assert.AreEqual(movieDetailFromFile.Price, movieDetailFromServer.Price);
        }


        private string GetMovieDigitId(string id, int index = 2)
        {
            return id.Length > 2 ? id.Substring(2) : id;
        }

        private MovieService GetMovieService()
        {
            var externalMovieService = _movieHelperService.GetMockExternalMovieServiceWithGetAllMoviesAsync(_movieHelperService.ReadAllMoviesFromFile(Providers.CinemaWorld), _movieHelperService.ReadAllMoviesFromFile(Providers.FilmWorld));
            return new MovieService(externalMovieService, _apiUtilityService.Object);
        }

    }
}
