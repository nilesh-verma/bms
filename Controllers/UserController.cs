using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using BMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using RestSharp;

namespace BMS.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private IConfiguration _configuration;
        // Get the connection string from app settings
        string connectionString = "DefaultEndpointsProtocol=https;AccountName=bmsappstorage;AccountKey=o+npBZaJ2WVWjXdpDLiWnmO5I/H6yXJhptrVipbGz+TyhiAdvyL2c0yJbAYaXvy6ew2SyNmkq7/Rfcwoiu6K4w==;EndpointSuffix=core.windows.net";
        private IMongoCollection<TheatreMovie> theatreMovie;
        private IMongoCollection<UserDetails> user;
        private MongoDbSetting _mongoDbOptions { get; set; }
        public UserController(IConfiguration configuration, IOptions<MongoDbSetting> mongoDbOptions)
        {
            _mongoDbOptions = mongoDbOptions.Value;
            var client = new MongoClient(_mongoDbOptions.ConnectionString);
            IMongoDatabase db = client.GetDatabase(_mongoDbOptions.Database);
            _configuration = configuration;
            this.theatreMovie = db.GetCollection<TheatreMovie>("TheatreMovie");
            this.user = db.GetCollection<UserDetails>("UserDetails");
        }
        public async Task<IActionResult> Index()
        {
            
            
            List<Theatres> theatre = new List<Theatres>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://bmsapi.azurewebsites.net/api/Theatre/getTheatre"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    theatre = JsonConvert.DeserializeObject<List<Theatres>>(apiResponse);
                }
            }
            return View(theatre);
        }

        public async Task<IActionResult> Movies()
        {
            List<MovieDetails> movies = new List<MovieDetails>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://bmsapi.azurewebsites.net/api/Theatre/movie"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    movies = JsonConvert.DeserializeObject<List<MovieDetails>>(apiResponse);
                }
            }
            return View(movies);

        }

        public async   Task< IActionResult> Book(string id)
        {
            MovieDetails movies = new MovieDetails();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://bmsapi.azurewebsites.net/api/Theatre/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    movies = JsonConvert.DeserializeObject<MovieDetails>(apiResponse);
                }
            }

            MovieUser m = new MovieUser()
            {
                movieDetails=movies
            };
            return View(m);
        }
        [HttpPost]
        public async Task<IActionResult> Book(MovieUser u,string id)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            u.userDetails.uid = userId;


            var tmov = theatreMovie.Find(FilterDefinition<TheatreMovie>.Empty).ToList();
            TheatreMovie tm = tmov.Where(p => p.TheatreMovieID == new MongoDB.Bson.ObjectId(id)).FirstOrDefault();
            if (u.userDetails.Seat <= tm.seat)
            {
                user.InsertOne(u.userDetails);
                tm.seat = tm.seat - u.userDetails.Seat;
                var filter = Builders<TheatreMovie>.Filter.Eq("TheatreMovieID", new ObjectId(id));
                var updateDef = Builders<TheatreMovie>.Update.Set("seat", tm.seat);
                theatreMovie.UpdateOne(filter, updateDef);
                if (CreateQueue(userId))
                {



                    // Instantiate a QueueClient which will be used to create and manipulate the queue
                    QueueClient queueClient = new QueueClient(connectionString, userId);

                    // Create the queue if it doesn't already exist
                    queueClient.CreateIfNotExists();

                    if (queueClient.Exists())
                    {
                        // Send a message to the queue
                        queueClient.SendMessage("success");
                        PeekedMessage[] peekedMessage = queueClient.PeekMessages();
                        if (peekedMessage.Length == 1)
                        {
                            string mseFromQ = peekedMessage[0].MessageText;
                            if (mseFromQ == "success")
                            {
                                Random generator = new Random();
                                string msg = "Hey  <b>" + u.userDetails.name +"</b>," + Environment.NewLine + "<br/>" + "Your booking is confirmed now of your " + u.userDetails.Seat + " seats.<br/>" + Environment.NewLine + "Your booking Id is " + generator.Next(0, 999999).ToString("D6") + "<br/>" + Environment.NewLine + "Thanks,<br/>" + Environment.NewLine + "<b>BMS</b>";
                                await callSecondService(u.userDetails,msg);
                                queueClient.Delete();

                            }
                        }
                        else if (peekedMessage.Length > 1)
                        {

                        }
                        else
                        {
                            string msg = "Hey  <b>" + u.userDetails.name + "</b>," + Environment.NewLine + "<br/>" + "sorry for inconvenience." + " <br/>" + "Thanks,<br/>" + Environment.NewLine + "<b>BMS</b>";
                            await callSecondService(u.userDetails, msg);
                        }

                    }

                }
                return RedirectToAction("index");
            }

            else
            {
                Random generator = new Random();
                string msg = "Hey  <b>" + u.userDetails.name + "</b>," + Environment.NewLine + "<br/>" + "sorry for inconvenience." + " <br/>"  + "Thanks,<br/>" + Environment.NewLine + "<b>BMS</b>";
                await callSecondService(u.userDetails, msg);
                return RedirectToAction("index");
            }

           
            return RedirectToAction("index");  
        }

        public async Task callSecondService(UserDetails u,string m)
        {

            var client = new HttpClient();
            
            // requires using System.Text.Json;
            var jsonData = System.Text.Json.JsonSerializer.Serialize(new
            {
                email = u.email,
                due = m,
                task = "Booking Conformation"
            });

            HttpResponseMessage result = await client.PostAsync(
                // Requires DI configuration to access app settings. See https://docs.microsoft.com/azure/app-service/configure-language-dotnetcore#access-environment-variables
                "https://prod-25.centralindia.logic.azure.com:443/workflows/2b5c925f207a41c7adc1420d79b66249/triggers/manual/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=DVYniRXf3kWNQbEzYxqrHRTBTHy_up2S0gQbvF3SbvY",
                new StringContent(jsonData, Encoding.UTF8, "application/json"));

            var statusCode = result.StatusCode.ToString();
        }

        public bool CreateQueue(string queueName)
        {
            try
            {
               
                // Instantiate a QueueClient which will be used to create and manipulate the queue
                QueueClient queueClient = new QueueClient(connectionString, queueName);

                // Create the queue
                queueClient.CreateIfNotExists();

                if (queueClient.Exists())
                {
                   
                    return true;
                }
                else
                {
                   
                    return false;
                }
            }
            catch (Exception ex)
            {
               
                return false;
            }
        }
    }
}
