using System;
using System.Net.Http;
using System.Net.Http.Headers;
using WebJetAPI.Utility;

namespace WebJetAPI.Services
{
    public class ApiHelperService : IApiHelperService
    {
        private readonly IApiUtilityService _apiUtilityService;

        public ApiHelperService(IApiUtilityService apiUtilityService)
        {
            _apiUtilityService = apiUtilityService;
        }

        public HttpClient GetHttpClient(string baseAddress)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("x-access-token", _apiUtilityService.ApiKey);
            httpClient.Timeout = TimeSpan.FromSeconds(10); // override time out
            return httpClient;
        }

    }
}
