using System.Collections.Generic;

namespace PicoShelter_DesktopApp.DTOs
{
    public class HttpResponseDto<T> where T : class
    {
        public bool success { get; set; }
        public T data { get; set; }
        public ErrorDetailsDto error { get; set; }
        public List<ModelStateErrorDto> errors { get; set; }
    }
}
