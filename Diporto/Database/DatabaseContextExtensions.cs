using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Diporto.Models;

namespace Diporto.Database {
  public static class DatabaseContextExtensions {
    public static void EnsureSeedData(this DatabaseContext context) {
      if (context.Database.GetPendingMigrations().Any()) {
        return;
      }

      if (context.Users.FirstOrDefault(u => u.UserName == "johnsmith") == null) {
        // Seed users
        var adminUser = new User {
          Name = "John Smith",
          Email = "johnsmith@gmail.com",
          UserName = "johnsmith",
          IsAdmin = true,
          NormalizedUserName = "JOHNSMITH",
          SecurityStamp = System.Guid.NewGuid().ToString()
        };

        var passwordHasher = new PasswordHasher<User>();
        var hashedPass = passwordHasher.HashPassword(adminUser, "PowerPower123456");
        adminUser.PasswordHash = hashedPass;

        context.Users.Add(adminUser);
        context.SaveChanges();
      }
    }
  }
}
