using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Diporto.Database;
using Diporto.Models;
using System.Linq;
using NpgsqlTypes;

namespace Diporto.Controllers {
  [Authorize]
  [Route("api/places")]
  public class PlaceController : Controller {
    const int pageSize = 20;

    private readonly DatabaseContext context;
    public PlaceController(DatabaseContext context) {
      this.context = context;
    }

    [HttpGet("all")]
    [AllowAnonymous]
    public IEnumerable<Place> GetAll(int page = 1, string categories = "") {
      return context.Places
        .Include(place => place.PlacePhotos)
        .Include(place => place.PlaceCategories)
        .Include(place => place.PlaceReviews)
        .Where(place => categories.Length > 0 ? place.PlaceCategories.Select(pc => pc.Category.Name).Intersect(categories.Split('|')).Any() : true)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToList();
    }

    [HttpPost]
    public IActionResult Create([FromBody] Place place) {
      if (!ModelState.IsValid) {
        return StatusCode((int)HttpStatusCode.BadRequest);
      }

      var categories = context.Categories.Where(cat => place.Categories.Contains(cat.Name));

      context.Places.Add(place);
      context.SaveChanges();
      
      foreach (var category in categories) {
        place.PlaceCategories.Add(new PlaceCategory {
          Place = place,
          Category = category,
        });
      }
      context.Places.Update(place);
      context.SaveChanges();
      return StatusCode((int)HttpStatusCode.OK);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public IActionResult GetById(int id) {
      var result = context.Places
        .Include(place => place.PlaceCategories)
          .ThenInclude(pc => pc.Category)
        .Include(place => place.PlacePhotos)
        .Include(place => place.PlaceReviews)
        .FirstOrDefault(place => place.Id == id);

        if (result == null) {
          return NotFound();
        }

      result.Categories = result.PlaceCategories.Select(pc => pc.Category.Name);

      return new ObjectResult(result);
    }

    [HttpGet("nearby")]
    [AllowAnonymous]
    public IEnumerable<Place> GetNearby(double lat, double lon, string categories = "", int numResults = 5) {
      var places = context.Places
        .FromSql("SELECT *, ST_Distance(ST_SetSRID(ST_MakePoint(p.lon,p.lat),4326), ST_SetSRID(ST_MakePoint({0},{1}),4326)) AS distance FROM place p ORDER BY distance", lon, lat)
        .Where(place => categories.Length > 0 ? place.PlaceCategories.Select(pc => pc.Category.Name).Intersect(categories.Split('|')).Any() : true)
        .Include(place => place.PlaceCategories)
          .ThenInclude(pc => pc.Category)
        .Include(place => place.PlacePhotos)
        .Include(place => place.PlaceReviews)
        .Take(numResults)
        .ToList();

      foreach(Place place in places) {
        place.Categories = place.PlaceCategories.Select(pc => pc.Category.Name);
      }
      
      return places;
    }

    [HttpGet]
    public IActionResult GetPlacesByRoomId(int roomId) {
      var room = context.Rooms
        .Include(r => r.RoomMemberships)
          .ThenInclude(rm => rm.User)
        .FirstOrDefault(r => r.Id == roomId);
      if (room == null) {
        return NotFound();
      }

      var locations = room.RoomMemberships.Select(rm => rm.User.CurrentLocation).ToList();

      var places = context.Places
        .FromSql(
          @"SELECT * FROM place p WHERE ST_Within(
            ST_MakePoint(p.lon, p.lat),
            ST_Envelope({0})
          )
          ", 
          new PostgisLineString(locations.Select(loc => new Coordinate2D(loc.X, loc.Y)))
        );

      return new ObjectResult(places);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] Place item) {
      if (item == null || item.Id != id) {
        return BadRequest();
      }

      var place = context.Places.FirstOrDefault(p => p.Id == id);
      if (place == null) {
        return NotFound();
      }

      place.Name = item.Name;
      place.OpeningHours = item.OpeningHours;
      place.Address = item.Address;
      place.Phone = item.Phone;

      context.Places.Update(place);
      context.SaveChanges();

      return new NoContentResult();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id) {
      var place = context.Places.FirstOrDefault(p => p.Id == id);
      if (place == null) {
        return NotFound();
      }

      context.Places.Remove(place);
      context.SaveChanges();
      return new NoContentResult();
    }
  }
}
