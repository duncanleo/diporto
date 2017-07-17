using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Diporto.ViewModels {
  public class UpdateLocationViewModel {
    [Required]
    [JsonProperty("lat")]
    public float Lat { get; set; }

    [Required]
    [JsonProperty("lon")]
    public float Lon { get; set; }
  }
}
