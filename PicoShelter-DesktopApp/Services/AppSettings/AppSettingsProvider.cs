using Microsoft.Win32;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;

namespace PicoShelter_DesktopApp.Services.AppSettings
{
    public static class AppSettingsProvider
    {
        private static AppSettings appSettings;
        private static RegistryKey registrySettings = Registry.LocalMachine.OpenSubKey("SOFTWARE", true).CreateSubKey("PicoShelterUploader", true);

        public static AppSettings Provide()
        {
            if (appSettings != null)
                return appSettings;

            appSettings = new AppSettings();
            ReadFromRegistry(appSettings);
            appSettings.PropertyChanged += UpdateToRegistry;

            return appSettings;
        }

        public static async Task<AppSettings> ProvideAsync()
        {
            return await Task.Run(() => Provide());
        }

        private static void ReadFromRegistry(AppSettings appSettings)
        {
            var values = registrySettings.GetValueNames();
            foreach (var val in values)
            {
                if (val == nameof(appSettings.AccessToken))
                {
                    // appSettings.AccessToken
                    var enToken = registrySettings.GetValue(nameof(appSettings.AccessToken))?.ToString();
                    if (!string.IsNullOrWhiteSpace(enToken))
                    {
                        var enData = Convert.FromBase64String(enToken);
                        var decData = ProtectedData.Unprotect(enData, null, DataProtectionScope.LocalMachine);
                        appSettings.AccessToken = Encoding.Unicode.GetString(decData);
                    }
                }
                else
                {
                    var prop = appSettings.GetType().GetProperty(val);
                    if (prop != null)
                    {
                        prop.SetValue(appSettings, Convert.ChangeType(registrySettings.GetValue(val), prop.PropertyType));
                    }
                }
            }
        }

        private static void UpdateToRegistry(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var settings = sender as AppSettings;
            if (settings == null || e?.PropertyName == null)
                throw new ArgumentNullException();

            if (e.PropertyName == nameof(settings.AccessToken) && settings.AccessToken != null)
            {
                var bytes = Encoding.Unicode.GetBytes(settings.AccessToken);
                var enData = ProtectedData.Protect(bytes, null, DataProtectionScope.LocalMachine);
                var data = Convert.ToBase64String(enData);

                registrySettings.SetValue(e.PropertyName, data);
            }
            else
            {
                var val = settings.GetType().GetProperty(e.PropertyName)?.GetValue(settings, null);
                if (val != null)
                {
                    registrySettings.SetValue(e.PropertyName, val.ToString());
                }
                else
                {
                    registrySettings.DeleteValue(e.PropertyName);
                }
            }
        }
    }
}
