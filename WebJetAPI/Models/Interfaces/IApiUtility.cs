namespace WebJetAPI.Models.Interfaces
{
    public interface IApiUtility
    {
        string BaseUrl { get; set; }
        string GetMovies { get; set; }
        string GetMovieDetail { get; set; }
    }
}
