using Microsoft.Win32;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PicoShelter_DesktopApp.Services.AppSettings
{
    public static class AppSettingsProvider
    {
        private static AppSettings _appSettings;
        private static RegistryKey _registrySettings = Registry.LocalMachine.OpenSubKey("SOFTWARE", true).CreateSubKey("PicoShelterUploader", true);

        public static AppSettings Provide()
        {
            if (_appSettings != null)
                return _appSettings;

            _appSettings = new AppSettings();
            ReadFromRegistry(_appSettings);
            _appSettings.PropertyChanged += UpdateToRegistry;

            return _appSettings;
        }

        public static async Task<AppSettings> ProvideAsync()
        {
            return await Task.Run(() => Provide());
        }

        private static void ReadFromRegistry(AppSettings appSettings)
        {
            var values = _registrySettings.GetValueNames();
            foreach (var val in values)
            {
                try
                {
                    if (val == nameof(appSettings.AccessToken))
                    {
                        // appSettings.AccessToken
                        var enToken = _registrySettings.GetValue(nameof(appSettings.AccessToken))?.ToString();
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
                            if (prop.PropertyType.IsEnum)
                            {
                                if (Enum.TryParse(prop.PropertyType, _registrySettings.GetValue(val)?.ToString(), out object result))
                                    prop.SetValue(appSettings, result);
                            }
                            else
                            {
                                prop.SetValue(appSettings, Convert.ChangeType(_registrySettings.GetValue(val), prop.PropertyType));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _registrySettings.DeleteValue(val);
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

                _registrySettings.SetValue(e.PropertyName, data);
            }
            else
            {
                var val = settings.GetType().GetProperty(e.PropertyName)?.GetValue(settings, null);
                if (val != null)
                {
                    _registrySettings.SetValue(e.PropertyName, val.ToString());
                }
                else
                {
                    _registrySettings.DeleteValue(e.PropertyName);
                }
            }
        }
    }
}
