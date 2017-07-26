using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Diporto.Database;
using Diporto.Models;
using System.Linq;
using System;
using NpgsqlTypes;

namespace Diporto.Controllers {
  [Authorize]
  [Route("api/places")]
  public class PlaceController : Controller {
    private enum Result { NotFound, DatabaseError, Ok };
    const int pageSize = 20;

    private readonly DatabaseContext context;
    public PlaceController(DatabaseContext context) {
      this.context = context;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult GetAll(int roomId = -1, string term = "", int page = 1, string categories = "", double lat = -1.0, double lon = -1.0, int numResults = -1) {
      List<Place> places = null;

      var commonQueriedPlaces = context.Places
        .Where(place => categories.Length > 0 ? place.PlaceCategories.Select(pc => pc.Category.Name).Intersect(categories.Split('|')).Any() : true)
        .Include(place => place.PlacePhotos)
        .Include(place => place.PlaceReviews)
          .ThenInclude(review => review.User)
        .Include(place => place.PlaceCategories)
          .ThenInclude(pc => pc.Category);

      if (term.Length != 0) {
        // Search
        places = commonQueriedPlaces
          .Where(place => place.Name
            .ToLower()
            .Contains(term.ToLower())
          )
          .ToList();
      } else if (roomId != -1) {
        // By room id
        var room = context.Rooms
          .Include(r => r.RoomMemberships)
            .ThenInclude(rm => rm.User)
          .FirstOrDefault(r => r.Id == roomId);
        if (room == null) {
          return NotFound();
        }

        var locations = room.RoomMemberships.Select(rm => rm.User.CurrentLocation).ToList().Where(loc => loc != null);

        places = commonQueriedPlaces
          .FromSql(
            @"SELECT * FROM place p WHERE ST_Within(
              ST_MakePoint(p.lon, p.lat),
              ST_Envelope({0})
            )
            ", 
            new PostgisLineString(locations.Select(loc => new Coordinate2D(loc.X, loc.Y)))
          ).ToList();
      } else if (lat != -1.0 && lon != -1.0) {
        // Nearby
        places = commonQueriedPlaces
          .FromSql(@"SELECT *, ST_Distance(
            ST_SetSRID(
              ST_MakePoint(p.lon,p.lat),
              4326
            ), 
            ST_SetSRID(
              ST_MakePoint({0},{1}),
              4326
            )) 
            AS distance 
            FROM place p 
            ORDER BY distance", 
            lon, 
            lat
          )
          .Take(numResults)
          .ToList();
      } else {
        // Paginated "all" results
        places = commonQueriedPlaces
          .Skip((page - 1) * pageSize)
          .Take(pageSize)
          .ToList();
      }

      // Patch categories
      foreach(Place place in places) {
        place.Categories = place.PlaceCategories.Select(pc => pc.Category.Name);

        // Patch user's review to be null
        if (place.PlaceReviews != null) {
          place.PlaceReviews = place.PlaceReviews.Select(review => {
            review.User.PlaceReviews = null;
            return review;
          }).ToList();
        }
      }

      return new ObjectResult(places);
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

    [HttpPut("{id:int}")]
    public IActionResult UpdateJSON(int id, [FromBody] Place item) {
      if (item == null || item.Id != id) {
        return BadRequest();
      }

      switch(UpdatePlace(id, item)) {
        case Result.NotFound:
          return NotFound();
        case Result.DatabaseError:
          return StatusCode((int)HttpStatusCode.InternalServerError);
      }

      return new NoContentResult();
    }

    [HttpPost("{id:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateForm(int id, [FromForm] Place item) {
      if (item == null || item.Id != id) {
        return BadRequest();
      }

      switch(UpdatePlace(id, item)) {
        case Result.NotFound:
          return NotFound();
        case Result.DatabaseError:
          return StatusCode((int)HttpStatusCode.InternalServerError);
      }

      return RedirectToAction("Places", "Admin", new { area = "" });
    }

    private Result UpdatePlace(int id, Place updatedModel) {
      var place = context.Places.FirstOrDefault(p => p.Id == id);
      if (place == null) {
        return Result.NotFound;
      }

      place.Name = updatedModel.Name;
      place.OpeningHours = updatedModel.OpeningHours;
      place.Address = updatedModel.Address;
      place.Phone = updatedModel.Phone;

      context.Places.Update(place);
      try {
        context.SaveChanges();
      } catch (Exception) {
        return Result.DatabaseError;
      }

      return Result.Ok;
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteJSON(int id) {
      switch(DeletePlace(id)) {
        case Result.NotFound:
          return NotFound();
        case Result.DatabaseError:
          return StatusCode((int)HttpStatusCode.InternalServerError);
      }
      return new NoContentResult();
    }

    [HttpPost("{id:int}/delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteForm(int id) {
      switch(DeletePlace(id)) {
        case Result.NotFound:
          return NotFound();
        case Result.DatabaseError:
          return StatusCode((int)HttpStatusCode.InternalServerError);
      }
      return RedirectToAction("Places", "Admin", new { area = "" });
    }

    private Result DeletePlace(int id) {
      var place = context.Places.FirstOrDefault(p => p.Id == id);
      if (place == null) {
        return Result.NotFound;
      }

      context.Places.Remove(place);
      try {
        context.SaveChanges();
      } catch (Exception) {
        return Result.DatabaseError;
      }
      return Result.Ok;
    }
  }
}
