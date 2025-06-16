using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using social_media_app_api.Database;
namespace social_media_app_api
{

    public class CheckIfDbIsEmpty
    {
        private readonly AppDbContext? _context;

        public CheckIfDbIsEmpty(AppDbContext context)
        {
            _context = context;
        }
        public bool DbIsEmpty()
        {

            return !_context.Users.Any();   

            //var users = _context.Users.ToList();

            //if (users.Count == 0)
            // {
            //     return true;
            // }
            //else
            //{
            //    return false;
            //}
        }
    }
}
