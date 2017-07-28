using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Diporto.Models;
using Diporto.Database;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Diporto.Controllers {
  [Authorize]
  [Route("api/reviews")]
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

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id) {
      var review = context.PlaceReviews.FirstOrDefault(pr => pr.Id == id);
      if (review == null) {
        return NotFound();
      }

      return new ObjectResult(review);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult GetPlaces(string username, int userId = -1, int placeId = -1)  {
      var reviews = from r in context.PlaceReviews
            select r;

      if (placeId != -1) {
        reviews = reviews.Where(pr => pr.PlaceId == placeId);
      }

      if (userId != -1) {
        reviews = reviews.Where(pr => pr.UserId == userId);
      }

      if (!string.IsNullOrEmpty(username)) {
        reviews = reviews.Where(pr => pr.User.UserName == username);
      }

      if (reviews == null) {
        return NotFound();
      }

      reviews =
        reviews
        .Include(r => r.User)
        .OrderByDescending(r => r.Time);

      return new ObjectResult(reviews);
    }


    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] PlaceReview item) {
      if (item == null) {
        return BadRequest();
      }

      var review = context.PlaceReviews.Include(rv => rv.User).FirstOrDefault(rv => rv.Id == id);
      if (review == null) {
        return NotFound();
      }

      // Ensure submitting user is owner of the review
      var user = await userManager.GetUserAsync(User);
      if (review.User.Id != user.Id) {
        return StatusCode((int)HttpStatusCode.Forbidden);
      }

      review.Text = item.Text;
      review.Time = item.Time;
      review.Rating = item.Rating;

      context.PlaceReviews.Update(review);
      context.SaveChanges();

      return new NoContentResult();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) {
      var review = context.PlaceReviews.Include(rv => rv.User).FirstOrDefault(rv => rv.Id == id);
      if (review == null) {
        return NotFound();
      }

      // Ensure request user is owner of the review
      var user = await userManager.GetUserAsync(User);
      if (review.User.Id != user.Id) {
        return StatusCode((int)HttpStatusCode.Forbidden);
      }

      context.PlaceReviews.Remove(review);
      context.SaveChanges();
      return new NoContentResult();
    }
  }
}
