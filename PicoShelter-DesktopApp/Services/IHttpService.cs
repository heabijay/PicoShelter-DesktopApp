using PicoShelter_DesktopApp.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicoShelter_DesktopApp.Services
{
    public interface IHttpService
    {
        public string AccessToken { get; set; }

        public Task<LoginResponseDto> LoginAsync(string username, string password);

        public Task<LoginResponseDto> LoginByEmailAsync(string email, string password);

        public Task<AccountInfoDto> GetCurrentUserAsync();

        public Task<ImageInfoDto> UploadImageAsync(string title, int deleteInHours, bool isPublic, int quality, Stream filestream);
    }
}
