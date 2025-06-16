using social_media_app_api.DTO;
namespace social_media_app_api.Services
{
    public interface ILikeService
    {
        Task<IEnumerable<Like>> GetAllLikesAsync();
        Task<Like?> GetLikeByIdAsync(int id);
        Task<(bool Success, string? ErrorMessage, Like? Like)> CreateLikeAsync(CreateLikeDto createLikeDto);
        Task<bool> DeleteLikeAsync(int id);
    }
}
