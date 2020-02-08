using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Thesis.Infrastructure
{
    public static class UrlHelpers
    {
        public static string CategoriesIconsPath(this UrlHelper helper, string nameIconOfCategory)
        {
            var CategoriesIconsFolder = AppConfig.IconsForCategoryFolder;
            var path = Path.Combine(CategoriesIconsFolder, nameIconOfCategory);
            var pathAb = helper.Content(path);
            return pathAb;
        }

        public static string ImagesGamePath(this UrlHelper helper, string nameOfImage)
        {
            if (nameOfImage == null || nameOfImage == string.Empty)
                nameOfImage = "main.jpg";
            var ImagesFolder = AppConfig.ImagesGameFolder;
            var path = Path.Combine(ImagesFolder, nameOfImage);
            var pathAb = helper.Content(path);
            return pathAb;
        }

        public static string ImagesPostPath(this UrlHelper helper, string nameOfImage)
        {
            if (nameOfImage == null || nameOfImage == string.Empty)
                nameOfImage = "main.jpg";
            var ImagesFolder = AppConfig.ImagesPostFolder;
            var path = Path.Combine(ImagesFolder, nameOfImage);
            var pathAb = helper.Content(path);
            return pathAb;
        }

        public static string ImagesGamingEventPath(this UrlHelper helper, string nameOfImage)
        {
            if (nameOfImage == null || nameOfImage == string.Empty)
                nameOfImage = "main.jpg";
            var ImagesFolder = AppConfig.ImagesGamingEventFolder;
            var path = Path.Combine(ImagesFolder, nameOfImage);
            var pathAb = helper.Content(path);
            return pathAb;
        }

        public static string ImagesUserPath(this UrlHelper helper, string nameOfImage)
        {
            if (nameOfImage == null || nameOfImage == string.Empty)
                nameOfImage = "user.png";
            var ImagesFolder = AppConfig.ImagesUserFolder;
            var path = Path.Combine(ImagesFolder, nameOfImage);
            var pathAb = helper.Content(path);
            return pathAb;
        }

        public static string ImagesOtherPath(this UrlHelper helper, string nameOfImage)
        {
            var ImagesFolder = AppConfig.ImagesOtherFolder;
            var path = Path.Combine(ImagesFolder, nameOfImage);
            var pathAb = helper.Content(path);
            return pathAb;
        }
    }
}