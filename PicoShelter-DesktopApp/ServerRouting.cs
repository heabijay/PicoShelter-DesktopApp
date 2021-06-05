using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicoShelter_DesktopApp
{
    public static class ServerRouting
    {
        public static class WebAppRouting
        {
            public const string HomeUrl = "https://www.picoshelter.tk";
            //public const string HomeUrl = "https://localhost:4200";
            public const string ResetPasswordUrl = HomeUrl + "/login";
            public const string ImageUrlEndpoint = HomeUrl + "/i/";
        }
        public const string HomeUrl = "https://picoshelter-apiserver.azurewebsites.net";
        //public const string HomeUrl = "https://localhost:5000";
        public const string LoginUrl = HomeUrl + "/api/Auth/login";
        public const string LoginByEmailUrl = HomeUrl + "/api/Auth/elogin";
        public const string GetCurrentUser = HomeUrl + "/api/Auth/getInfo";
        public const string UploadUrl = HomeUrl + "/api/Upload";
        public const string ImageUrlEndpoint = HomeUrl + "/i/";
    }
}
