using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using System.Threading.Tasks;
using System.Linq;
using Diporto.Models;
using Diporto.ViewModels;
using Diporto.Database;

namespace Diporto.Controllers {
  [Route("api")]
  public class AccountController : Controller {
    private readonly DatabaseContext context;
    private readonly UserManager<User> userManager;
    private readonly SignInManager<User> signInManager;
    
    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, DatabaseContext context) {
      this.userManager = userManager;
      this.signInManager = signInManager;
      this.context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterViewModel model) {
      if (model.Password != model.ConfirmPassword || !ModelState.IsValid) {
        return StatusCode((int)HttpStatusCode.BadRequest);
      }

      var newUser = new User {
        UserName = model.UserName,
        Name = model.Name,
        IsAdmin = false
      };

      var result = await userManager.CreateAsync(newUser, model.Password);
      if (result.Succeeded) {
        await signInManager.SignInAsync(newUser, isPersistent: false);
        return StatusCode((int)HttpStatusCode.OK);
      }
      return StatusCode((int)HttpStatusCode.InternalServerError);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginViewModel model) {
      if (!ModelState.IsValid) {
        return StatusCode((int)HttpStatusCode.BadRequest);
      }

      var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, true, lockoutOnFailure: false);
      if (result.Succeeded) {
        return StatusCode((int)HttpStatusCode.OK);
      }

      if (result.IsLockedOut) {
        return StatusCode((int)HttpStatusCode.Conflict);
      } else {
        return StatusCode((int)HttpStatusCode.Forbidden);
      }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout() {
      await signInManager.SignOutAsync();
      return StatusCode((int)HttpStatusCode.OK);
    }

    [HttpPost("password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model) {
      if (!ModelState.IsValid) {
        return BadRequest();
      }

      var user = await userManager.GetUserAsync(User);
      await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
      return Ok();
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetOwnProfile() {
      var user = await userManager.GetUserAsync(User);
      return new ObjectResult(new {
        name = user.Name,
        email = user.Email,
        username = user.UserName
      });
    }

    [HttpGet("users/{id:int}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id) {
      var user = await userManager.GetUserAsync(User);
      if (!user.IsAdmin) {
        return Forbid();
      }

      var targetUser = context.Users.FirstOrDefault(u => u.Id == id);
      if (targetUser == null) {
        return NotFound();
      }

      return new ObjectResult(targetUser);
    }

    [HttpPut("users/{id:int}")]
    [Authorize]
    // Update user. NOTE: WILL NOT UPDATE PASSWORDS.
    public async Task<IActionResult> Update(int id, [FromBody] User payload) {
      var user = await userManager.GetUserAsync(User);
      if (!user.IsAdmin && user.Id != id) {
        return Forbid();
      }

      var targetUser = context.Users.FirstOrDefault(u => u.Id == id);
      if (targetUser == null) {
        return NotFound();
      }

      targetUser.IsAdmin = payload.IsAdmin;
      targetUser.Email = payload.Email;
      targetUser.IsAdmin = payload.IsAdmin;
      targetUser.Name = payload.Name;
      targetUser.UserName = payload.UserName;

      context.Users.Update(targetUser);
      context.SaveChanges();

      return new NoContentResult();
    }

    [HttpDelete("users/{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id) {
      var user = await userManager.GetUserAsync(User);
      if (!user.IsAdmin) {
        return Forbid();
      }

      var targetUser = context.Users.FirstOrDefault(u => u.Id == id);
      if (targetUser == null) {
        return NotFound();
      }

      context.Users.Remove(targetUser);
      context.SaveChanges();

      return new ObjectResult(targetUser);
    }
  }
}
