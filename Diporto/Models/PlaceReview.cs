using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Diporto.Models {
  [Table("place_review")]
  public class PlaceReview {
    [Column("id")]
    public int Id { get; set; }

    [Column("rating")]
    [Required]
    public float Rating { get; set; }

    [Column("time")]
    [Required]
    public DateTime Time { get; set; }

    [Column("text")]
    [Required]
    public string Text { get; set; }

    [JsonIgnore]
    public Place Place { get; set; }

    [JsonProperty("user")]
    public User User { get; set; }

    [Column("user_id")]
    [JsonIgnore]
    public int UserId { get; set; }
  }
}
