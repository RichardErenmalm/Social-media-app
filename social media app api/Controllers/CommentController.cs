using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using social_media_app_api.Database;
using social_media_app_api.DTO;
using social_media_app_api.Services;

namespace social_media_app_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            var comments = await _commentService.GetAllCommentsAsync();
            return Ok(comments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
            var comment = await _commentService.GetCommentAsync(id);
            if (comment == null)
                return NotFound();

            return comment;
        }

        [HttpPost]
        public async Task<ActionResult<Comment>> PostComment(CreateCommentDto createCommentDto)
        {
            var createdComment = await _commentService.CreateCommentAsync(createCommentDto);
            return CreatedAtAction(nameof(GetComment), new { id = createdComment.CommentId }, createdComment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(int id, UpdateCommentDto createCommentDto)
        {
            var success = await _commentService.UpdateCommentAsync(id, createCommentDto);
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var success = await _commentService.DeleteCommentAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }

}
