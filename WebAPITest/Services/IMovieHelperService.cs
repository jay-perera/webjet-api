using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Moq;
using WebJetAPI.Models;
using WebJetAPI.Services;
using WebJetAPI.Services.ExternalService;

namespace WebAPITest.Services
{
    public interface IMovieHelperService
    {
        Movies ReadAllMoviesFromFile(Providers provider);
        MovieDetail ReadMovieDetailFromFile(Providers provider);
        Mock<HttpMessageHandler> GetMockHttpMessageHandler(string respondContentFilePath, HttpStatusCode respondStatusCode);
        Mock<HttpMessageHandler> GetMockHttpMessageHandler(Exception exception);
        Mock<IApiHelperService> GetMockApiHelperService(HttpMessageHandler handlerMock, string baseUrl);
        IExternalMovieService GetMockExternalMovieServiceWithGetAllMoviesAsync(Movies cinemaMovie, Movies filmMovies);
        IExternalMovieService GetMovieServiceWithMockGetMovieDetailAsync(MovieDetail cinemaMovie, MovieDetail filmMovie);
    }
}
