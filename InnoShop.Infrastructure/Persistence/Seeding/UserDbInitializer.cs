using InnoShop.Domain.Entities;
using InnoShop.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.Infrastructure.Persistence.Seeding
{
    public static class UserDbInitializer
    {
        public static async Task SeedAsync(UserDbContext context)
        {
            if (await context.Users.AnyAsync()) return;

            var users = new List<User>
            {
                new User
                {
                    Id = SeedingConstants.AdminId, 
                    Name = "Super Admin",
                    Email = "admin@gmail.com",
                    Role = "Admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin1234", 12),
                    IsActive = true,
                    IsEmailConfirmed = true
                },
                new User
                {
                    Id = SeedingConstants.User1Id, 
                    Name = "Alex User",
                    Email = "alex@gmail.com",
                    Role = "User",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("user1234", 12),
                    IsActive = true,
                    IsEmailConfirmed = true
                },
                new User
                {
                    Id = SeedingConstants.User2Id,
                    Name = "Ivan User",
                    Email = "alex@gmail.com.",
                    Role = "User",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("user1234", 12),
                    IsActive = true,
                    IsEmailConfirmed = true
                }
            };

            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();
        }
    }
}
