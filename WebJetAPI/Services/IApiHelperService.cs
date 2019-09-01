using System.Net.Http;


namespace WebJetAPI.Services
{
    public interface IApiHelperService
    {
        HttpClient GetHttpClient(string baseAddress);
    }
}
