using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Diporto.Models {
  [Table("place_photo")]
  public class PlacePhoto {
    [Column("id")]
    public int Id { get; set; }

    [Column("file_name")]
    [JsonIgnore]
    public string FileName { get; set; }
    
    [Column("google_place_id")]
    [JsonIgnore]
    public string GooglePlacesId { get; set; }

    [Column("is_google_places_image")]
    [JsonIgnore]
    public bool IsGooglePlacesImage { get; set; }
    
    [Required]
    [JsonIgnore]
    public Place Place { get; set; }
  }
}
