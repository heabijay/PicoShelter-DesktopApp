using Microsoft.VisualStudio.TestTools.UnitTesting;
using PicoShelter_DesktopApp.Services.AppSettings;
using PicoShelter_DesktopApp.Services.AppSettings.Enums;
using System;
using System.Diagnostics;
using System.Reflection;

namespace PicoShelter_DesktopApp.Tests
{
    [TestClass]
    public class AppSettingsTests
    {
        private Random _random = new();
        private AppSettings _settingsRef
        {
            get => (AppSettings)typeof(AppSettingsProvider).GetField("_appSettings", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
            set => typeof(AppSettingsProvider).GetField("_appSettings", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, value);
        }

        public AppSettings SettingsRebuild()
        {
            _settingsRef = null;
            return AppSettingsProvider.Provide();
        }


        [TestMethod]
        public void TokenChange()
        {
            var settings = SettingsRebuild();
            var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            Debug.WriteLine("Before: " + (settings.AccessToken ?? "[null]"));
            settings.AccessToken = token;

            settings = SettingsRebuild();
            Debug.WriteLine("After: " + (settings.AccessToken ?? "[null]"));
            Assert.AreEqual(token, settings.AccessToken);
        }

        [TestMethod]
        public void EnumChange()
        {
            var settings = SettingsRebuild();
            var value = (DefaultTitleOptions)(_random.Next(Enum.GetNames(typeof(DefaultTitleOptions)).Length));
            Debug.WriteLine("Before: " + (settings.DefaultTitle.ToString() ?? "[null]"));
            settings.DefaultTitle = value;

            settings = SettingsRebuild();
            Debug.WriteLine("After: " + (settings.DefaultTitle.ToString() ?? "[null]"));
            Assert.AreEqual(value, settings.DefaultTitle);
        }


        [TestMethod]
        public void SettingDelete()
        {
            var settings = SettingsRebuild();
            Debug.WriteLine("Before: " + (settings.AccessToken ?? "[null]"));
            _settingsRef.AccessToken = null;

            settings = SettingsRebuild();
            Debug.WriteLine("After: " + (settings.AccessToken ?? "[null]"));
            Assert.AreEqual(null, settings.AccessToken);
        }
    }
}
