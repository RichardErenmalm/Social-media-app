using social_media_app_api.DTO;

namespace social_media_app_api.Services
{
    public interface ICommentService
    {
        Task<IEnumerable<Comment>> GetAllCommentsAsync();
        Task<Comment?> GetCommentAsync(int id);
        Task<Comment> CreateCommentAsync(CreateCommentDto createCommentDto);
        Task<bool> UpdateCommentAsync(int id, UpdateCommentDto createCommentDto);
        Task<bool> DeleteCommentAsync(int id);
    }
}
