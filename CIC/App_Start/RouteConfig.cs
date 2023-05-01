using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;

namespace CIC
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            var settings = new FriendlyUrlSettings();
            settings.AutoRedirectMode = RedirectMode.Permanent;
            routes.EnableFriendlyUrls(settings);
            RouteTable.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { 
                id = System.Web.Http.RouteParameter.Optional
                });
        }
    }
}
