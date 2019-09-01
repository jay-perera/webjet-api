using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebJetAPI.Models;
using WebJetAPI.Services.ExternalService;
using WebJetAPI.Utility;

namespace WebJetAPI.Services
{
    public class MovieService : IMovieService
    {
        private readonly IExternalMovieService _externalMovieService;
        private readonly IApiUtilityService _apiUtility;

        public MovieService(IExternalMovieService externalMovieService, IApiUtilityService apiUtility)
        {
            _externalMovieService = externalMovieService;
            _apiUtility = apiUtility;
        }

        public async Task<List<Movie>> GetMovieSummaryListAsync()
        {
            var cinemaWorldMovies = _externalMovieService.GetAllMoviesAsync(_apiUtility.CinemaWorld.BaseUrl, _apiUtility.CinemaWorld.GetMovies);
            var filmWorldMovies = _externalMovieService.GetAllMoviesAsync(_apiUtility.FilmWorld.BaseUrl, _apiUtility.FilmWorld.GetMovies);

            await Task.WhenAll(cinemaWorldMovies, filmWorldMovies);

            var moviesList = new List<Movies>();
            moviesList.Add(cinemaWorldMovies.Result);
            moviesList.Add(filmWorldMovies.Result);

            return GetUniqueMovieList(moviesList);
        }

        public async Task<MovieDetail> GetMovieDetailAsync(string id)
        {
            var filmWorldMovie = _externalMovieService.GetMovieDetailAsync(_apiUtility.FilmWorld.BaseUrl, _apiUtility.FilmWorld.GetMovieDetail, String.Concat("fw", id));
            var cinemaWorldMovie = _externalMovieService.GetMovieDetailAsync(_apiUtility.CinemaWorld.BaseUrl, _apiUtility.CinemaWorld.GetMovieDetail, String.Concat("cw", id));

            await Task.WhenAll(cinemaWorldMovie, filmWorldMovie);

            return BuildMovieDetailModel(cinemaWorldMovie.Result, filmWorldMovie.Result);

        }

        private List<Movie> GetUniqueMovieList(List<Movies> moviesList)
        {
            var uniqueMoveDictionary = new Dictionary<string, Movie>();

            foreach (var movies in moviesList)
            {
                if (movies != null)
                {
                    foreach (var movie in movies.MovieCollection)
                    {
                        if (!uniqueMoveDictionary.Keys.Contains(GetMovieDigitId(movie.ID)))
                        {
                            movie.ID = GetMovieDigitId(movie.ID);
                            uniqueMoveDictionary.Add(movie.ID, movie);
                        }
                    }
                }
            }

            return uniqueMoveDictionary.Values.ToList();
        }


        private string GetMovieDigitId(string id, int index = 2)
        {
            return id.Length > 2 ? id.Substring(2) : id;
        }


        private MovieDetail BuildMovieDetailModel(MovieDetail cinemaWorldMovie, MovieDetail filmWorldMovie)
        {
            MovieDetail moveDetail = null;

            if (cinemaWorldMovie != null)
            {
                moveDetail = cinemaWorldMovie;
            }
            else if (filmWorldMovie != null)
            {
                moveDetail = filmWorldMovie;
            }

            if (moveDetail != null && cinemaWorldMovie != null && filmWorldMovie != null)
            {
                moveDetail.Price = CalculateCheapPrice(cinemaWorldMovie, filmWorldMovie).ToString();
            }

            if (moveDetail != null)
            {
                moveDetail.ID = GetMovieDigitId(moveDetail.ID);
            }

            return moveDetail;
        }

        private decimal CalculateCheapPrice(MovieDetail cinemaWorldMovie, MovieDetail filmWoldMovie)
        {
            var cinemaPrice = decimal.Parse(cinemaWorldMovie.Price);
            var filmWorldPrice = decimal.Parse(filmWoldMovie.Price);
            return cinemaPrice > filmWorldPrice ? filmWorldPrice : cinemaPrice;
        }
    }
}
