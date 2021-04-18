using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicoShelter_DesktopApp.DTOs
{
    public class AccountInfoDto
    {
        public int id { get; set; }
        public string username { get; set; }
        public ProfileNameDto profile { get; set; }
        public string role { get; set; }
    }
}
