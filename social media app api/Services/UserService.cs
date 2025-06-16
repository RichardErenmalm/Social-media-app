using Microsoft.EntityFrameworkCore;
using social_media_app_api.Database;
using social_media_app_api.DTO;

namespace social_media_app_api.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UpdateUserAsync(int id, RegisterDto updatedUserDto)
        {
            // Hämta användaren som ska uppdateras
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;

            // Uppdatera användarens fält baserat på Dto:n
            user.Username = updatedUserDto.Username;
            user.Gmail = updatedUserDto.Gmail;
            user.Password = updatedUserDto.Password;
            user.Name = updatedUserDto.Name;

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Users.AnyAsync(u => u.UserId == id))
                    return false;

                throw;
            }
        }


        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        }

        public async Task<User> RegisterUserAsync(RegisterDto dto)
        {
            var user = new User
            {
                Username = dto.Username,
                Gmail = dto.Gmail,
                Name = dto.Name,
                Password = dto.Password // OBS: I riktiga system ska detta vara hashat!
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
