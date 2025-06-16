using social_media_app_api.Database;
using Microsoft.EntityFrameworkCore;
using social_media_app_api.DTO;

namespace social_media_app_api.Services
{
    public class LikeService : ILikeService
    {
        private readonly AppDbContext _context;

        public LikeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Like>> GetAllLikesAsync()
        {
            return await _context.Likes.ToListAsync();
        }

        public async Task<Like?> GetLikeByIdAsync(int id)
        {
            return await _context.Likes.FindAsync(id);
        }

        public async Task<(bool Success, string? ErrorMessage, Like? Like)> CreateLikeAsync(CreateLikeDto likeDto)
        {
            if (likeDto.PostId == null && likeDto.CommentId == null)
                return (false, "Like must be associated with either a Post or a Comment.", null);

            if (likeDto.PostId != null && !await _context.Posts.AnyAsync(p => p.PostId == likeDto.PostId))
                return (false, $"Post with ID {likeDto.PostId} does not exist.", null);

            if (likeDto.CommentId != null && !await _context.Comments.AnyAsync(c => c.CommentId == likeDto.CommentId))
                return (false, $"Comment with ID {likeDto.CommentId} does not exist.", null);

            var like = new Like
            {
                LikedById = likeDto.LikedById,
                PostId = likeDto.PostId,
                CommentId = likeDto.CommentId,
                PublicationTime = DateTime.Now
            };

            _context.Likes.Add(like);
            await _context.SaveChangesAsync();

            return (true, null, like);
        }



        public async Task<bool> DeleteLikeAsync(int id)
        {
            var like = await _context.Likes.FindAsync(id);
            if (like == null)
                return false;

            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
