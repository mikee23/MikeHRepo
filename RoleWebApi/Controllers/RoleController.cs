using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoleWebApi.Dto;
using RoleWebApi.Models;

namespace RoleWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleDbContext _roleDbContext;
        private readonly IMapper _mapper;

        public RoleController(RoleDbContext dbContext, IMapper mapper)

        {
            _mapper = mapper;
            _roleDbContext = dbContext;
        }

        #region Role Web API Methods
        /// <summary>
        /// Get the list of Roles
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<Role>> GetRoleList()
        {
            var listOfRoles = _roleDbContext.Roles;

            if (listOfRoles == null)
            {
                return new List<Role>();
            }

            return listOfRoles;
        }

        /// <summary>
        /// Get Role by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> Get(int id)
        {
            var obj = await _roleDbContext.Roles.FindAsync(id);
            if (obj != null)
            {
                return obj;
            }
            return new Role();
        }

        /// <summary>
        /// Add a new Role
        /// </summary>
        /// <param name="Role"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(RoleAddDTO objDTO)
        {
            var obj = _mapper.Map<RoleAddDTO, Role>(objDTO); //Convert Role DTO to Role object
            bool isRoleCreated = false;

            var listOfRoles = _roleDbContext.Roles;
            if (listOfRoles == null)
            {
                return BadRequest();
            }

            //Determine if role already exist
            var existingRole = listOfRoles.Where(r => r.Description == objDTO.Description).FirstOrDefault();

            if (existingRole == null)
            {
                await _roleDbContext.Roles.AddAsync(obj);
                await _roleDbContext.SaveChangesAsync();
                isRoleCreated = true;
            }

            if (isRoleCreated)
            {
                return Ok(isRoleCreated);
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Delete Role by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var role = await _roleDbContext.Roles.FindAsync(id);
            _roleDbContext.Roles.Remove(role);
            await _roleDbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> Update(Role objDTO)
        {
            _roleDbContext.Roles.Update(objDTO);
            await _roleDbContext.SaveChangesAsync();
            return Ok();
        }
        #endregion
    }
}
