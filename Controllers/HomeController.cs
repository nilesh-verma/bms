using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BMS.Models;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;

namespace BMS.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        //string redisCacheName = ConfigurationManager.AppSettings["redisCacheName"];
        //string redisCachePassword = ConfigurationManager.AppSettings["redisCachePassword"];
        private readonly IDistributedCache _cache;
        private IMongoCollection<Movies> collection;
        private MongoDbSetting _mongoDbOptions { get; set; }
        public HomeController(IOptions<MongoDbSetting> mongoDbOptions, IDistributedCache cache)
        {
            _mongoDbOptions = mongoDbOptions.Value;
            _cache = cache;
            var client = new MongoClient(_mongoDbOptions.ConnectionString);
            IMongoDatabase db = client.GetDatabase(_mongoDbOptions.Database);
            this.collection = db.GetCollection<Movies>("Movies");
        }

        public async Task<IActionResult> Index()
        {
            var moviesFromCache = await _cache.GetSearchResultAsync("movies");
            var Movie = collection.Find(FilterDefinition<Movies>.Empty).ToList(); 
            
            if (moviesFromCache != null)
            {
                if (moviesFromCache.Count < Movie.Count && moviesFromCache.Count > Movie.Count)
                {
                    List<Movies> list = collection.Find(FilterDefinition<Movies>.Empty).ToList();

                    await _cache.AddSearchResultsAsync("movies", list, 12);
                    return View(await _cache.GetSearchResultAsync("movies"));

                }
                else {
                    return View(moviesFromCache); 
                }
            }
            else { 
            List<Movies> list = collection.Find(FilterDefinition<Movies>.Empty).ToList(); 
            
                await _cache.AddSearchResultsAsync("movies", list, 12);
                return View(await _cache.GetSearchResultAsync("movies"));
            }
           
        }


    }
}
//private readonly ILogger<HomeController> _logger;

//public HomeController(ILogger<HomeController> logger)
//{
//    _logger = logger;
//}

//public IActionResult Index()
//{
//    return View();
//}

//public IActionResult Privacy()
//{
//    return View();
//}

//[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//public IActionResult Error()
//{
//    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//}