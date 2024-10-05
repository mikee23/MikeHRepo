using PhotoWebApi.Dto;
using PhotoWebApi.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace PhotoWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PhotoController : ControllerBase
    {
        private readonly PhotoDbContext _photoDbContext;
        private readonly IMapper _mapper;

        public PhotoController(PhotoDbContext dbContext, IMapper mapper)
        {
            _photoDbContext = dbContext;
            _mapper = mapper;
        }

        #region Photo Web API Methods
        /// <summary>
        /// Get the list of Photos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<Photo>> GetPhotos()
        {
            return _photoDbContext.Photos;
        }

        /// <summary>
        /// Get the list of Photo URLs
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetPhotoUrls")]
        public ActionResult<List<PhotoUrlDTO>> GetPhotoUrls()
        {
            var listOfPhotos = _photoDbContext.Photos;
            if (listOfPhotos == null)
            {
                return new List<PhotoUrlDTO>();
            }

            var photoUrls = listOfPhotos
                .Where(p => !p.IsDeleted)
                .OrderBy(p => p.DisplayOrder)
                .Select(p => new PhotoUrlDTO { PhotoId = p.PhotoId, ImageURL = p.ImageURL })
                .Distinct()
                .ToList();

            return photoUrls;
        }

        /// <summary>
        /// Get Photo by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Photo>> GetById(int id)
        {
            var photo = await _photoDbContext.Photos.FindAsync(id);
            return photo;
        }

        /// <summary>
        /// Add a new Photo
        /// </summary>
        /// <param name="Photo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(PhotoAddDTO objDTO)
        {
            var obj = _mapper.Map<PhotoAddDTO, Photo>(objDTO); //Convert Photo DTO to Photo object
            bool isPhotoCreated = false;

            var listOfPhotos = _photoDbContext.Photos;
            if (listOfPhotos == null)
            {
                return BadRequest();
            }

            //Determine if photo already exist
            var existingPhoto = listOfPhotos.Where(p => p.Title == objDTO.Title && p.ImageURL == objDTO.ImageURL).FirstOrDefault();

            if (existingPhoto == null)
            {
                await _photoDbContext.Photos.AddAsync(obj);
                await _photoDbContext.SaveChangesAsync();
                isPhotoCreated = true;
            }

            if (isPhotoCreated)
            {
                return Ok(isPhotoCreated);
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Delete Photo by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var photo = await _photoDbContext.Photos.FindAsync(id);
            _photoDbContext.Photos.Remove(photo);
            await _photoDbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> Update(Photo objDTO)
        {
            _photoDbContext.Photos.Update(objDTO);
            await _photoDbContext.SaveChangesAsync();
            return Ok();
        }
        #endregion
    }
}
