using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BMS.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private MongoDbSetting _mongoDbOptions { get; set; }
        private IMongoCollection<Movies> collection;
        private IMongoCollection<Theatre> theatre;
        private IMongoCollection<TheatreMovie> theatreMovie;
        private readonly IWebHostEnvironment _hostEnvironment;
        public AdminController(IOptions<MongoDbSetting> mongoDbOptions, IWebHostEnvironment hostEnvironment)
        {
            _mongoDbOptions = mongoDbOptions.Value;
            _hostEnvironment = hostEnvironment;
            var client = new MongoClient(_mongoDbOptions.ConnectionString);
            IMongoDatabase db = client.GetDatabase(_mongoDbOptions.Database);
            this.collection = db.GetCollection<Movies>("Movies");
            this.theatre = db.GetCollection<Theatre>("Theatre");
            this.theatreMovie = db.GetCollection<TheatreMovie>("TheatreMovie");

        }
        public IActionResult Index()
        {
        
            var movies = collection.Find(FilterDefinition<Movies>.Empty).ToList();           //Get Issuer Collection list
            
            return View(movies);
        }


        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(Movies movie)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string webRootPath = _hostEnvironment.WebRootPath;
                    var files = HttpContext.Request.Form.Files;
                    
                    if (files.Count > 0)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(webRootPath, @"movies");
                        var extenstion = Path.GetExtension(files[0].FileName);
                        
                        #region blob
                        string blobstorageconnection = "DefaultEndpointsProtocol=https;AccountName=bmsappstorage;AccountKey=o+npBZaJ2WVWjXdpDLiWnmO5I/H6yXJhptrVipbGz+TyhiAdvyL2c0yJbAYaXvy6ew2SyNmkq7/Rfcwoiu6K4w==;EndpointSuffix=core.windows.net";

                        byte[] dataFiles;
                        // Retrieve storage account from connection string.
                        CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobstorageconnection);
                        // Create the blob client.
                        CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                        // Retrieve a reference to a container.
                        CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("blobcontainer");

                        BlobContainerPermissions permissions = new BlobContainerPermissions
                        {
                            PublicAccess = BlobContainerPublicAccessType.Blob
                        };
                        string systemFileName = fileName + extenstion;
                        await cloudBlobContainer.SetPermissionsAsync(permissions);
                        await using (var target = new MemoryStream())
                        {
                            files[0].CopyTo(target);
                            dataFiles = target.ToArray();
                        }
                        // This also does not make a service call; it only creates a local object.
                        CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(systemFileName);
                        await cloudBlockBlob.UploadFromByteArrayAsync(dataFiles, 0, dataFiles.Length);


                        #endregion


                        string mimeType = files[0].ContentType;
                        byte[] fileData = new byte[files[0].Length];

                        //BlobStorageService objBlobService = new BlobStorageService();

                        //movie.imageUrl = objBlobService.UploadFileToBlob(fileName + extenstion, fileData, mimeType);
                        movie.imageUrl = cloudBlockBlob.Uri.AbsoluteUri;
                        //movie.imageUrl = @"\movies\" + fileName + extenstion;

                    }

                    Theatre t = theatre.Find(FilterDefinition<Theatre>.Empty).FirstOrDefault();           //Get Issuer Collection list


                    collection.InsertOne(movie);         //To post the issuer object   
                    TheatreMovie tm = new TheatreMovie()
                    {
                        MID=movie.MovieId,
                        TID=t.TheatreID,
                        seat=t.Seat
                       
                    };
                    theatreMovie.InsertOne(tm);
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "Please try again later.");
                    return View();
                }
                return RedirectToAction("Index");

            }

            return View();
        }

        public async Task<CloudBlockBlob> uploadBlob(string filename,Stream b )  
        {
            string systemFileName = filename;
            string blobstorageconnection = "DefaultEndpointsProtocol=https;AccountName=bmsmoviestorage;AccountKey=gzF8YiAOfobkTqP6gnkkRFr1RUZQnXoM1qWhGlKIks73gAaoE8Qu9vNbQkb0TS+e4Mi55zuDBY3s7utS9hcgEQ==;EndpointSuffix=core.windows.net";
            // Retrieve storage account from connection string.
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobstorageconnection);
            // Create the blob client.
            CloudBlobClient blobClient = cloudStorageAccount.CreateCloudBlobClient();
            // Retrieve a reference to a container.
            CloudBlobContainer container = blobClient.GetContainerReference("filescontainers");
            // This also does not make a service call; it only creates a local object.
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(systemFileName);
            await using (var data = b)
            {
                await blockBlob.UploadFromStreamAsync(data);
            }
            return   blockBlob;
        }

        public async Task<IActionResult> Delete(string id)
        {
            ObjectId oId = new ObjectId(id);
            Movies movie = collection.Find(e => e.MovieId
       == oId).FirstOrDefault();
            string blobstorageconnection = "DefaultEndpointsProtocol=https;AccountName=bmsappstorage;AccountKey=o+npBZaJ2WVWjXdpDLiWnmO5I/H6yXJhptrVipbGz+TyhiAdvyL2c0yJbAYaXvy6ew2SyNmkq7/Rfcwoiu6K4w==;EndpointSuffix=core.windows.net";
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobstorageconnection);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            string strContainerName = "blobcontainer";
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);
            string[] parts = movie.imageUrl.Split('/');
            string fileName = "";

            if (parts.Length > 0)
                fileName = parts[parts.Length - 1];
            else
                fileName = movie.imageUrl;

            var blob = cloudBlobContainer.GetBlobReference(fileName);
            await blob.DeleteIfExistsAsync();

            TheatreMovie theatreMov = theatreMovie.Find(e => e.MID
       == oId).FirstOrDefault();

            collection.DeleteOne<Movies>(e => e.MovieId == oId);
            theatreMovie.DeleteOne<TheatreMovie>(e => e.TheatreMovieID == theatreMov.TheatreMovieID);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(string id)
        {
            ObjectId oId = new ObjectId(id);

            Movies movie = collection.Find(e => e.MovieId
        == oId).FirstOrDefault();
            return View(movie);                                     //in order to render object which we wants to edit
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, Movies movies,string url)
        {
            movies.imageUrl = url;

            if (ModelState.IsValid)
            {

                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;

                ObjectId oId = new ObjectId(id);




                if (files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"movies");
                    var extenstion = Path.GetExtension(files[0].FileName);
                    string blobstorageconnection = "DefaultEndpointsProtocol=https;AccountName=bmsappstorage;AccountKey=o+npBZaJ2WVWjXdpDLiWnmO5I/H6yXJhptrVipbGz+TyhiAdvyL2c0yJbAYaXvy6ew2SyNmkq7/Rfcwoiu6K4w==;EndpointSuffix=core.windows.net";

                    byte[] dataFiles;
                    // Retrieve storage account from connection string.
                    CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobstorageconnection);
                    // Create the blob client.
                    CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                    // Retrieve a reference to a container.
                    CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("blobcontainer");

                    BlobContainerPermissions permissions = new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    };
                    string systemFileName = fileName + extenstion;
                    if (movies.imageUrl != null)
                    {
                        #region Blob delete first
                       
                        string strContainerName = "blobcontainer";
                       
                        string[] parts = movies.imageUrl.Split('/');
                        string fileNam = "";

                        if (parts.Length > 0)
                            fileNam = parts[parts.Length - 1];
                        else
                            fileNam = movies.imageUrl;

                        var blob = cloudBlobContainer.GetBlobReference(fileNam);
                        await blob.DeleteIfExistsAsync();

                        #endregion
                        await cloudBlobContainer.SetPermissionsAsync(permissions);
                        await using (var target = new MemoryStream())
                        {
                            files[0].CopyTo(target);
                            dataFiles = target.ToArray();
                        }
                        // This also does not make a service call; it only creates a local object.
                        CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(systemFileName);
                        await cloudBlockBlob.UploadFromByteArrayAsync(dataFiles, 0, dataFiles.Length);

                        movies.imageUrl = cloudBlockBlob.Uri.AbsoluteUri;
                    }
                    else
                    {
                        #region blob
                        
                        await cloudBlobContainer.SetPermissionsAsync(permissions);
                        await using (var target = new MemoryStream())
                        {
                            files[0].CopyTo(target);
                            dataFiles = target.ToArray();
                        }
                        // This also does not make a service call; it only creates a local object.
                        CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(systemFileName);
                        await cloudBlockBlob.UploadFromByteArrayAsync(dataFiles, 0, dataFiles.Length);

                        movies.imageUrl = cloudBlockBlob.Uri.AbsoluteUri;
                        #endregion

                    }
                  
                }
               
              



                var filter = Builders<Movies>.Filter.Eq("MovieId", oId);
                var updateDef = Builders<Movies>.Update.
            Set("MovieName", movies.MovieName);
                updateDef = updateDef.Set("Actor", movies.Actor);
                updateDef = updateDef.Set("length", movies.length);                //updating data in Issuer collection
                updateDef = updateDef.Set("imageUrl", movies.imageUrl);
                updateDef = updateDef.Set("Description", movies.Description);
                var result = collection.UpdateOne(filter, updateDef);

              
                return RedirectToAction("Index");






            }

            return View();

        }




    }
}
