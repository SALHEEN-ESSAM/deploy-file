using System.Web.Mvc;
using System.Web.Routing;

namespace Amana
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Sitemapxml",
                url: "{language}/{page}.xml",
                defaults: new { controller = "Sitemap", action = "Index", language = "ar", page = UrlParameter.Optional }
            );

            routes.MapRoute(
                "AjaxController",
                "{language}/Ajax/{action}/{id}",
                new { controller = "Ajax", language = "ar", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "ContactPage",
                "{language}/contact/{perma}/{id}",
                new { controller = "Common", action = "Contact", language = "ar", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "SignIn",
                "{language}/{action}",
                new { controller = "Home", language = "ar" },
                namespaces: new[] { "Amana.Controllers" }
            );

            routes.MapRoute(
                "GoogleCallback",
                "{language}/GoogleLoginCallback",
                new { controller = "Home", action = "GoogleLoginCallback", language = "ar" },
                namespaces: new[] { "Amana.Controllers" }
            );

            routes.MapRoute(
                "CommonPages",
                "{language}/{action}/{id}",
                new { controller = "Common", language = "ar", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "NodePages",
                "{language}/content/{perma}/{id}",
                new { controller = "N", action = "List", language = "ar", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "NodeDetailsPages",
                "{language}/page/{perma}/{id}",
                new { controller = "N", action = "Details", language = "ar", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "NodeImages",
                "{language}/images/{perma}/{id}",
                new { controller = "N", action = "Images", language = "ar", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "ListPages",
                "{language}/{controller}/{perma}/{id}",
                new { action = "List", language = "ar", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "ImagesPages",
                "{language}/{controller}/images/{perma}/{id}",
                new { action = "Images", language = "ar", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "DetailsPages",
                "{language}/{controller}/d/{perma}/{id}",
                new { action = "Details", language = "ar", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "HomePage",
                "{language}",
                new { controller = "Home", action = "Index", language = "ar" },
                new { id = @"\d+" } //this means that id must be numeric
            );

            routes.MapRoute(
                "IndexPages",
                "{language}/{controller}/{id}",
                new { action = "Index", language = "ar", id = UrlParameter.Optional },
                new { id = @"\d+" } //this means that id must be numeric
            );

            routes.MapRoute(
                name: "Default",
                url: "{language}/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", language = "ar", id = UrlParameter.Optional },
                namespaces: new[] { "Amana.Areas.Admin.Controllers" }
            );


        }
    }
}
