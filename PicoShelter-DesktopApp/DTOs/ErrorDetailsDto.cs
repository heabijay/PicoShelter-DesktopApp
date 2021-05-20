using PicoShelter_DesktopApp.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
