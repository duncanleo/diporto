using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Diporto.Database;
using Diporto.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Diporto.Controllers {
  [Authorize]
  [Route("api/bookmarks")]
  public class BookmarkController : Controller {
    private readonly DatabaseContext context;
    private UserManager<User> userManager;

    public BookmarkController(DatabaseContext context, UserManager<User> userManager) {
      this.context = context;
      this.userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetOwnBookmarks() {
      var user = await userManager.GetUserAsync(User);
      
      var bookmarks = context.UserPlaceBookmarks
        .Where(b => b.UserId == user.Id)
        .Include(b => b.Place)
        .ToList();

      return new ObjectResult(bookmarks);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserPlaceBookmark model) {
      if (!ModelState.IsValid) {
        return BadRequest();
      }

      var place = context.Places.FirstOrDefault(p => p.Id == model.PlaceId);
      if (place == null) {
        return NotFound();
      }

      var user = await userManager.GetUserAsync(User);
      var bookmark = new UserPlaceBookmark {
        User = user,
        Place = place,
      };

      context.UserPlaceBookmarks.Add(bookmark);
      context.SaveChanges();
      return new ObjectResult(bookmark);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UserPlaceBookmark model) {
      var bookmark = context.UserPlaceBookmarks.FirstOrDefault(upb => upb.Id == id);

      if (bookmark == null) {
        return NotFound();
      }

      var user = await userManager.GetUserAsync(User);
      if (bookmark.UserId != user.Id) {
        return StatusCode((int)HttpStatusCode.Forbidden);
      }

      bookmark.PlaceId = model.PlaceId;
      context.UserPlaceBookmarks.Update(bookmark);
      context.SaveChanges();
      return new ObjectResult(bookmark);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) {
      var bookmark = context.UserPlaceBookmarks.FirstOrDefault(upb => upb.Id == id);
      if (bookmark == null) {
        return NotFound();
      }
      var user = await userManager.GetUserAsync(User);
      if (bookmark.UserId != user.Id) {
        return StatusCode((int)HttpStatusCode.Forbidden);
      }
      context.UserPlaceBookmarks.Remove(bookmark);
      context.SaveChanges();
      return new NoContentResult();
    }
  }
}