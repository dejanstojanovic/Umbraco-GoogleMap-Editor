using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Reflection;
using System.IO;
using Umbraco.GoogleMaps.Extensions;
using Umbraco.GoogleMaps.Models;


namespace Umbraco.GoogleMaps.DataType
{
    public class GoogleMapEditor : Panel, umbraco.interfaces.IDataEditor
    {

        private umbraco.interfaces.IData _data;

        private HiddenField ctlValue;
        private MapState value;

        public GoogleMapEditor(umbraco.interfaces.IData Data, MapState Configuration)
        {
            _data = Data;
            if (Configuration != null)
            {
                value = Configuration;
            }
            else
            {
                value = new MapState()
                {
                    Width = Constants.DEFAULT_WIDTH,
                    Height = Constants.DEFAULT_HEIGHT,
                    Zoom = Constants.DEFAULT_ZOOM,
                    DrawingTools = Constants.DEFAULT_DRAWINGTOOLS,
                    SingleLocation = false,
                    SearchBox = true,
                    RichtextEditor=false,
                    Center = new Center
                    {
                        Latitude = Constants.DEFAULT_LAT,
                        Longitude = Constants.DEFAULT_LNG
                    }
                };
            }
        }

        public Control Editor { get { return this; } }

        public void Save()
        {
            this._data.Value = ctlValue.Value;
        }

        public bool ShowLabel
        {
            get { return true; }
        }

        public bool TreatAsRichTextEditor
        {
            get { return false; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MapperJavaScript", Common.GetResourceText("jquery.googlemaps.js"), true);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MappInitJavaScript", Common.GetResourceText("map.init.js"),true);
            Page.RegisterStyleSheetBlock("ColorPicker", Common.GetResourceText("jquery.simple-color-picker.css"));
            Page.RegisterStyleSheetBlock("MapContainer", Common.GetResourceText("mapstyle.css"));

            ctlValue = new HiddenField();

            HtmlGenericControl mainDiv = new HtmlGenericControl("div");
            HtmlGenericControl mapDiv = new HtmlGenericControl("div");
            mapDiv.Attributes.Add("class", "google-map");
            mapDiv.Attributes.Add("style", string.Format("width:{0}px;height:{1}px;", value.Width, value.Height));
            mainDiv.Controls.Add(mapDiv);
            mainDiv.Controls.Add(ctlValue);

            this.Controls.Add(mainDiv);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!this.Page.IsPostBack)
            {
                if (this._data.Value != null && !string.IsNullOrEmpty(this._data.Value.ToString()))
                {
                    MapState mapVal = MapState.Deserialize(this._data.Value.ToString());

                    if (mapVal != null)
                    {
                        mapVal.SingleLocation = value.SingleLocation;
                        mapVal.Width = value.Width;
                        mapVal.Height = value.Height;
                        mapVal.DrawingTools = value.DrawingTools;
                        mapVal.SearchBox = value.SearchBox;
                        mapVal.RichtextEditor = value.RichtextEditor;
                        this.ctlValue.Value = mapVal.ToJSON();
                    }

                }
                else
                {
                    this.ctlValue.Value = value.ToJSON();
                }
            }

        }
    }
}
