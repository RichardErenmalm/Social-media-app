namespace social_media_app_api.DTO
{
    using FluentValidation;
    using Microsoft.EntityFrameworkCore;
    using social_media_app_api.Database;


    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        private readonly AppDbContext _context;
        public RegisterDtoValidator(AppDbContext context)
        {
            _context = context;


            RuleFor(x => x.Username)
                .NotEmpty()
                .Must(BeUniqueUsername).WithMessage("Användarnamnet är redan taget.");

            RuleFor(x => x.Gmail)
                .NotEmpty()
                .EmailAddress()
                .Must(BeUniqueEmail).WithMessage("E-postadressen används redan.");

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Användarnamn krävs.")
                .MinimumLength(3).WithMessage("Användarnamnet måste vara minst 3 tecken långt.");

            RuleFor(x => x.Gmail)
                .NotEmpty().WithMessage("Gmail krävs.")
                .EmailAddress().WithMessage("Gmail måste vara en giltig adress.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Lösenord krävs.")
                .MinimumLength(6).WithMessage("Lösenordet måste vara minst 6 tecken.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Namn krävs.")
                .MinimumLength(3).WithMessage("Namnet måste vara minst 3 tecken långt.");
        }

        private bool BeUniqueUsername(string username)
        {
            return !_context.Users.Any(u => u.Username == username);
        }

        private bool BeUniqueEmail(string gmail)
        {
            return !_context.Users.Any(u => u.Gmail == gmail);
        }
    }



    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        private readonly AppDbContext _context;
        public LoginDtoValidator()
        {
           
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Användarnamn krävs");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Lösenord krävs");

            RuleFor(x => x)
           .MustAsync(async (x, cancellation) => await CheckUsernameAndPassword(x))
           .WithMessage("Fel användarnamn eller lösenord");
        }

        //kolla så allt stämmer
        private async Task<bool> CheckUsernameAndPassword(LoginDto dto)
        {
            var user = await _context.Users
           .FirstOrDefaultAsync(u => u.Username == dto.Username);

            if (user == null)
            {
                // Om användarnamnet inte finns, returnera ett fel
                return false;
            }

            // Om användarnamnet finns, kontrollera om lösenordet är korrekt
            if (user.Password != dto.Password)
            {
                // Om lösenordet inte stämmer, returnera ett fel
                return false;
            }

            return true;
        }
    }

    public class CreatePostDtoValidator : AbstractValidator<CreatePostDto>
    {
        public CreatePostDtoValidator()
        {
            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Innehåll får inte vara tomt.")
                .MaximumLength(500).WithMessage("Maxlängd är 500 tecken.");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("Ogiltigt användar-ID.");
        }
    }
}
