using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Diporto.Models {
  [Table("place_review")]
  public class PlaceReview {
    [Column("id")]
    public int Id { get; set; }

    [Column("author_name")]
    [Required]
    public string AuthorName { get; set; }

    [Column("author_profile_image_url")]
    public string AuthorProfileImageURL { get; set; }

    [Column("rating")]
    [Required]
    public float Rating { get; set; }

    [Column("time")]
    [Required]
    public DateTime Time { get; set; }

    [Column("text")]
    [Required]
    public string Text { get; set; }

    [Required]
    [JsonIgnore]
    public Place Place { get; set; }
  }
}
