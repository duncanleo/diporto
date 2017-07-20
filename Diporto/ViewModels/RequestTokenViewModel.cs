using System.ComponentModel.DataAnnotations;

namespace Diporto.ViewModels {
  public class RequestTokenViewModel {
    [Required]
    public string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
  }
}