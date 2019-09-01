using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using WebAPITest.Utilities;
using WebJetAPI.Models;
using WebJetAPI.Services;
using WebJetAPI.Services.ExternalService;

namespace WebAPITest.Services
{
    public class MovieHelperService : IMovieHelperService
    {
        public Movies ReadAllMoviesFromFile(Providers provider)
        {
            switch (provider)
            {
                case Providers.CinemaWorld:
                    {
                        var moviesReadFromFile = JsonConvert.DeserializeObject<Movies>(File.ReadAllText(TestAppData.CinemaWorldAllMoviesFilePath));
                        return moviesReadFromFile;
                    }
                case Providers.FilmWorld:
                    {
                        var moviesReadFromFile = JsonConvert.DeserializeObject<Movies>(File.ReadAllText(TestAppData.FilmWorldAllMoviesFilePath));
                        return moviesReadFromFile;
                    }
                case Providers.Combine:
                    {
                        var moviesReadFromFile = JsonConvert.DeserializeObject<Movies>(File.ReadAllText(TestAppData.AllMoviesFilePath));
                        return moviesReadFromFile;
                    }
            }

            return null;
        }

        public MovieDetail ReadMovieDetailFromFile(Providers provider)
        {
            switch (provider)
            {
                case Providers.CinemaWorld:
                    {
                        var moviesReadFromFile = JsonConvert.DeserializeObject<MovieDetail>(File.ReadAllText(TestAppData.CinemaMovieDetailFilePath));
                        return moviesReadFromFile;
                    }
                case Providers.FilmWorld:
                    {
                        var moviesReadFromFile = JsonConvert.DeserializeObject<MovieDetail>(File.ReadAllText(TestAppData.FilmMovieDetailFilePath));
                        return moviesReadFromFile;
                    }
                case Providers.Combine:
                    {
                        var moviesReadFromFile = JsonConvert.DeserializeObject<MovieDetail>(File.ReadAllText(TestAppData.CombineMoveieDetailFilePath));
                        return moviesReadFromFile;
                    }
            }

            return null;
        }

        public Mock<HttpMessageHandler> GetMockHttpMessageHandler(string respondContentFilePath, HttpStatusCode respondStatusCode)
        {
            var content = File.ReadAllText(respondContentFilePath);

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = respondStatusCode,
                    Content = new StringContent(content),
                })
                .Verifiable();

            return handlerMock;
        }

        public Mock<HttpMessageHandler> GetMockHttpMessageHandler(Exception respondeException)
        {

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .Throws(respondeException)
                .Verifiable();

            return handlerMock;
        }

        public Mock<IApiHelperService> GetMockApiHelperService(HttpMessageHandler handlerMock, string baseUrl)
        {
            var apiHelperService = new Mock<IApiHelperService>();
            apiHelperService.Setup(x => x.GetHttpClient(baseUrl)).Returns(new HttpClient(handlerMock)
            {
                BaseAddress = new Uri(baseUrl)
            }
            );

            return apiHelperService;
        }

        public IExternalMovieService GetMockExternalMovieServiceWithGetAllMoviesAsync(Movies cinemaMovies, Movies filmMovies)
        {
            var externalMovieService = new Mock<IExternalMovieService>();
            externalMovieService.Setup(x => x.GetAllMoviesAsync(TestAppData.CinemaBaseUrl, TestAppData.CinemaGetMoviesUrl)).ReturnsAsync(cinemaMovies);
            externalMovieService.Setup(x => x.GetAllMoviesAsync(TestAppData.FilmBaseUrl, TestAppData.FilmGetMoviesUrl)).ReturnsAsync(filmMovies);

            return externalMovieService.Object;
        }


        public IExternalMovieService GetMovieServiceWithMockGetMovieDetailAsync(MovieDetail cinemaMovie, MovieDetail filmMovie)
        {
            var externalMovieService = new Mock<IExternalMovieService>();
            externalMovieService.Setup(x => x.GetMovieDetailAsync(TestAppData.CinemaBaseUrl, TestAppData.CinemaGetMovieDetailUrl, TestAppData.CinemaTestMovieId)).ReturnsAsync(cinemaMovie);
            externalMovieService.Setup(x => x.GetMovieDetailAsync(TestAppData.FilmBaseUrl, TestAppData.FilmGetMovieDetailUrl, TestAppData.FilmTestMovieId)).ReturnsAsync(filmMovie);

            return externalMovieService.Object;
        }
    }
}
