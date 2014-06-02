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
        public HttpResponseMessage GetEditForm(string filename)
        {
            return new HttpResponseMessage()
            {
                Content = new StringContent(Common.GetResourceText(filename), System.Text.Encoding.UTF8, "application/html")
            };
        }
        [HttpGet]
        [ReturnHtmlOnly]
        public HttpResponseMessage GetMarkerEditForm(string filename)
        {
            string result = Common.GetResourceText(filename);
            StringBuilder sbFiles = new StringBuilder();

            string folderPath = "/Umbraco/Images/MapPins/";
            if (Directory.Exists(HttpContext.Current.Server.MapPath(folderPath)))
            {
                foreach (string file in Directory.GetFiles(HttpContext.Current.Server.MapPath(folderPath)))
                {
                    sbFiles.Append(string.Format("<option value='{0}'>{1}</option>", string.Concat(folderPath, Path.GetFileName(file)), Path.GetFileName(file)));
                }
            }

            return new HttpResponseMessage()
            {
                Content = new StringContent(string.Format(result, sbFiles.ToString()), System.Text.Encoding.UTF8, "application/html")
            };
        }

        [ReturnJavaScriptOnly]
        public HttpResponseMessage GetEmbdededJavaScript(string filename)
        {
            string result = Common.GetResourceText(filename);

            return new HttpResponseMessage()
            {
                Content = new StringContent(result, System.Text.Encoding.UTF8, "application/javascript")
            };

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
