using AlbumWebApi.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace AlbumWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        private readonly IMongoCollection<Album> _albumCollection;

        public AlbumController()
        {
            var dbHost = "host.docker.internal"; //this work for localhost only
            //var dbHost = "localhost"; //this work for localhost only
            //var dbName = "dms_album";
            var dbName = Environment.GetEnvironmentVariable("DB_NAME");
            var connectionString = $"mongodb://{dbHost}:27017/{dbName}";

            var mongoUrl = MongoUrl.Create(connectionString);
            var mongoClient = new MongoClient(mongoUrl);
            var database = mongoClient.GetDatabase(mongoUrl.DatabaseName);
            _albumCollection = database.GetCollection<Album>("photoalbum");
        }

        #region Album Web API Methods
        /// <summary>
        /// Get the list of albums
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Album>>> GetAlbumList()
        {
            return await _albumCollection.Find(Builders<Album>.Filter.Empty).ToListAsync();
        }

        /// <summary>
        /// Get Album by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Album>> GetById(string id)
        {
            var filterDefinition = Builders<Album>.Filter.Eq(x=> x.AlbumId, id);
            return await _albumCollection.Find(filterDefinition).SingleOrDefaultAsync();
        }

        /// <summary>
        /// Add a new Album
        /// </summary>
        /// <param name="Album"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Create(Album album)
        {
            await _albumCollection.InsertOneAsync(album);
            return Ok();
        }

        /// <summary>
        /// Delete Album by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var filterDefinition = Builders<Album>.Filter.Eq(x => x.AlbumId, id);
            await _albumCollection.DeleteOneAsync(filterDefinition);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> Update(Album album)
        {
            var filterDefinition = Builders<Album>.Filter.Eq(x => x.AlbumId, album.AlbumId);
            await _albumCollection.ReplaceOneAsync(filterDefinition, album);
            return Ok();
        }
        #endregion
    }
}
