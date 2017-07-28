using System.Collections.Generic;
using Diporto.Models;

namespace Diporto.ViewModels.Admin {
  public class PlacesFieldsViewModel : CommonViewModel {
    public enum FieldsMode { Create, Edit };
    public Place Place { get; set; }
    public FieldsMode Mode { get; set; }
  }
}