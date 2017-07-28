using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Diporto.Models {
  [Table("place")]
  public class Place {
    [Column("id")]
    public int Id { get; set; }
    
    [Required]
    [Column("name")]
    public string Name { get; set; }
    
    [Required]
    [Column("lat")]
    public float Lat { get; set; }
    
    [Required]
    [Column("lon")]
    public float Lon { get; set; }
    
    [Required(AllowEmptyStrings = true)]
    [Column("phone")]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string Phone { get; set; }
    
    [Required]
    [Column("address")]
    public string Address { get; set; }
    
    [Required(AllowEmptyStrings = true)]
    [Column("opening_hours")]
    [JsonProperty("opening_hours")]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string OpeningHours { get; set; }

    [NotMapped]
    public IEnumerable<string> Categories { get; set; }
    
    [JsonIgnore]
    public ICollection<PlaceCategory> PlaceCategories { get; set; }

    [JsonProperty("photos")]    
    public ICollection<PlacePhoto> PlacePhotos { get; set; }

    [JsonProperty("reviews")]
    public ICollection<PlaceReview> PlaceReviews { get; set; }

    [JsonIgnore]
    public ICollection<UserPlaceBookmark> UserBookmarks { get; set; }
  }
}
