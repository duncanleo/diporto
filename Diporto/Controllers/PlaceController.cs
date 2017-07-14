using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Diporto.Database;
using Diporto.Models;
using System.Linq;

namespace Diporto.Controllers {
  [Route("api/[controller]")]
  public class PlaceController : Controller {
    private readonly DatabaseContext context;
    public PlaceController(DatabaseContext context) {
      this.context = context;
    }

    [HttpGet]
    public IEnumerable<Place> GetAll() {
      return context.Places
        .Include(place => place.PlacePhotos)
        .Include(place => place.PlaceCategories)
        .Include(place => place.PlaceReviews)
        .ToList();
    }
  }
}
