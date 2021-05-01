using PicoShelter_DesktopApp.Services.AppSettings.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace PicoShelter_DesktopApp.Converters
{
    public class EnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable array && !(array is string))
                return array.Cast<object>().Select(t => ConvertSingle(t, targetType, parameter, culture)).ToArray();

            return ConvertSingle(value, targetType, parameter, culture);
        }

        private object ConvertSingle(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is LocaleOptions lang)
                return new CultureInfo(lang.ToString().Replace("_", "-")).EnglishName;

            return ApplicationViewModel.LanguageResolve($"Enum.{value.GetType().Name}.{value}");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable array && !(array is string))
                return array.Cast<object>().Select(t => ConvertBackSingle(t, targetType.GetElementType(), parameter, culture)).ToArray();

            return ConvertBackSingle(value, targetType, parameter, culture);
        }

        private object ConvertBackSingle(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(LocaleOptions))
                return Enum.Parse<LocaleOptions>(CultureInfo.GetCultures(CultureTypes.AllCultures).FirstOrDefault(t => t.EnglishName == value.ToString()).Name.Replace("-", "_"));

            string startsWith = $"Enum.{targetType.Name}.";

            var dictionaries = App.Current.Resources.MergedDictionaries
                .Where(d => d.Source != null && d.Source.OriginalString.StartsWith("Resources/Locales/lang."))
                .Select(
                    t => t.Keys
                        .Cast<string>()
                        .Select(x => new KeyValuePair<string, string>(x, t[x].ToString()))
                        .ToDictionary(t => t.Key, t => t.Value)
                    );

            var targetDictionary = dictionaries
                .Select(
                    t => t.Where(
                        t => t.Key.StartsWith(startsWith)
                        )
                        .ToDictionary(
                            t => t.Key.Substring(startsWith.Length),
                            t => t.Value
                            )
                        )
                .LastOrDefault(
                    t => t.ContainsValue(value.ToString())
                    );

            var res = targetDictionary?.FirstOrDefault(t => t.Value == value.ToString()).Key;

            if (res == null)
            {
                if (value.ToString().StartsWith(startsWith))
                    res = value.ToString().Substring(startsWith.Length);
                else
                    return null;
            }

            return Enum.Parse(targetType, res);
        }
    }
}
