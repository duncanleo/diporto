using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Diporto.Models;
using Diporto.Controllers;
using NpgsqlTypes;

namespace Diporto.Database {
  public static class DatabaseContextExtensions {
    public static void EnsureSeedData(this DatabaseContext context) {
      if (context.Database.GetPendingMigrations().Any()) {
        return;
      }

      if (!context.Users.Any()) {
        var passwordHasher = new PasswordHasher<User>();
        
        // Seed users
        var adminUser = new User {
          Name = "John Smith",
          Email = "johnsmith@gmail.com",
          UserName = "johnsmith",
          IsAdmin = true,
          CurrentLocation = new PostgisPoint(103.790098, 1.307319),
          NormalizedUserName = "JOHNSMITH",
          SecurityStamp = System.Guid.NewGuid().ToString()
        };

        adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "PowerPower123456");

        context.Users.Add(adminUser);

        // Room one
        var roomOne = new Room {
          Name = "Jurong buddies",
          Owner = adminUser
        };
        context.Rooms.Add(roomOne);
        context.SaveChanges();
        roomOne.ShortCode = RoomController.hashids.Encode(roomOne.Id);

        var roomOneUsers = new[] {
          generateUser(new { Name = "Tommy Lau", UserName = "tlau", Lat = 1.348016, Lon = 103.719112 }),
          generateUser(new { Name = "Gathbiyya Jovan", UserName = "gjovan", Lat = 1.353234, Lon = 103.739796 }),
          generateUser(new { Name = "Fuad Isabel", UserName = "fisabel", Lat = 1.347307, Lon = 103.746614 }),
          generateUser(new { Name = "Nereus Yeruslan", UserName = "nyeruslan", Lat = 1.330544, Lon = 103.744196 }),
        };

        for (int i = 0; i < roomOneUsers.Count(); i++) {
          var user = roomOneUsers.ElementAt(i);
          context.Users.Add(user);
          context.SaveChanges();
          user.PasswordHash = passwordHasher.HashPassword(user, "PasswordPassword123");
        }
        context.Users.UpdateRange(roomOneUsers);

        var roomOneMemberships = roomOneUsers.Select(user => new RoomMembership { Room = roomOne, User = user }).ToList();
        roomOneMemberships.Add(new RoomMembership { Room = roomOne, User = adminUser });

        context.RoomMemberships.AddRange(roomOneMemberships);
        context.Rooms.Update(roomOne);
        context.SaveChanges();

        // Room two
        var roomTwo = new Room {
          Name = "Shop everyday!",
          Owner = adminUser
        };
        context.Rooms.Add(roomTwo);
        context.SaveChanges();
        roomTwo.ShortCode = RoomController.hashids.Encode(roomTwo.Id);

        var roomTwoUsers = new[] {
          generateUser(new { Name = "Rakel Arun", UserName = "rarun", Lat = 1.309899, Lon = 103.837120 }),
          generateUser(new { Name = "Joram Lukas", UserName = "jlukas", Lat = 1.303703, Lon = 103.849460 }),
          generateUser(new { Name = "Raanan Silvio", UserName = "rsilvio", Lat = 1.293654, Lon = 103.830316 }),
          generateUser(new { Name = "Eurwen Vilhelm", UserName = "evilhelm", Lat = 1.300078, Lon = 103.848345 })
        };

        for (int i = 0; i < roomTwoUsers.Count(); i++) {
          var user = roomTwoUsers.ElementAt(i);
          context.Users.Add(user);
          context.SaveChanges();
          user.PasswordHash = passwordHasher.HashPassword(user, "PasswordPassword123");
        }
        context.Users.UpdateRange(roomOneUsers);

        var roomTwoMemberships = roomTwoUsers.Select(user => new RoomMembership { Room = roomTwo, User = user }).ToList();
        roomTwoMemberships.Add(new RoomMembership { Room = roomTwo, User = adminUser });

        context.RoomMemberships.AddRange(roomTwoMemberships);
        context.Rooms.Update(roomTwo);
        context.SaveChanges();
      }
    }

    private static User generateUser(dynamic data) {
      return new User {
        Name = data.Name,
        Email = $"{data.UserName}@gmail.com",
        UserName = data.UserName,
        CurrentLocation = new PostgisPoint(data.Lon, data.Lat),
        IsAdmin = false,
        NormalizedUserName = data.UserName.ToUpper(),
        SecurityStamp = System.Guid.NewGuid().ToString(),
      };
    }
  }
}
