using System;

using System.Threading.Tasks;
using Newtonsoft.Json;

using WebJetAPI.Models;
using WebJetAPI.Utility;

namespace WebJetAPI.Services.ExternalService
{
    public class ExternalMovieService : IExternalMovieService
    {

        private readonly IApiHelperService _apiHelpService;

        public ExternalMovieService(IApiUtilityService apiUtilityService, IApiHelperService apiHelpService)
        {
            _apiHelpService = apiHelpService;
        }

        public async Task<Movies> GetAllMoviesAsync(string baseUrl, string resourceUrl)
        {
            try
            {
                var client = _apiHelpService.GetHttpClient(baseUrl);

                if (client != null)
                {
                    var response = await client.GetAsync(resourceUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<Movies>(result);
                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<MovieDetail> GetMovieDetailAsync(string baseUrl, string resourceUrl, string id)
        {
            try
            {
                var client = _apiHelpService.GetHttpClient(baseUrl);
                var response = await client.GetAsync(resourceUrl + '/' + id);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<MovieDetail>(result);
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
