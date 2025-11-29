using Bogus;
using InnoShop.ProductDomain.Entities;
using InnoShop.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace InnoShop.ProductInfrastructure.Persistence.Seeding
{
    public static class ProductDbInitializer
    {
        public static async Task SeedAsync(ProductDbContext context)
        {
            if (await context.Products.AnyAsync())
            {
                return;
            }

            var faker = new Faker<Product>("ru")
                .RuleFor(p => p.Id, f => Guid.NewGuid())
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price(10, 5000)))
                .RuleFor(p => p.IsAvailable, f => f.Random.Bool(0.8f))
                .RuleFor(p => p.CreatedAt, f => f.Date.Past(1).ToUniversalTime())
                .RuleFor(p => p.UserId, f => f.PickRandom(SeedingConstants.AllUserIds));

            var products = faker.Generate(50);

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }
    }
}