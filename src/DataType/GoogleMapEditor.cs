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
using Umbraco.Core.Models;
using System.Web;
using umbraco.DataLayer;
using umbraco.BusinessLogic;


namespace Umbraco.GoogleMaps.DataType
{
    [ValidationProperty("IsValid")]
    public class GoogleMapEditor : Panel, umbraco.interfaces.IDataEditor
    {

        private umbraco.interfaces.IData _data;
        private CustomValidator _validator;

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
                    Language = Constants.DEFAULT_LANGUAGE,
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

        public bool IsValid
        {
            get
            {
                var contentSvc = Umbraco.Core.ApplicationContext.Current.Services.ContentService;
                var dataTypeSvc = Umbraco.Core.ApplicationContext.Current.Services.DataTypeService;
                int pageId = 0;
                if (HttpContext.Current.Request["id"] != null)
                {
                    if (contentSvc != null && int.TryParse(HttpContext.Current.Request["id"].ToString(), out pageId))
                    {
                        IContent currentPage = contentSvc.GetById(pageId);
                        if (currentPage != null)
                        {
                            int propertyId = int.Parse(this._data.GetType().GetProperty("PropertyId").GetValue(this._data).ToString());
                            Property property = currentPage.Properties.Where(p => p.Id == propertyId).FirstOrDefault();
                            if (property != null)
                            {
                                int propertyTypeId = SqlHelper.ExecuteScalar<int>("select propertytypeid from cmsPropertyData where id = @id", SqlHelper.CreateParameter("@id", propertyId));
                                PropertyType propertyType = currentPage.ContentType.PropertyTypes.Where(p => p.Id == propertyTypeId).FirstOrDefault();

                                if (propertyType != null && propertyType.Id > 0)
                                {
                                    _validator.ErrorMessage = string.Format("Propety \"{0}\" is mandatory", propertyType.Name);

                                    if (propertyType.Mandatory)
                                    {
                                        MapState _value = MapState.Deserialize(ctlValue.Value);
                                        return (_value != null && _value.Locations != null) ? _value.Locations.Any() : false;
                                    }
                                }
                            }
                        }
                    }
                }
                return true;
            }
        }

        public Control Editor { get { return this; } }

        public void Save()
        {
            this._data.Value = ctlValue.Value;

            if (!IsValid)
            {
                Umbraco.Core.Services.ContentService.Publishing += ContentService_Publishing;
            }
        }

        void ContentService_Publishing(Core.Publishing.IPublishingStrategy sender, Core.Events.PublishEventArgs<IContent> e)
        {
            //Umbraco.Web.UI.Pages.ClientTools client = new Umbraco.Web.UI.Pages.ClientTools((Page)HttpContext.Current.CurrentHandler);
            //client.ShowSpeechBubble(Web.UI.SpeechBubbleIcon.Warning,"Publish",string.Format("{0} ({1}) could not be published because these properties {1} did not pass validation rules.");
            e.Cancel = true;
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
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MappInitJavaScript", Common.GetResourceText("map.init.js"), true);
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

            _validator = new CustomValidator();

            _validator.ServerValidate += validator_ServerValidate;
            this.Controls.Add(_validator);
        }

        void validator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = this.IsValid;
        }

        public static ISqlHelper SqlHelper
        {
            get
            {
                return Application.SqlHelper;
            }
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
                        mapVal.Language = value.Language;
                        mapVal.Width = value.Width;
                        mapVal.Height = value.Height;
                        mapVal.DrawingTools = value.DrawingTools;
                        mapVal.SearchBox = value.SearchBox;
                        mapVal.RichtextEditor = value.RichtextEditor;
                        mapVal.ZoomControl = value.ZoomControl;
                        mapVal.PanControl = value.PanControl;
                        mapVal.StreetViewControl = value.StreetViewControl;
                        mapVal.ScaleControl = value.ScaleControl;

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
