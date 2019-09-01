using WebJetAPI.Models.Interfaces;

namespace WebJetAPI.Models
{
    public class ApiUtility : IApiUtility
    {
        public string BaseUrl { get; set; }
        public string GetMovies { get; set; }
        public string GetMovieDetail { get; set; }

    }
}
