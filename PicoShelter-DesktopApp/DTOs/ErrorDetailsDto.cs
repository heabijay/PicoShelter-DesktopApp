using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicoShelter_DesktopApp.DTOs
{
    public class ErrorDetailsDto
    {
        public string type { get; private set; }
        public string message { get; private set; }
        public object data { get; private set; }
    }
}
