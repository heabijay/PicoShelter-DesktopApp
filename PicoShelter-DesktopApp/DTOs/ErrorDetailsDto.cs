using PicoShelter_DesktopApp.Infrastructure;
using System.Text.Json.Serialization;

namespace PicoShelter_DesktopApp.DTOs
{
    public class ErrorDetailsDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ExceptionType type { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }
}
