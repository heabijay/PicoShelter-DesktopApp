using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicoShelter_DesktopApp.DTOs
{
    public class LoginByEmailRequestDto
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}
