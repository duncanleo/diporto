using System.ComponentModel.DataAnnotations;

namespace Diporto.ViewModels {
  public class RequestTokenViewModel {
    public string UserName { get; set; }

    public string RefreshToken { get; set; }

    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    public string GrantType { get; set; }
  }
}
