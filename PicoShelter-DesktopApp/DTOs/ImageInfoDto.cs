using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicoShelter_DesktopApp.DTOs
{
    public class ImageInfoDto
    {
        public int imageId { get; set; }
        public string imageCode { get; set; }
        public string imageType { get; set; }
        public string title { get; set; }
        public bool isPublic { get; set; }
        public AccountInfoDto user { get; set; }
        public DateTime uploadedTime { get; set; }
        public DateTime? autoDeleteIn { get; set; }
    }
}
