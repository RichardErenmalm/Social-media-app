using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using social_media_app_api.Database;
using social_media_app_api.DTO;
using social_media_app_api.Services;


namespace social_media_app_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        // GET: api/Post
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts(
      [FromQuery] string? sort = "asc",
         [FromQuery] string? filter = null)
        {
            var posts = await _postService.GetAllPostsAsync( sort, filter);
            return Ok(posts);
        }



        // GET: api/Post/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var post = await _postService.GetPostAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return post;
        }

        // POST: api/Post
        [HttpPost]
        public async Task<ActionResult<Post>> CreatePost([FromBody]CreatePostDto createPostDto)
        {
            // Kontrollera om PublisherId finns i Users
            try
            {
                // Anropa tjänsten för att skapa posten.
                var createdPost = await _postService.CreatePostAsync(createPostDto);
                return CreatedAtAction(nameof(GetPost), new { id = createdPost.PostId }, createdPost);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Post/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(int id, PostDto updatedPost)
        {
            if (id != updatedPost.PostId)
            {
                return BadRequest("Post ID mismatch.");
            }

            CreatePostDto UpdatedPostDto = new CreatePostDto
            {
                Text = updatedPost.Text,
                ImageURL = updatedPost.ImageURL,
                UserId = updatedPost.PostId
            };


            // Anropar service-lagret för att uppdatera posten
            var success = await _postService.UpdatePostAsync(id, UpdatedPostDto);

            if (!success)
            {
                return NotFound("Post not found or update conflict occurred.");
            }

            return NoContent();
        }

        // DELETE: api/Post/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            // Anropar service-lagret för att ta bort posten
            var success = await _postService.DeletePostAsync(id);

            if (!success)
            {
                return NotFound("Post not found.");
            }

            return NoContent();
        }

    }
}
