using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umbraco.GoogleMaps.Models
{
    public class MapView
    {
        public int Width
        {
            get;
            set;
        }
        public int Height
        {
            get;
            set;
        }

        public string MapState
        {
            get;
            set;
        }

        public MapView(string mapState)
        {
            MapState = mapState;
        }
        public MapView(MapState mapState)
        {
            MapState = mapState.ToJSON();
        }

        public MapView(string mapState,int width, int height)
        {
            MapState = mapState;
            Width = width;
            Height = height;
        }

        public MapView(MapState mapState, int width, int height)
        {
            MapState = mapState.ToJSON();
            Width = width;
            Height = height;
        }
    }
}
