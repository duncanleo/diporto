using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diporto.Models {
  [Table("user")]
  public class User {
    [Column("id")]
    public int Id { get; set; }
    
    [Required]
    [Column("name")]
    public string Name { get; set; }
    
    [Required]
    [Column("password")]
    public string Password { get; set; }
    
    [Required]
    [Column("is_admin")]
    public bool IsAdmin { get; set; }
  }
}