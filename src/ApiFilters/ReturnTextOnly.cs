using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Web.Http;
using System.Net.Http.Headers;

namespace Umbraco.GoogleMaps.ApiFilters
{
    public class ReturnTextOnly : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            actionContext.Request.Headers.Accept.Clear();
            actionContext.Request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("text/text"));
        }
    }
}
