using Microsoft.Extensions.Configuration;
using WebJetAPI.Models;
using WebJetAPI.Models.Interfaces;

namespace WebJetAPI.Utility
{
    public class ApiUtilityService : IApiUtilityService
    {
        public IApiUtility CinemaWorld { get; }
        public IApiUtility FilmWorld { get; }
        public string ApiKey { get; }


        public ApiUtilityService(IConfiguration configuration)
        {
            CinemaWorld = new ApiUtility();
            FilmWorld = new ApiUtility();

            CinemaWorld.BaseUrl = configuration.GetSection("Webjet:CinemaWorld:BaseUrl").Value;
            CinemaWorld.GetMovieDetail = configuration.GetSection("Webjet:CinemaWorld:GetMovieDetail").Value;
            CinemaWorld.GetMovies = configuration.GetSection("Webjet:CinemaWorld:GetMovies").Value;


            FilmWorld.BaseUrl = configuration.GetSection("Webjet:FilmWorld:BaseUrl").Value;
            FilmWorld.GetMovieDetail = configuration.GetSection("Webjet:FilmWorld:GetMovieDetail").Value;
            FilmWorld.GetMovies = configuration.GetSection("Webjet:FilmWorld:GetMovies").Value;

            ApiKey = configuration.GetSection("Webjet:ApiKey").Value;
        }
    }
}