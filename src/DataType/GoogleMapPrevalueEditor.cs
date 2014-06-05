using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using umbraco.editorControls;
using umbraco.DataLayer;
using umbraco.BusinessLogic;
using umbraco.uicontrols;
using Umbraco.GoogleMaps.Models;
using System.Web.UI.HtmlControls;

namespace Umbraco.GoogleMaps.DataType
{
    public class GoogleMapPrevalueEditor : System.Web.UI.WebControls.PlaceHolder, umbraco.interfaces.IDataPrevalue
    {

        private umbraco.cms.businesslogic.datatype.BaseDataType _datatype;
        private TextBox _txtWidth;
        private TextBox _txtHeight;
        private TextBox _txtZoom;
        private CheckBox _cbSingleLocation;
        private CheckBox _cbSearchBox;
        private CheckBox _cbRichText;

        private TextBox _txtCenterLatitude;
        private TextBox _txtCenterLongitude;

        private CheckBox _cbZoomControl;
        private CheckBox _cbPanControl;
        private CheckBox _cbScaleControl;
        private CheckBox _cbStreetViewControl;

        private CheckBoxList _cblDrawingTools;


        public GoogleMapPrevalueEditor(umbraco.cms.businesslogic.datatype.BaseDataType DataType)
        {
            _datatype = DataType;
            setupChildControls();

        }

        private void setupChildControls()
        {

            HyperLink _logoImageLink = new HyperLink();

            _logoImageLink.ImageUrl = "/Umbraco/Api/Resource/GetEmbededPng?filename=settings-logo.png";
            _logoImageLink.NavigateUrl = "http://dejanstojanovic.net/umbraco-googlemap-editor";
            _logoImageLink.Target = "_blank";
            _logoImageLink.ID = "logoImage";
            PropertyPanel propertyPanel = new PropertyPanel();
            propertyPanel.Text = string.Empty;
            propertyPanel.Controls.Add(_logoImageLink);
            Controls.Add(propertyPanel);


            _txtWidth = new TextBox();
            _txtWidth.ID = "txtWidth";
            propertyPanel = new PropertyPanel();
            propertyPanel.Text = "Width & Height";
            propertyPanel.Controls.Add(_txtWidth);
            propertyPanel.Controls.Add(new Literal() { Text = " x " });
            _txtHeight = new TextBox();
            _txtHeight.ID = "txtHeight";
            propertyPanel.Controls.Add(_txtHeight);
            Controls.Add(propertyPanel);


            _txtZoom = new TextBox();
            _txtZoom.ID = "txtZoom";
            propertyPanel = new PropertyPanel();
            propertyPanel.Text = "Zoom";
            propertyPanel.Controls.Add(_txtZoom);
            Controls.Add(propertyPanel);

            _cbSingleLocation = new CheckBox();
            _cbSingleLocation.ID = "cbSingleLocation";
            propertyPanel = new PropertyPanel();
            propertyPanel.Text = "Single location";
            propertyPanel.Controls.Add(_cbSingleLocation);
            Controls.Add(propertyPanel);

            _cbSearchBox = new CheckBox();
            _cbSearchBox.ID = "cbSearchBox";
            propertyPanel = new PropertyPanel();
            propertyPanel.Text = "Search box";
            propertyPanel.Controls.Add(_cbSearchBox);
            Controls.Add(propertyPanel);

            _cbRichText = new CheckBox();
            _cbRichText.ID = "cbRichText";
            propertyPanel = new PropertyPanel();
            propertyPanel.Text = "Rich text editor";
            propertyPanel.Controls.Add(_cbRichText);
            Controls.Add(propertyPanel);

            _txtCenterLatitude = new TextBox();
            _txtCenterLatitude.ID = "txtCenterLatitude";
            propertyPanel = new PropertyPanel();
            propertyPanel.Text = "Center latitude & longitude";
            propertyPanel.Controls.Add(_txtCenterLatitude);
            propertyPanel.Controls.Add(new Literal() { Text = " : " });
            _txtCenterLongitude = new TextBox();
            _txtCenterLongitude.ID = "txtCenterLongitude";
            propertyPanel.Controls.Add(_txtCenterLongitude);

            Controls.Add(propertyPanel);

            HtmlButton _htmlLocationButton = new HtmlButton();
            _htmlLocationButton.InnerText = "use my current location";
            _htmlLocationButton.ID = "btnGetLocation";
            _htmlLocationButton.Attributes.Add("class", "current-location");
            _htmlLocationButton.Attributes.Add("type", "button");
            propertyPanel = new PropertyPanel();
            propertyPanel.Text = "&nbsp;";
            propertyPanel.Controls.Add(_htmlLocationButton);
            Controls.Add(propertyPanel);

            _cblDrawingTools = new CheckBoxList();
            _cblDrawingTools.ID = "cblDRawingTools";

            _cblDrawingTools.Items.Add(new ListItem("Marker", "marker"));
            _cblDrawingTools.Items.Add(new ListItem("Polyline", "polyline"));
            _cblDrawingTools.Items.Add(new ListItem("Polygon", "polygon"));
            _cblDrawingTools.Items.Add(new ListItem("Circle", "circle"));
            _cblDrawingTools.Items.Add(new ListItem("Rectangle", "rectangle"));

            propertyPanel = new PropertyPanel();
            propertyPanel.Text = "Drawing tools";
            propertyPanel.Controls.Add(_cblDrawingTools);
            Controls.Add(propertyPanel);

            _cbZoomControl = new CheckBox();
            _cbZoomControl.ID = "cbZoomControl";
            propertyPanel = new PropertyPanel();
            propertyPanel.Text = "Zoom control";
            propertyPanel.Controls.Add(_cbZoomControl);
            Controls.Add(propertyPanel);

            _cbPanControl = new CheckBox();
            _cbPanControl.ID = "cbPanControl";
            propertyPanel = new PropertyPanel();
            propertyPanel.Text = "Pan control";
            propertyPanel.Controls.Add(_cbPanControl);
            Controls.Add(propertyPanel);

            _cbStreetViewControl = new CheckBox();
            _cbStreetViewControl.ID = "cbStreetViewControl";
            propertyPanel = new PropertyPanel();
            propertyPanel.Text = "StreetView control";
            propertyPanel.Controls.Add(_cbStreetViewControl);
            Controls.Add(propertyPanel);

            _cbScaleControl = new CheckBox();
            _cbScaleControl.ID = "cbScaleControl";
            propertyPanel = new PropertyPanel();
            propertyPanel.Text = "Scale control";
            propertyPanel.Controls.Add(_cbScaleControl);
            Controls.Add(propertyPanel);

        }


