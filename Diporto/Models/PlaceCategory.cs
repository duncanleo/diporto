using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Diporto.Models {
  [Table("place_category")]
  public class PlaceCategory {
    [Column("id")]
    public int Id { get; set; }

    [JsonIgnore]
    public Place Place { get; set; }
    
    [Column("place_id")]
    public int PlaceId { get; set; }
    
    public Category Category { get; set; }
    
    [Column("category_id")]
    public int CategoryId { get; set; }
  }
}