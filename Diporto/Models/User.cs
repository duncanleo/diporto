using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Diporto.Models {
  [Table("user")]
  public class User : IdentityUser<int> {
    [Required]
    [Column("name")]
    public string Name { get; set; }

    [Column("profile_image_url")]
    [JsonProperty("profile_image_url")]
    public string ProfileImageURL { get; set; }

    [Required]
    [Column("is_admin")]
    public bool IsAdmin { get; set; }

    [JsonProperty("reviews")]
    public ICollection<PlaceReview> PlaceReviews { get; set; }
  }
}
