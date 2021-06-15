using System.Collections.Generic;

namespace PicoShelter_DesktopApp.DTOs
{
    public class ModelStateErrorDto
    {
        public string param { get; set; }
        public List<string> errors { get; set; }
    }
}
