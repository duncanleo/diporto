using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Diporto.Models;
using Diporto.Database;
using System.Linq;
using System.Threading.Tasks;

namespace Diporto.Controllers {
  [Authorize]
  [Route("api/review")]
  public class PlaceReviewController : Controller {
    private readonly DatabaseContext context;
    private UserManager<User> userManager;
    public PlaceReviewController(DatabaseContext context, UserManager<User> userManager) {
      this.context = context;
      this.userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] JObject payload) {
      if (payload["place_id"] == null) {
        return BadRequest();
      }
      var placeId = payload["place_id"].Value<int>();
      var review = payload.ToObject<PlaceReview>();
      var place = context.Places.FirstOrDefault(p => p.Id == placeId);

      if (!TryValidateModel(review) || place == null) {
        return BadRequest();
      }

      var user = await userManager.GetUserAsync(User);
      review.User = user;

      if (place.PlaceReviews == null) {
        place.PlaceReviews = new List<PlaceReview>();
      }
      place.PlaceReviews.Add(review);
      context.Places.Update(place);
      context.SaveChanges();

      return Ok();
    }
  }
}
