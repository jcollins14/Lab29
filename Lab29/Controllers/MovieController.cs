using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.Extensions.Logging;

namespace Lab29.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly MovieDBContext Db;
        public MovieController(MovieDBContext context)
        {
            Db = context;
        }
        [HttpGet]
        public ActionResult<Movie> GetMovies()
        {
            List<Movie> resultList = Db.Movies.ToList();
            return Ok(resultList);
        }
        [HttpGet]
        public ActionResult<Movie> GetCategory(string cat)
        {
            List<Movie> resultList = new List<Movie>();
            var data = Db.Movies.Where(x => x.Genre1 == cat || x.Genre2 == cat).ToList();
            foreach (Movie movie in data)
            {
                resultList.Add(movie);
            }
            return Ok(resultList);
        }
        [HttpGet]
        public ActionResult<Movie> GetRandomMovie()
        {
            List<Movie> data = Db.Movies.ToList();
            Random rng = new Random();
            Movie result = data[rng.Next(0, data.Count)];
            return Ok(result);
        }
        [HttpGet]
        public ActionResult<Movie> GetRandomCategory(string cat)
        {
            List<Movie> data = Db.Movies.Where(x => x.Genre1 == cat || x.Genre2 == cat).ToList();
            Random rng = new Random();
            Movie result = data[rng.Next(0, data.Count)];
            return Ok(result);
        }
        [HttpGet]
        public ActionResult<Movie> GetRandomMultiple(int num)
        {
            List<Movie> data = Db.Movies.ToList();
            Random rng = new Random();
            List<Movie> resultList = new List<Movie>();
            if (num <= data.Count && num != 0)
            {
                for (int i = 0; i < num; i++)
                {
                    Movie result = data[rng.Next(0, data.Count)];
                    data.Remove(result);
                    resultList.Add(result);
                }
            return Ok(resultList);
            }
            else
            {
                return NoContent();
            }
        }
        [HttpGet]
        public ActionResult<string> GetAllCategories()
        {
            List<string> resultList = new List<string>();
            List<Movie> data = Db.Movies.ToList();
            foreach (Movie movie in data)
            {
                if (!resultList.Contains(movie.Genre1))
                {
                    resultList.Add(movie.Genre1);
                }
                if (!resultList.Contains(movie.Genre2))
                {
                    resultList.Add(movie.Genre2);
                }
            }
            resultList.Remove(null);
            if (resultList.Count==0)
            {
                return NoContent();
            }
            return Ok(resultList);
        }

        [HttpGet]
        public ActionResult<Movie> GetMovieInfo(string title)
        {
            Movie result = Db.Movies.Where(x => x.Title.Trim().ToLower() == title.Trim().ToLower()).FirstOrDefault();
            if (result is null)
            {
                return NoContent();
            }
            return Ok(result);
        }

        [HttpGet]
        public ActionResult<Movie> MovieSearch(string title)
        {
            List<Movie> data = Db.Movies.ToList();
            List<Movie> result = new List<Movie>();
            title = title.Trim().ToLower();
            foreach (Movie movie in data)
            {
                if (movie.Title.Trim().ToLower().Contains(title))
                {
                    result.Add(movie);
                }
            }
            if (result.Count > 0)
            {
            return Ok(result);
            }
            else
            {
                return NoContent();
            }
        }
    }
}