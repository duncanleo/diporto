using System.ComponentModel.DataAnnotations;

namespace Diporto.ViewModels {
  public class RegisterViewModel {
    [Required]
    public string UserName { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }

    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
  }
}
