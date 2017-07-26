using System.Collections.Generic;
using Diporto.Models;

namespace Diporto.ViewModels.Admin {
  public class UsersViewModel : CommonViewModel {
    public List<User> Users { get; set; }
    public int PageIndex { get; set; }
  }
}