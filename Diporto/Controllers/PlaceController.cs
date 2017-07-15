using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Diporto.Database;
using Diporto.Models;
using System.Linq;

namespace Diporto.Controllers {
  [Authorize]
  [Route("api/[controller]")]
  public class PlaceController : Controller {
    private readonly DatabaseContext context;
    public PlaceController(DatabaseContext context) {
      this.context = context;
    }

    [HttpGet("all")]
    public IEnumerable<Place> GetAll() {
      return context.Places
        .Include(place => place.PlacePhotos)
        .Include(place => place.PlaceCategories)
        .Include(place => place.PlaceReviews)
        .ToList();
    }

    [HttpGet("nearby")]
    public IEnumerable<Place> GetNearby(double lat, double lon, string categoryFilters = "", int numResults = 5) {
      var places = context.Places
        .FromSql("SELECT *, ST_Distance(ST_SetSRID(ST_MakePoint(p.lat,p.lon),4326), ST_SetSRID(ST_MakePoint({0},{1}),4326)) AS distance FROM place p ORDER BY distance", lat, lon)
        .Where(place => categoryFilters.Length > 0 ? place.PlaceCategories.Select(pc => pc.Category.Name).Intersect(categoryFilters.Split('|')).Any() : true)
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
  }
}
