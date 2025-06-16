using Microsoft.EntityFrameworkCore;
using social_media_app_api.Database;
using social_media_app_api.DTO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace social_media_app_api.Services
{
    public class PostService : IPostService
    {
        private readonly AppDbContext _context;

        public PostService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync(string? sort, string? filter)
        {

            var query = _context.Posts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(p =>
                p.Text != null && p.Text.Contains(filter));
            }



            if (sort?.ToLower() == "desc")
            {
                query = query.OrderByDescending(p => p.PublicationTime); 
            }
            else if (sort?.ToLower() == "asc")
            {
                query = query.OrderBy(p => p.PublicationTime); 
            }

            return await query.ToListAsync();
        }

        public async Task<Post?> GetPostAsync(int id)
        {
            return await _context.Posts.FindAsync(id);
        }

        public async Task<Post> CreatePostAsync(CreatePostDto createPostDto)
        {
            var user = await _context.Users.FindAsync(createPostDto.UserId);
            if (user == null)
                throw new ArgumentException("Invalid PublisherId");

            var post = new Post
            {
                Text = createPostDto.Text,
                ImageURL = createPostDto.ImageURL,
                PublisherId = createPostDto.UserId,
                PublicationTime = DateTime.Now
            };
           
             
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return post;
        }

        public async Task<bool> UpdatePostAsync(int id, CreatePostDto updatedPostDto)
        {
            var existingPost = await _context.Posts.FindAsync(id);

            if (existingPost == null)
                return false;

            // Uppdatera fälten från DTO:n
            existingPost.Text = updatedPostDto.Text;
            existingPost.ImageURL = updatedPostDto.ImageURL;
            existingPost.PublisherId = updatedPostDto.UserId;


            _context.Entry(existingPost).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Posts.AnyAsync(e => e.PostId == id))
                    return false;
                throw;
            }
        }


        public async Task<bool> DeletePostAsync(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
                return false;

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
