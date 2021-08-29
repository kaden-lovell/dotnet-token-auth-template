using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Server.Models;
using Server.Persistence;

public class UserSeeder {
    public static void Initialize(DataContext context, IServiceProvider services) {
        // Get a logger
        var logger = services.GetRequiredService<ILogger<UserSeeder>>();
        if (context.Database.CanConnect()) {
            logger.LogInformation("Database already exists.");
            return;
        }

        // Make sure the database is created
        context.Database.EnsureCreated();
        logger.LogInformation("Start seeding the database.");

        var users = new List<User>();
        users.Add(new User {
            Email = "user@kadenlovell.com",
            Username = "user",
            Password = "user",
            Role = "User",
            CreatedDate = DateTime.UtcNow,
        });

        users.Add(new User {
            Email = "support@kadenlovell.com",
            Username = "support",
            Password = "support",
            Role = "Support",
            CreatedDate = DateTime.UtcNow,
        });

        users.Add(new User {
            Email = "management@kadenlovell.com",
            Username = "management",
            Password = "management",
            Role = "Management",
            CreatedDate = DateTime.UtcNow,
        });

        context.User.AddRange(users);
        context.SaveChanges();

        logger.LogInformation("Finished seeding the database.");
    }
}