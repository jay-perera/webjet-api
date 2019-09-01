using System.Threading.Tasks;
using WebJetAPI.Models;

namespace WebJetAPI.Services.ExternalService
{
    public interface IExternalMovieService
    {
        Task<Movies> GetAllMoviesAsync(string baseUrl , string resourceUrl);

        Task<MovieDetail> GetMovieDetailAsync(string baseUrl, string resourceUrl, string id);
    }
}
