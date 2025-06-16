namespace social_media_app_api.Seeders
{
    using Bogus;
    using Microsoft.EntityFrameworkCore;
    using social_media_app_api.Database;
    using social_media_app_api.Models; // Ändra om din namespace är annorlunda

    public class UserSeeder
    {
        private readonly AppDbContext _context;

        public UserSeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (await _context.Users.AnyAsync()) return; // Om det redan finns data – gör inget

            var userFaker = new Faker<User>()
                .RuleFor(u => u.Username, f => f.Internet.UserName())
                .RuleFor(u => u.Gmail, f => f.Internet.Email())
                .RuleFor(u => u.Password, f => f.Internet.Password(8)) // Random lösenord, minst 8 tecken
                .RuleFor(u => u.Name, f => f.Name.FullName());

            var fakeUsers = userFaker.Generate(10); // Skapa 10 fejkanvändare

            await _context.Users.AddRangeAsync(fakeUsers);
            await _context.SaveChangesAsync();
        }


        
    }
}
