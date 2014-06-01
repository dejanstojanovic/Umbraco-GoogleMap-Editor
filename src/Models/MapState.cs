using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umbraco.GoogleMaps.Models
{
    public class Center
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class Coordinate
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class Location
    {
        public List<Coordinate> Coordinates { get; set; }
        public Double Radius { get; set; }
        public string LocationType { get; set; }
        public object Icon { get; set; }
        public string Message { get; set; }
        public string BorderColor { get; set; }
        public int BorderWeight { get; set; }
        public string FillColor { get; set; }
        public object Tag { get; set; }
    }

    public class MapState
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Zoom { get; set; }
        public bool SingleLocation { get; set; }
        public Center Center { get; set; }
        public string[] DrawingTools { get; set; }
        public List<Location> Locations { get; set; }
        public bool SearchBox { get; set; }
        public bool RichtextEditor { get; set; }
        public string ToJSON()
        {
            string result = null;
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            result = serializer.Serialize(this);
            return result;
        }

        public static MapState Deserialize(string mapJSON)
        {
            MapState result = null;
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            try
            {
                result = serializer.Deserialize<MapState>(mapJSON);
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
        }
    }

}
