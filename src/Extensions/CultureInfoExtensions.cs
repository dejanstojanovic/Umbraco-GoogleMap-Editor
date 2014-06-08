using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umbraco.GoogleMaps.Extensions
{
    public static class CultureInfoExtensions
    {
        /// <summary>
        /// List of all supported languages in GoogleMaps API v3 https://spreadsheets.google.com/spreadsheet/pub?key=0Ah0xU81penP1cDlwZHdzYWkyaERNc0xrWHNvTTA1S1E&gid=1
        /// </summary>
        public static string[] GoogleMapsSupportedLanguageCodes
        {
            get
            {
                return new string[] { "ar", "eu", "bg", "bn", "ca", "cs", "da", "de", "el", "en", "en-AU", "en-GB", "es", "eu", "fa", "fi", "fil", "fr", "gl", "gu", "hi", "hr", "hu", "id", "it", "iw", "ja", "kn", "ko", "lt", "lv", "ml", "mr", "nl", "no", "pl", "pt", "pt-BR", "pt-PT", "ro", "ru", "sk", "sl", "sr", "sv", "tl", "ta", "te", "th", "tr", "uk", "vi", "zh-CN", "zh-TW" };
            }
        }

        /// <summary>
        /// Check whether language is supported by GoogleApi
        /// </summary>
        /// <param name="language">CultureInfo which needs to be checked is it is supported</param>
        /// <returns>Boolean value indicating whether language is supported or not by GoogleMaps API v3</returns>
        public static bool IsGoogleMapsSupported(CultureInfo language)
        {
            return GoogleMapsSupportedLanguageCodes.Where(l =>
                l.Equals(language.TwoLetterISOLanguageName, StringComparison.InvariantCultureIgnoreCase) ||
                l.Equals(language.Name, StringComparison.InvariantCultureIgnoreCase)).Any();
        }


        /// <summary>
        /// Return language code supported by GoogleMap API v3. If culture is not supported "en" will be returned.
        /// </summary>
        /// <param name="language">CultureInfo for which supported language code is returned</param>
        /// <returns>Language name or two letters code for the culture info </returns>
        public static string GetGoogleMapSupportedLanguageCode(CultureInfo language)
        {
            string result = "en";
            if (IsGoogleMapsSupported(language))
            {
                if (GoogleMapsSupportedLanguageCodes.Where(l => l.Equals(language.Name, StringComparison.InvariantCultureIgnoreCase)).Any())
                {
                    return language.Name;
                }
                else if (GoogleMapsSupportedLanguageCodes.Where(l => l.Equals(language.TwoLetterISOLanguageName, StringComparison.InvariantCultureIgnoreCase)).Any())
                {
                    return language.TwoLetterISOLanguageName;
                }
            }
            return result;
        }
    }
}
