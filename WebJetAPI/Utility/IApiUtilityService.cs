using System.Net.Http;
using WebJetAPI.Models.Interfaces;

namespace WebJetAPI.Utility
{
    public interface IApiUtilityService
    {
         IApiUtility CinemaWorld { get; }
         IApiUtility FilmWorld { get; }
         string ApiKey { get; }
    }
}