        public System.Web.UI.Control Editor
        {
            get
            {
                return this;
            }
        }

        public MapState Configuration
        {
            get
            {
                object conf =
                   SqlHelper.ExecuteScalar<object>("select value from cmsDataTypePreValues where datatypenodeid = @datatypenodeid",
                                           SqlHelper.CreateParameter("@datatypenodeid", _datatype.DataTypeDefinitionId));

                if (conf != null)
                    return MapState.Deserialize(conf.ToString());
                else
                    return new MapState()
                    {
                        Width = Constants.DEFAULT_WIDTH,
                        Height = Constants.DEFAULT_HEIGHT,
                        Zoom = Constants.DEFAULT_ZOOM,
                        DrawingTools = Constants.DEFAULT_DRAWINGTOOLS,
                        SingleLocation = false,
                        SearchBox = true,
                        RichtextEditor = false,
                        ZoomControl = true,
                        PanControl = true,
                        StreetViewControl = true,
                        ScaleControl = true,
                        Center = new Center
                        {
                            Latitude = Constants.DEFAULT_LAT,
                            Longitude = Constants.DEFAULT_LNG
                        }
                    };
            }
        }

        public void Save()
        {
            _datatype.DBType = (umbraco.cms.businesslogic.datatype.DBTypes)Enum.Parse(typeof(umbraco.cms.businesslogic.datatype.DBTypes), DBTypes.Ntext.ToString(), true);

            int zoom = Constants.DEFAULT_ZOOM;
            int width = Constants.DEFAULT_WIDTH;
            int height = Constants.DEFAULT_HEIGHT;
            double lat = Constants.DEFAULT_LAT;
            double lng = Constants.DEFAULT_LNG;

            int.TryParse(this._txtHeight.Text, out height);
            int.TryParse(this._txtWidth.Text, out width);
            int.TryParse(this._txtZoom.Text, out zoom);
            double.TryParse(this._txtCenterLatitude.Text, out lat);
            double.TryParse(this._txtCenterLongitude.Text, out lng);

            List<string> drawingTools = new List<string>();

            foreach (ListItem item in this._cblDrawingTools.Items)
            {
                if (item.Selected)
                {
                    drawingTools.Add(item.Value);
                }
            }


            MapState data = new MapState()
            {
                Height = height,
                Width = width,
                Zoom = zoom,
                SearchBox = this._cbSearchBox.Checked,
                RichtextEditor = this._cbRichText.Checked,
                DrawingTools = drawingTools.ToArray<string>(),
                SingleLocation = this._cbSingleLocation.Checked,
                ZoomControl = this._cbZoomControl.Checked,
                PanControl = this._cbPanControl.Checked,
                StreetViewControl = this._cbStreetViewControl.Checked,
                ScaleControl = this._cbScaleControl.Checked,
                Center = new Center() { Latitude = lat, Longitude = lng }
            };

            SqlHelper.ExecuteNonQuery("delete from cmsDataTypePreValues where datatypenodeid = @dtdefid",
                    SqlHelper.CreateParameter("@dtdefid", _datatype.DataTypeDefinitionId));
            SqlHelper.ExecuteNonQuery("insert into cmsDataTypePreValues (datatypenodeid,[value],sortorder,alias) values (@dtdefid,@value,0,'')",
                    SqlHelper.CreateParameter("@dtdefid", _datatype.DataTypeDefinitionId),
                    SqlHelper.CreateParameter("@value", data.ToJSON()));

        }

