using System.Collections.Generic;
using System.Threading.Tasks;
using WebJetAPI.Models;

namespace WebJetAPI.Services
{
    public interface IMovieService
    {
        Task<List<Movie>> GetMovieSummaryListAsync();
        Task<MovieDetail> GetMovieDetailAsync(string id);
    }
}
