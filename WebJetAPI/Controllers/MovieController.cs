using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebJetAPI.Models;
using WebJetAPI.Services;

namespace WebJetAPI.Controllers
{
    [Route("api/movie")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }
        // GET: api/Movie
        [HttpGet]
        public async Task<IEnumerable<Movie>> Get()
        {
            return await _movieService.GetMovieSummaryListAsync();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<MovieDetail> Get(string id)
        {
            return await _movieService.GetMovieDetailAsync(id);
        }

        // POST: api/Movie
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Movie/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
