using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace social_media_app_api.JWTHelper
{
    public class JWTGenerator
    {
        //IConfiguration används för att hämta värden från appsettings.json
        private readonly IConfiguration _configuration;

        public JWTGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //token generator som skapar token för en användare
        public string JWTTokenGenereator(User user)
        {
            //hämtar JWT info från appsettings.json och skapar sedan en security key baserad på den hamliga nyckeln. skapar sedan en personlig signering för token
            var jwtSettings = _configuration.GetSection("JwtConfig");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //användarinfo som ska läggas in i token
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Gmail),
                //new Claim(ClaimTypes.Role, "Admin")
            };

            //skapar token med uppgifter som skapar, användare, claims, utgångstid för token och signering av token
            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
               // expires: DateTime.Now.AddMinutes(double.Parse(jwtSettings["TokenValidityMins"])),
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
            );

            //skickar tillbaka token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
