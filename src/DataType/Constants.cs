using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umbraco.GoogleMaps.DataType
{
    public static class Constants
    {
        public const string DEFAULT_LANGUAGE = "en";
        public const int DEFAULT_ZOOM = 15;
        public const int DEFAULT_WIDTH = 800;
        public const int DEFAULT_HEIGHT = 400;
        public const double DEFAULT_LAT = 25.0417;
        public const double DEFAULT_LNG = 55.2194;
        public static string[] DEFAULT_DRAWINGTOOLS = new string[] { "marker", "polyline", "polygon", "circle", "rectangle" };

        public const string DATATYPE_GUID = "BD6402ED-54A5-40CF-8DD0-C35EFE4C510C";
    }
}
