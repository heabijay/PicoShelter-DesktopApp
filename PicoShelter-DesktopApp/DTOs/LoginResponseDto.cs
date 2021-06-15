using System;

namespace PicoShelter_DesktopApp.DTOs
{
    public class LoginResponseDto
    {
        public string access_token { get; set; }
        public DateTime expires { get; set; }
        public AccountInfoDto user { get; set; }
    }
}
