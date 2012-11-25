using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DatabaseProject
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute( // This is the route configuration to access the content section of the website
                name: "ContentItem",
                url: "ContentItem/{action}",
                defaults: new { controller = "ContentItem", action = "Index" }
            );

            routes.MapRoute( // This is the route configuration to access the content section of the website
                name: "QueryTest",
                url: "Query/{PAGE}/{TOPIC}",
                defaults: new { controller = "Query", action = "get_posts", PAGE = 0, TOPIC = ""}
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}