using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicoShelter_DesktopApp.DTOs
{
    public class ErrorDetailsDto
    {
        public string type { get; init; }
        public string message { get; init; }
        public object data { get; init; }
    }
}
