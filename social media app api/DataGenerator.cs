using Bogus;

namespace social_media_app_api
{
    public class DataGenerator
    {
        Faker<PersonModel> personModelFake;

        public DataGenerator() 
        {
            Randomizer.Seed = new Random(1234567890);

            personModelFake = new Faker<PersonModel>()
                //.RuleFor(p => p.UserId, f => f.IndexFaker)
                .RuleFor(p => p.Username, f => f.Internet.UserName())
                .RuleFor(p => p.Gmail, f => f.Internet.Email())
                .RuleFor(p => p.Password, f => f.Internet.Password())
                .RuleFor(p => p.Name, f => f.Name.FullName());
        }

        public List<PersonModel> GeneratePeople()
        {

            return personModelFake.Generate(10);
        }

    }
}
