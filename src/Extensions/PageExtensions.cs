using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umbraco.GoogleMaps.Extensions
{
    public static class PageExtensions
    {

        public static bool RegisterStyleSheetInclude(this System.Web.UI.Page page, string key, string styleSheetFilePath)
        {
            styleSheetFilePath = page.ResolveClientUrl(styleSheetFilePath);
            if (page != null)
            {
                System.Web.UI.HtmlControls.HtmlHead head = (System.Web.UI.HtmlControls.HtmlHead)page.Header;
                bool isExistStyleSheet = false;
                foreach (System.Web.UI.Control item in head.Controls)
                {
                    if (item is System.Web.UI.HtmlControls.HtmlLink && item.ID == key)
                    {
                        isExistStyleSheet = true;
                    }
                }
                if (!isExistStyleSheet)
                {
                    System.Web.UI.HtmlControls.HtmlLink link = new System.Web.UI.HtmlControls.HtmlLink();
                    link.Attributes.Add("href", page.ResolveClientUrl(styleSheetFilePath));
                    link.Attributes.Add("type", "text/css");
                    link.Attributes.Add("rel", "stylesheet");
                    head.Controls.Add(link);
                }
                return true;
            }
            return false;
        }

        public static bool RegisterStyleSheetBlock(this System.Web.UI.Page page, string key, string style)
        {

            if (page != null)
            {
                System.Web.UI.HtmlControls.HtmlHead head = (System.Web.UI.HtmlControls.HtmlHead)page.Header;
                bool isExistStyleSheet = false;
                foreach (System.Web.UI.Control item in head.Controls)
                {
                    if (item is System.Web.UI.HtmlControls.HtmlLink && item.ID == key)
                    {
                        isExistStyleSheet = true;
                    }
                }
                if (!isExistStyleSheet)
                {
                    System.Web.UI.HtmlControls.HtmlGenericControl styleNode = new System.Web.UI.HtmlControls.HtmlGenericControl("style");
                    styleNode.Attributes.Add("type", "text/css");
                    styleNode.InnerText = style;
                    head.Controls.Add(styleNode);
                }
                return true;
            }
            return false;
        }

    }
}
