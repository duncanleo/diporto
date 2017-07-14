using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diporto.Models {
  [Table("category")]
  public class Category {
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("name")]
    public string Name { get; set; }
    
    public ICollection<PlaceCategory> PlaceCategories { get; set; }
  }
}
