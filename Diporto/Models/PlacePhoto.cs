using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diporto.Models {
  [Table("place_photo")]
  public class PlacePhoto {
    [Column("id")]
    public int Id { get; set; }
    
    [Required]
    [Column("url")]
    public string URL { get; set; }
    
    [Required]
    public Place Place { get; set; }
  }
}