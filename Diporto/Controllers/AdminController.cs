using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Diporto.Database;
using Diporto.Models;
using Diporto.ViewModels.Admin;
using System.Threading.Tasks;
using System.Linq;

namespace Diporto.Controllers {
  [Route("admin")]
  [Authorize]
  public class AdminController : Controller {
    const int pageSize = 20;

    private readonly DatabaseContext context;
    private UserManager<User> userManager;
    public AdminController(DatabaseContext context, UserManager<User> userManager) {
      this.context = context;
      this.userManager = userManager;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Index() {
      var user = await userManager.GetUserAsync(User);
      if (user != null) {
        return RedirectToAction("Home");
      }
      return View();
    }

    [HttpGet("error")]
    [AllowAnonymous]
    public ViewResult Error() {
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
    [Route("places/edit/{id:int}")]
    public async Task<ViewResult> PlacesEdit(int id) {
      var user = await userManager.GetUserAsync(User);
      return View("PlacesFields", new PlacesFieldsViewModel {
        Name = user.Name,
        Place = context.Places.FirstOrDefault(p => p.Id == id),
        Mode = PlacesFieldsViewModel.FieldsMode.Edit
      });
    }

    [HttpGet]
    [Route("places/create")]
    public async Task<ViewResult> PlacesCreate() {
      var user = await userManager.GetUserAsync(User);
      return View("PlacesFields", new PlacesFieldsViewModel {
        Name = user.Name,
        Mode = PlacesFieldsViewModel.FieldsMode.Create
      });
    }

    [HttpGet]
    [Route("places/delete/{id:int}")]
    public async Task<ViewResult> PlacesDelete(int id) {
      var user = await userManager.GetUserAsync(User);
      return View(new PlaceDeleteViewModel {
        Name = user.Name,
        Place = context.Places.FirstOrDefault(p => p.Id == id)
      });
    }

    [HttpGet]
    [Route("users")]
    public async Task<ViewResult> Users(int page = 1) {
      if (page < 1) {
        page = 1;
      }
      var user = await userManager.GetUserAsync(User);

      var users = context.Users
        .Include(u => u.PlaceReviews)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToList();

      return View(new UsersViewModel {
        Name = user.Name,
        Users = users,
        PageIndex = page
      });
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
