using social_media_app_api.DTO;

namespace social_media_app_api.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User> CreateUserAsync(User user);
        Task<bool> UpdateUserAsync(int id, RegisterDto updatedUser);
        Task<bool> DeleteUserAsync(int id);
        Task<User?> AuthenticateAsync(string username, string password);
        Task<User> RegisterUserAsync(RegisterDto dto);
    }
}
