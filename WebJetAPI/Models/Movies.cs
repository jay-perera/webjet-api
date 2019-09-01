using System.Collections.Generic;
using Newtonsoft.Json;

namespace WebJetAPI.Models
{
    public class Movies
    {
        [JsonProperty("Movies")]
        public List<Movie> MovieCollection { get; set; }

        public Movies()
        {
            MovieCollection = new List<Movie>();
        }
    }
}
