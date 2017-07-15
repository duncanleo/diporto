using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Threading.Tasks;
using Diporto.Models;
using Diporto.ViewModels;

namespace Diporto.Controllers {
  [Route("api/[controller]")]
  public class AccountController : Controller {
    private readonly UserManager<User> userManager;
    private readonly SignInManager<User> signInManager;
    
    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager) {
      this.userManager = userManager;
      this.signInManager = signInManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost("register")]
    // [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model) {
      if (model.Password != model.ConfirmPassword) {
        return StatusCode((int)HttpStatusCode.BadRequest);
      }

      var newUser = new User {
        UserName = model.UserName,
        IsAdmin = false
      };

      var result = await userManager.CreateAsync(newUser, model.Password);
      if (result.Succeeded) {
        await signInManager.SignInAsync(newUser, isPersistent: false);
        return StatusCode((int)HttpStatusCode.OK);
      }
      return StatusCode((int)HttpStatusCode.InternalServerError);
    }
  }
}
