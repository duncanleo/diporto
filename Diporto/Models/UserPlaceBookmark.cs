using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Diporto.Models {
  [Table("user_place_bookmark")]
  public class UserPlaceBookmark {
    [Column("id")]
    public int Id { get; set; }

    [JsonProperty("place")]
    public Place Place { get; set; }

    [Column("place_id")]
    [JsonProperty("place_id")]
    [Required]
    public int PlaceId { get; set; }

    [JsonIgnore]
    public User User { get; set; }

    [Column("user_id")]
    [JsonProperty("user_id")]
    public int UserId { get; set; }
  }
}
