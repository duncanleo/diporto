using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Diporto.Models {
  [Table("room")]
  public class Room {
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("short_code")]
    public string ShortCode { get; set; }

    [JsonProperty("members")]
    [NotMapped]
    public IEnumerable<User> Members { get; set; }

    public IEnumerable<RoomMembership> RoomMemberships { get; set; }
  }
}
