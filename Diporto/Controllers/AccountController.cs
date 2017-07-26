using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Diporto.Models;
using Diporto.ViewModels;
using Diporto.Database;
using NpgsqlTypes;

namespace Diporto.Controllers {
  [Route("api")]
  public class AccountController : Controller {
    private readonly DatabaseContext context;
    private readonly UserManager<User> userManager;
    private readonly SignInManager<User> signInManager;

    private IConfiguration configuration;
    
    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, DatabaseContext context, IConfiguration configuration) {
      this.userManager = userManager;
      this.signInManager = signInManager;
      this.context = context;
      this.configuration = configuration;
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
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model) {
      if (!ModelState.IsValid) {
        return StatusCode((int)HttpStatusCode.BadRequest);
      }

      var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, true, lockoutOnFailure: false);
      if (result.Succeeded) {
        var user = context.Users.First(u => u.UserName == model.UserName);
        if (!user.IsAdmin) {
          await signInManager.SignOutAsync();
          return RedirectToAction("Error", "Admin", new { area = "" });
        }
        return RedirectToAction("Home", "Admin", new { area = "" });
      }
      return RedirectToAction("Error", "Admin", new { area = "" });
    }

    [HttpPost("logout")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout() {
      await signInManager.SignOutAsync();
      return RedirectToAction("Index", "Admin", new { area = "" });
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

    [HttpPut("location")]
    [Authorize]
    public async Task<IActionResult> UpdateLocation([FromBody] UpdateLocationViewModel model) {
      if (!ModelState.IsValid) {
        return BadRequest();
      }

      var user = await userManager.GetUserAsync(User);
      user.CurrentLocation = new PostgisPoint(model.Lon, model.Lat);
      context.Users.Update(user);
      context.SaveChanges();
      return Ok();
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetOwnProfile() {
      var user = await userManager.GetUserAsync(User);
      var dbUser = context.Users
        .Include(u => u.PlaceReviews)
        .First(u => u.Id == user.Id);
      return new ObjectResult(dbUser);
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

    [HttpPost("users/{id:int}/admin")]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleAdmin(int id) {
      var user = await userManager.GetUserAsync(User);
      if (!user.IsAdmin) {
        return Forbid();
      }

      var targetUser = context.Users.FirstOrDefault(u => u.Id == id);
      if (targetUser == null) {
        return NotFound();
      }

      targetUser.IsAdmin = !targetUser.IsAdmin;

      context.Users.Update(targetUser);
      context.SaveChanges();

      return RedirectToAction("Users", "Admin", new { area = "" });
    }

    [HttpPost("token")]
    public async Task<IActionResult> Token([FromBody] RequestTokenViewModel model) {
      if (!ModelState.IsValid) {
        return BadRequest();
      }

      var tokenValidationParams = new TokenValidationParameters {
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("AppConfiguration:Key").Value)),
          ValidAudience = configuration.GetSection("AppConfiguration:SiteUrl").Value,
          ValidateIssuerSigningKey = true,
          ValidateLifetime = true,
          ValidIssuer = configuration.GetSection("AppConfiguration:SiteUrl").Value
      };

      JwtSecurityToken refreshToken;
      var jwtTokenhandler = new JwtSecurityTokenHandler();
      User user;

      switch(model.GrantType) {
        case "refresh_token":
          try {
            SecurityToken validatedToken;
            refreshToken = jwtTokenhandler.ReadJwtToken(model.RefreshToken);

            var claims = jwtTokenhandler.ValidateToken(model.RefreshToken, tokenValidationParams, out validatedToken);
            var userName = refreshToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;

            user = await userManager.FindByNameAsync(userName);
          } catch (Exception) {
            // Could not decode refresh token
            return StatusCode(401);
          }
          break;
        case "access_token":
          user = await userManager.FindByNameAsync(model.UserName);

          if (user == null || new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, model.Password) != PasswordVerificationResult.Success) {
            return StatusCode(401);
          }
          break;
        default:
          return BadRequest();
      }

      var accessToken = await GetJwtSecurityToken(user);
      refreshToken = await GetJwtSecurityRefreshToken(user);
      return Ok(new {
        token = jwtTokenhandler.WriteToken(accessToken),
        refresh_token = jwtTokenhandler.WriteToken(refreshToken),
        expiration = accessToken.ValidTo
      });
    }

    private async Task<JwtSecurityToken> GetJwtSecurityToken(User user) {
      var userClaims = await userManager.GetClaimsAsync(user);

      return new JwtSecurityToken(
        issuer: configuration.GetSection("AppConfiguration:SiteUrl").Value,
        audience: configuration.GetSection("AppConfiguration:SiteUrl").Value,
        claims: GetTokenClaims(user).Union(userClaims),
        expires: DateTime.UtcNow.AddMinutes(30),
        signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("AppConfiguration:Key").Value)), SecurityAlgorithms.HmacSha256)
      );
    }

    private static IEnumerable<Claim> GetTokenClaims(User user) {
      return new List<Claim> {
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Sub, $"{user.Id}")
      };
    }

    private async Task<JwtSecurityToken> GetJwtSecurityRefreshToken(User user) {
      var userClaims = await userManager.GetClaimsAsync(user);

      return new JwtSecurityToken(
        issuer: configuration.GetSection("AppConfiguration:SiteUrl").Value,
        audience: configuration.GetSection("AppConfiguration:SiteUrl").Value,
        claims: GetRefreshTokenClaims(user).Union(userClaims),
        expires: DateTime.UtcNow.AddDays(7),
        signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("AppConfiguration:Key").Value)), SecurityAlgorithms.HmacSha256)
      );
    }

    private static IEnumerable<Claim> GetRefreshTokenClaims(User user) {
      return new List<Claim> {
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Sub, $"{user.UserName}")
      };
    }
  }
}
