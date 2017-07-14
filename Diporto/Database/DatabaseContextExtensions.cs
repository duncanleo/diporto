using System.Linq;
using Microsoft.EntityFrameworkCore;
using Diporto.Models;

namespace Diporto.Database {
  public static class DatabaseContextExtensions {
    public static void EnsureSeedData(this DatabaseContext context) {
      if (context.Database.GetPendingMigrations().Any()) {
        return;
      }

      if (!context.Users.Any()) {
        // Seed users
      }
    }
  }
}
