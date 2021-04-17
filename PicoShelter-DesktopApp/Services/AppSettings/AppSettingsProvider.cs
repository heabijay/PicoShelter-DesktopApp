using Microsoft.Win32;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PicoShelter_DesktopApp.Services.AppSettings
{
    static class AppSettingsProvider
    {
        private static AppSettings appSettings;
        private static RegistryKey registrySettings = Registry.LocalMachine.OpenSubKey("SOFTWARE").CreateSubKey("PicoShelterUploader");

        public static AppSettings Provide()
        {
            if (appSettings != null)
                return appSettings;

            appSettings = new AppSettings();


            #region Settings Read

            // appSettings.AccessToken
            var enToken = registrySettings.GetValue(appSettings.AccessToken.GetType().ToString())?.ToString();
            if (!string.IsNullOrWhiteSpace(enToken))
            {
                var enData = Convert.FromBase64String(enToken);
                var decData = ProtectedData.Unprotect(enData, null, DataProtectionScope.LocalMachine);
                appSettings.AccessToken = Encoding.Unicode.GetString(decData);
            }

            #endregion

            appSettings.PropertyChanged += AppSettings_PropertyChanged;

            return appSettings;
        }

        public static async Task<AppSettings> ProvideAsync()
        {
            return await Task.Run(() => Provide());
        }


        private static void AppSettings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var settings = sender as AppSettings;
            if (settings == null)
                throw new ArgumentNullException();

            if (e.PropertyName == settings.AccessToken.GetType().Name)
            {

            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}
