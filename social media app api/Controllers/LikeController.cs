using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using social_media_app_api;
using social_media_app_api.Database;
using social_media_app_api.DTO;
using social_media_app_api.Services;

namespace social_media_app_api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly ILikeService _likeService;

        public LikeController(ILikeService likeService)
        {
            _likeService = likeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Like>>> GetLikes()
        {
            var likes = await _likeService.GetAllLikesAsync();
            return Ok(likes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Like>> GetLike(int id)
        {
            var like = await _likeService.GetLikeByIdAsync(id);
            if (like == null)
                return NotFound();

            return like;
        }

        [HttpPost]
        public async Task<ActionResult<LikeDto>> PostLike(CreateLikeDto likeDto)
        {
            var (success, errorMessage, createdLike) = await _likeService.CreateLikeAsync(likeDto);

            if (!success)
                return BadRequest(errorMessage);

            // Om du använder AutoMapper eller liknande, kan du mappa så här:
            var result = new LikeDto
            {
                LikeId = createdLike!.LikeId,
                PublicationTime = createdLike.PublicationTime,
                LikedById = createdLike.LikedById,
                PostId = createdLike.PostId,
                CommentId = createdLike.CommentId
            };

            return CreatedAtAction(nameof(GetLike), new { id = result.LikeId }, result);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLike(int id)
        {
            var success = await _likeService.DeleteLikeAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}

