using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Diporto.Models {
  [Table("room")]
  public class Room {
    public int Id { get; set; }

    [Required]
    [Column("name")]
    public string Name { get; set; }

    [Column("short_code")]
    public string ShortCode { get; set; }

    public User Owner { get; set; }
    
    [Column("owner_id")]
    public int OwnerId { get; set; }

    [JsonProperty("members")]
    [NotMapped]
    public IEnumerable<User> Members { get; set; }

    public IEnumerable<RoomMembership> RoomMemberships { get; set; }
  }
}
