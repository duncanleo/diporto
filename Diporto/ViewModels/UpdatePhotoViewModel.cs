using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Diporto.ViewModels {
  public class UpdatePhotoViewModel {
    [Required]
    [FileExtensions(Extensions = "jpg,jpeg,png,gif")]
    public IFormFile File { get; set; }
  }
}
