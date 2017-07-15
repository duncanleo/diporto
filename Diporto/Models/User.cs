using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Diporto.Models {
  [Table("user")]
  public class User : IdentityUser<int> {
    [Required]
    [Column("is_admin")]
    public bool IsAdmin { get; set; }
  }
}