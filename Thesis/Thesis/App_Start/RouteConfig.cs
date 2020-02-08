using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Thesis
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Post", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "GameList",
                url: "Games/{nameOfGameCategory}",
                defaults: new { controller = "Game", action = "Index", nameOfGameCategory = UrlParameter.Optional });

            routes.MapRoute(
               name: "GameDetail",
               url: "Game/{id}",
               defaults: new { controller = "Game", action = "Detail", id = UrlParameter.Optional });

            routes.MapRoute(
               name: "AddComment",
               url: "Post/AddComment",
               defaults: new { controller = "Post", action = "AddComment"});

            routes.MapRoute(
               name: "LikePost",
               url: "Post/LikePost",
               defaults: new { controller = "Post", action = "LikePost" });

            routes.MapRoute(
               name: "UnlikePost",
               url: "Post/UnlikePost",
               defaults: new { controller = "Post", action = "UnlikePost" });

            routes.MapRoute(
             name: "AskQuestion",
             url: "Game/AskQuestion",
             defaults: new { controller = "Game", action = "AskQuestion" });

             routes.MapRoute(
             name: "RespondToQuestion",
             url: "Game/ReplyToQuestion",
             defaults: new { controller = "Game", action = "ReplyToQuestion" });

            routes.MapRoute(
              name: "AddUserToEvent",
              url: "GameEvent/AddUserToGameEvent",
              defaults: new { controller = "GamingEvent", action = "AddUserToGameEvent" });

            
        }
    }
}
