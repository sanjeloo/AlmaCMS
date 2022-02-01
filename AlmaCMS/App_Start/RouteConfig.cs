using System.Web.Mvc;
using System.Web.Routing;

namespace AlmaCMS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
name: "ProductAddComment",
url: "Products/Func/AddComments",
defaults: new { controller = "Products", action = "AddComments" }
);

            routes.MapRoute(
          name: "pages",
          url: "content/{id}/{Title}",
          defaults: new { controller = "Pages", action = "Index", id = UrlParameter.Optional }
      );
            routes.MapRoute(
name: "customeproduct",
url: "CustomeProducts/{id}/{Title}",
defaults: new { controller = "CustomeProducts", action = "index", id = UrlParameter.Optional }
);
            routes.MapRoute(
name: "vlog",
url: "blog/{id}/{Title}",
defaults: new { controller = "news", action = "index", id = UrlParameter.Optional }
);
            routes.MapRoute(
name: "blogdetails",
url: "blog/read/{id}/{Title}",
defaults: new { controller = "news", action = "details", id = UrlParameter.Optional }
);

           routes.MapRoute(
name: "products",
url: "Products/{id}/{Title}",
defaults: new { controller = "products", action = "index", id = UrlParameter.Optional }
);

           routes.MapRoute(
name: "productsdetails",
url: "Products/details/{id}/{Title}",
defaults: new { controller = "products", action = "details", id = UrlParameter.Optional }
);

            routes.MapRoute(
          name: "gallery",
          url: "Gallery/list/{id}/{Title}",
          defaults: new { controller = "gallery", action = "list", id = UrlParameter.Optional }
      );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}