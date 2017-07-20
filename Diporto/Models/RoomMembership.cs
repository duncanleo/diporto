using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Diporto.Models {
  [Table("room_membership")]
  public class RoomMembership {
    [Column("id")]
    public int Id { get; set; }

    [JsonIgnore]
    [Required]
    public Room Room { get; set; }
    
    [Column("room_id")]
    public int RoomId { get; set; }
    
    [Required]
    public User User { get; set; }
    
    [Column("user_id")]
    public int UserId { get; set; }
  }
}
