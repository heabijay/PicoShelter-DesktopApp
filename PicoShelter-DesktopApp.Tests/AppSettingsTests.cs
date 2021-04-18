using Microsoft.VisualStudio.TestTools.UnitTesting;
using PicoShelter_DesktopApp.Services.AppSettings;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PicoShelter_DesktopApp.Tests
{
    [TestClass]
    public class AppSettingsTests
    {
        AppSettings settings;

        [TestInitialize]
        public async Task Initialize()
        {
            settings = await AppSettingsProvider.ProvideAsync();
        }

        [TestMethod]
        public void SettingChange()
        {
            Debug.WriteLine("Before: " + (settings.AccessToken ?? "[null]"));
            settings.AccessToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            Debug.WriteLine("After: " + (settings.AccessToken ?? "[null]"));
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
