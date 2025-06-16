
using social_media_app_api.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using social_media_app_api.JWTHelper;
using FluentValidation.AspNetCore;
using FluentValidation;
using social_media_app_api.DTO;
using social_media_app_api.Seeders;
using Microsoft.Extensions.Options;
using social_media_app_api.Services;

namespace social_media_app_api
{
    public class Program
    {
       
        public static void  Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<RegisterDtoValidator>();
            builder.Services.AddHttpClient<QuoteService>();


            //för seeder
            builder.Services.AddTransient<DataGenerator>();


            ////kanske kan ta bort
            //builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            //builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //gör JWT generator scoped. alltså att den skapar en ny instans varjw http request och återanvänder under http requesten 
            builder.Services.AddScoped<JWTGenerator>();

            builder.Services.AddLogging(options => options.AddConsole());

            //refererar till "Jwt" i appsettings.json


            //bestämmer hur vi ska validera token
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                var jwtSettings = builder.Configuration.GetSection("JwtConfig");
                var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            builder.Services.AddAuthorization();


            //lägg till db service
            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<ICommentService, CommentService>();
            builder.Services.AddScoped<ILikeService, LikeService>();



            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                //context.Likes.RemoveRange(context.Likes);
                //context.Comments.RemoveRange(context.Comments);
                //context.Users.RemoveRange(context.Users);

                //context.SaveChanges();

                CheckIfDbIsEmpty checkIfDbIsEmpty = new CheckIfDbIsEmpty(context);
                if (checkIfDbIsEmpty.DbIsEmpty())
                {
                    DataGenerator datagen = new DataGenerator();
                    List<PersonModel> people = datagen.GeneratePeople();


                   


                    var users = people.Select(p => new User
                    {
                        //UserId = p.UserId,
                        Username = p.Username,
                        Gmail = p.Gmail,
                        Password = p.Password,
                        Name = p.Name
                    }).ToList();

                    context.Users.AddRange(users);
                    context.SaveChanges();
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseAuthentication();
            app.UseAuthorization();

            


            app.UseHttpsRedirection();

            app.MapControllers();

            app.Run();
        }
    }
}
