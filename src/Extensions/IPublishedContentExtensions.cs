using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.Mvc;
using System.Web.Http;
using Umbraco.Core;
using Umbraco.Core.Services;
using Umbraco.Core.Models;
using Umbraco.Web;
using System.Text.RegularExpressions;
using Umbraco.GoogleMaps;
using Umbraco.GoogleMaps.Models;

namespace Umbraco.GoogleMaps.Extensions
{
   public static class IPublishedContentExtensions
    {

        /// <summary>
        /// Returns value from property casted to string type without rendering Macros in RTE property. If property does not exist returns String.Empty.
        /// </summary>
        /// <param name="page">Umbraco cached content</param>
        /// <param name="propertyName">Name of the property which value is casted to String type</param>
        /// <returns>Property value casted to String type</returns>
        public static string GetPropertyUnrenderedValueAsString(this IPublishedContent page, string propertyName)
        {
            string result = string.Empty;
            IPublishedContentProperty prop = page.Properties.Where(p => p.PropertyTypeAlias == propertyName).FirstOrDefault();
            if (prop != null && prop.Value != null)
            {
                result = prop.Value.ToString();
            }
            return result;
        }

        /// <summary>
        /// Returns value from property casted to string type with rendering Macros in RTE property. If property does not exist returns String.Empty.
        /// </summary>
        /// <param name="page">Umbraco cached content</param>
        /// <param name="propertyName">Name of the property which value is casted to String type</param>
        /// <returns>Property value casted to String type</returns>
        public static string GetPropertyValueAsString(this IPublishedContent page, string propertyName)
        {
            string result = string.Empty;
            if (page.HasProperty(propertyName) && page.GetPropertyValue(propertyName) != null)
            {
                result = page.GetPropertyValue(propertyName).ToString();
            }
            return result;
        }

        /// <summary>
        /// Returns value from property casted to int type. If property does not exist returns 0.
        /// </summary>
        /// <param name="page">Umbraco cached content</param>
        /// <param name="propertyName">Name of the property which value is casted to int type</param>
        /// <returns>Property value casted to int type</returns>
        public static int GetPropertyValueAsInt(this IPublishedContent page, string propertyName)
        {
            int result = 0;
            int.TryParse(page.GetPropertyValueAsString(propertyName), out result);
            return result;
        }

        /// <summary>
        /// Returns value from property casted to Boolean type. If property does not exist returns false.
        /// </summary>
        /// <param name="page">Umbraco cached content</param>
        /// <param name="propertyName">Name of the property which value is casted to Boolean type</param>
        /// <returns>Property value casted to Boolean type</returns>
        public static bool GetPropertyValueAsBool(this IPublishedContent page, string propertyName)
        {
            bool result = false;
            if (!bool.TryParse(page.GetPropertyValueAsString(propertyName), out result))
            {
                if (page.GetPropertyValueAsString(propertyName) == 1.ToString())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return result;
        }

        /// <summary>
        /// Returns value from property casted to DateTime type. If property does not exist returns DateTime.MinValue.
        /// </summary>
        /// <param name="page">Umbraco cached content</param>
        /// <param name="propertyName">Name of the property which value is casted to DateTime type</param>
        /// <returns>Property value casted to DateTime type</returns>
        public static DateTime GetPropertyValueAsDateTime(this IPublishedContent page, string propertyName)
        {
            DateTime result = DateTime.MinValue;
            if (page.HasProperty(propertyName) && page.GetPropertyValue(propertyName) != null)
            {
                DateTime.TryParseExact(page.Properties.Where(p => p.PropertyTypeAlias == propertyName).FirstOrDefault().Value.ToString()
                    , "yyyy-MM-ddTHH:mm:ss", null, System.Globalization.DateTimeStyles.None, out result);
            }
            return result;
        }

        public static MapState GetPropertyValueAsMapState(this IPublishedContent content, string property)
        {
            return MapState.Deserialize(content.GetPropertyValueAsString(property));
        }

    }

}