        public static ISqlHelper SqlHelper
        {
            get
            {
                return Application.SqlHelper;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MappConfigJavaSCript", Common.GetResourceText("settings.init.js"), true);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {

                if (Configuration != null)
                {
                    _txtWidth.Text = Configuration.Width.ToString();
                    _txtHeight.Text = Configuration.Height.ToString();
                    _txtZoom.Text = Configuration.Zoom.ToString();
                    _txtCenterLatitude.Text = Configuration.Center.Latitude.ToString();
                    _txtCenterLongitude.Text = Configuration.Center.Longitude.ToString();
                    _cbSingleLocation.Checked = Configuration.SingleLocation;
                    _cbSearchBox.Checked = Configuration.SearchBox;
                    _cbRichText.Checked = Configuration.RichtextEditor;
                    _cbZoomControl.Checked = Configuration.ZoomControl;
                    _cbPanControl.Checked = Configuration.PanControl;
                    _cbStreetViewControl.Checked = Configuration.StreetViewControl;
                    _cbScaleControl.Checked = Configuration.ScaleControl;

                    if (Configuration.DrawingTools != null)
                    {
                        foreach (ListItem item in this._cblDrawingTools.Items)
                        {
                            if (Configuration.DrawingTools.Contains(item.Value))
                            {
                                item.Selected = true;
                            }
                            else
                            {
                                item.Selected = false;
                            }
                        }
                    }
                    else
                    {
                        foreach (ListItem item in this._cblDrawingTools.Items)
                        {
                            item.Selected = true;
                        }
                    }

                }
                else
                {
                    _txtWidth.Text = Constants.DEFAULT_WIDTH.ToString();
                    _txtHeight.Text = Constants.DEFAULT_HEIGHT.ToString();
                    _txtZoom.Text = Constants.DEFAULT_ZOOM.ToString();
                    _txtCenterLatitude.Text = Constants.DEFAULT_LAT.ToString();
                    _txtCenterLongitude.Text = Constants.DEFAULT_LNG.ToString();
                    _cbSingleLocation.Checked = false;
                    _cbSearchBox.Checked = true;
                    _cbRichText.Checked = false;
                    _cbZoomControl.Checked = true;
                    _cbPanControl.Checked = true;
                    _cbStreetViewControl.Checked = true;
                    _cbScaleControl.Checked = true;

                    foreach (ListItem item in this._cblDrawingTools.Items)
                    {
                        item.Selected = true;
                    }
                }


            }


        }

    }
}
