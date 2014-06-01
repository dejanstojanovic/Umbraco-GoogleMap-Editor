using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Umbraco.GoogleMaps
{
    public static class Common
    {
        public static string GetResourceText(string filename)
        {
            string result = string.Empty;
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = string.Concat("Umbraco.GoogleMaps.Resources.", filename);

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                if (stream != null)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        result = reader.ReadToEnd();
                    }
                }
            return result;

        }

        public static Bitmap GetResourceBitmap(string filename)
        {
            Bitmap result = null;
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = string.Concat("Umbraco.GoogleMaps.Resources.", filename);

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                if (stream != null)
                {
                    result = new Bitmap(stream); 
                }
            return result;

        }

        public static Stream GetResourceStream(string filename)
        {
            Stream result = null;
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = string.Concat("Umbraco.GoogleMaps.Resources.", filename);

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                if (stream != null)
                {
                    result = stream;
                }
            return result;

        }
    }
}
