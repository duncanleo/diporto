using System.ComponentModel.DataAnnotations;

namespace Diporto.ViewModels {
  public class ChangePasswordViewModel {
    [Required]
    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }
  }
}
