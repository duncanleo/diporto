using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Diporto.Models {
  [Table("user_place_bookmark")]
  public class UserPlaceBookmark {
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [JsonIgnore]
    public Place Place { get; set; }

    [Column("place_id")]
    public int PlaceId { get; set; }

    public User User { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }
  }
}
