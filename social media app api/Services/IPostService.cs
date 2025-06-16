using social_media_app_api.DTO;

namespace social_media_app_api.Services
{
    public interface IPostService
    {
        Task<IEnumerable<Post>> GetAllPostsAsync( string? sort, string? filter);
        Task<Post?> GetPostAsync(int id);
        Task<Post> CreatePostAsync(CreatePostDto createPostDto);
        Task<bool> UpdatePostAsync(int id, CreatePostDto createPostDto);
        Task<bool> DeletePostAsync(int id);
    }
}
