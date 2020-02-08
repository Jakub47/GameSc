using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Thesis.Infrastructure
{
    public class AppConfig
    {
        public static string IconsForCategoryFolder { get; } = ConfigurationManager.AppSettings["IconsForCategoryFolder"];

        public static string ImagesGameFolder { get; } = ConfigurationManager.AppSettings["ImagesGameFolder"];

        public static string ImagesPostFolder { get; } = ConfigurationManager.AppSettings["ImagesPostFolder"];

        public static string ImagesGamingEventFolder { get; } = ConfigurationManager.AppSettings["ImagesGamingEventFolder"];

        public static string ImagesUserFolder { get; } = ConfigurationManager.AppSettings["ImagesUserFolder"];

        public static string ImagesOtherFolder { get; } = ConfigurationManager.AppSettings["ImagesOtherFolder"];

        public static string FolderWithContentToDelete { get; } = ConfigurationManager.AppSettings["FolderWithContentToDelete"];
    }
}