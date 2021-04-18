using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicoShelter_DesktopApp.DTOs
{
    public class ModelStateErrorDto
    {
        public string param { get; set; }
        public List<string> errors { get; set; }
    }
}
