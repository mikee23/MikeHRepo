using JwtAuthenticationManager.Services;
using MemberWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace MemberWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMongoCollection<Member> _memberCollection;

        public MemberController()
        {
            var dbHost = "host.docker.internal"; //this work for localhost only
            //var dbHost = "localhost"; //this work for localhost only
            //var dbName = "dms_member";
            var dbName = Environment.GetEnvironmentVariable("DB_NAME");
            var connectionString = $"mongodb://{dbHost}:27017/{dbName}";

            var mongoUrl = MongoUrl.Create(connectionString);
            var mongoClient = new MongoClient(mongoUrl);
            var database = mongoClient.GetDatabase(mongoUrl.DatabaseName);
            _memberCollection = database.GetCollection<Member>("member");
        }

        #region Member Web API Methods
        /// <summary>
        /// Get the list of members
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Member>>> GetMembers()
        {
            return await _memberCollection.Find(Builders<Member>.Filter.Empty).ToListAsync();
        }

        /// <summary>
        /// Get Member by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetById(string id)
        {
            var filterDefinition = Builders<Member>.Filter.Eq(x => x.MemberId, id);
            return await _memberCollection.Find(filterDefinition).SingleOrDefaultAsync();
        }

        /// <summary>
        /// Add a new Member
        /// </summary>
        /// <param name="Member"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Create(Member member)
        {
            await _memberCollection.InsertOneAsync(member);
            return Ok();
        }

        /// <summary>
        /// Delete Member by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var filterDefinition = Builders<Member>.Filter.Eq(x => x.MemberId, id);
            await _memberCollection.DeleteOneAsync(filterDefinition);
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Administrator,User")]
        public async Task<ActionResult> Update(Member member)
        {
            var filterDefinition = Builders<Member>.Filter.Eq(x => x.MemberId, member.MemberId);
            await _memberCollection.ReplaceOneAsync(filterDefinition, member);
            return Ok();
        }
        #endregion
    }
}
