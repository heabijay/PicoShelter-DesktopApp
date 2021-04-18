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
            public const string HomeUrl = "http://j66301z0.beget.tech";
            public const string ResetPasswordUrl = HomeUrl + "/login";
        }

        public const string HomeUrl = "https://picoshelter-apiserver20210218164711.azurewebsites.net";
        public const string LoginUrl = HomeUrl + "/api/Auth/login";
        public const string LoginByEmailUrl = HomeUrl + "/api/Auth/elogin";
        public const string GetCurrentUser = HomeUrl + "/api/Auth/getInfo";
    }
}
