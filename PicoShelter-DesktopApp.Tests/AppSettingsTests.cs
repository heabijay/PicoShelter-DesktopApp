using Microsoft.VisualStudio.TestTools.UnitTesting;
using PicoShelter_DesktopApp.Services.AppSettings;
using PicoShelter_DesktopApp.Services.AppSettings.Enums;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PicoShelter_DesktopApp.Tests
{
    [TestClass]
    public class AppSettingsTests
    {
        AppSettings settings;
        Random random;

        [TestInitialize]
        public async Task Initialize()
        {
            settings = await AppSettingsProvider.ProvideAsync();
            random = new Random();
        }

        [TestMethod]
        public void SettingChange()
        {
            Debug.WriteLine("Before: " + (settings.AccessToken ?? "[null]"));
            settings.AccessToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            Debug.WriteLine("After: " + (settings.AccessToken ?? "[null]"));

            Debug.WriteLine("Before: " + (settings.DefaultTitle.ToString() ?? "[null]"));
            settings.DefaultTitle = (DefaultTitleOptions)(random.Next(Enum.GetNames(typeof(DefaultTitleOptions)).Length));
            Debug.WriteLine("After: " + (settings.DefaultTitle.ToString() ?? "[null]"));
        }

        [TestMethod]
        public void SettingDelete()
        {
            Debug.WriteLine("Before: " + (settings.AccessToken ?? "[null]"));
            settings.AccessToken = null;
            Debug.WriteLine("After: " + (settings.AccessToken ?? "[null]"));
        }
    }
}
