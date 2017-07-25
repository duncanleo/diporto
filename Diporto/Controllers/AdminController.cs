using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Diporto.Database;
using Diporto.Models;
using Diporto.ViewModels.Admin;
using System.Threading.Tasks;

namespace Diporto.Controllers {
  [Route("admin")]
  public class AdminController : Controller {
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
    public ViewResult Places() {
      return View();
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
