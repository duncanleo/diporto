using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Diporto.Database;
using Diporto.Models;
using Diporto.ViewModels.Admin;
using System.Threading.Tasks;
using System.Linq;

namespace Diporto.Controllers {
  [Route("admin")]
  public class AdminController : Controller {
    const int pageSize = 20;

    private readonly DatabaseContext context;
    private UserManager<User> userManager;
    public AdminController(DatabaseContext context, UserManager<User> userManager) {
      this.context = context;
      this.userManager = userManager;
    }

    [HttpGet]
    public ViewResult Index() {
      return View();
    }

    [HttpGet]
    [Route("places")]
    public async Task<ViewResult> Places(int page = 1) {
      if (page <= 1) {
        page = 1;
      }
      var user = await userManager.GetUserAsync(User);
      var places = context.Places
        // .Where(place => categories.Length > 0 ? place.PlaceCategories.Select(pc => pc.Category.Name).Intersect(categories.Split('|')).Any() : true)
        .Include(place => place.PlacePhotos)
        .Include(place => place.PlaceReviews)
          .ThenInclude(review => review.User)
        .Include(place => place.PlaceCategories)
          .ThenInclude(pc => pc.Category)
        .Skip((page - 1) * pageSize)
          .Take(pageSize)
          .ToList();
      return View(new PlacesViewModel{
        Name = user.Name,
        Places = places,
        PageIndex = page
      });
    }

    [HttpGet]
    [Route("users")]
    public ViewResult Users() {
      return View();
    }

    [HttpGet]
    [Route("home")]
    public async Task<ViewResult> Home() {
      var user = await userManager.GetUserAsync(User);
      return View(new HomeViewModel {
        Name = user.Name
      });
    }
  }
}
