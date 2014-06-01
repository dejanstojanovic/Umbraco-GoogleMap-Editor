using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Web.WebApi;
using Umbraco.GoogleMaps.ApiFilters;
using System.Web.Http;
using System.IO;
using System.Web;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Drawing.Imaging;
using System.Net;

namespace Umbraco.GoogleMaps.Controllers
{
    public class ResourceController:UmbracoApiController
    {
        [HttpGet]
        [ReturnHtmlOnly]
        public string GetEditForm(string filename)
        {
            return Common.GetResourceText(filename).Replace(System.Environment.NewLine, string.Empty).Replace("\"",string.Empty);
        }
        [HttpGet]
        [ReturnHtmlOnly]
        public string GetMarkerEditForm(string filename)
        {
            string result = Common.GetResourceText(filename).Replace(System.Environment.NewLine, string.Empty).Replace("\"", string.Empty); 
            StringBuilder sbFiles = new StringBuilder();

            string folderPath = "/Umbraco/Images/MapPins/";
            if (Directory.Exists(HttpContext.Current.Server.MapPath(folderPath)))
            {
                foreach (string file in Directory.GetFiles(HttpContext.Current.Server.MapPath(folderPath)))
                {
                    sbFiles.Append(string.Format("<option value='{0}'>{1}</option>", string.Concat(folderPath, Path.GetFileName(file)), Path.GetFileName(file)));
                }
            }


            return string.Format(result, sbFiles.ToString());

            
        }


        public HttpResponseMessage GetEmbededGif(string filename)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            MemoryStream ms = new MemoryStream();
            Common.GetResourceBitmap(filename).Save(ms, ImageFormat.Gif);
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new ByteArrayContent(ms.ToArray());
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/gif");
            return result;
        }

        public HttpResponseMessage GetEmbededPng(string filename)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            MemoryStream ms = new MemoryStream();
            Common.GetResourceBitmap(filename).Save(ms, ImageFormat.Png);
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new ByteArrayContent(ms.ToArray());
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            return result;
        }

    }
}
