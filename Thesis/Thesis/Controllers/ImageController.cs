using Microsoft.AspNet.Identity;
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thesis.DAL;
using Thesis.Helpers.AI.ObjectDetection;
using Thesis.Helpers.AI.ObjectDetection.DataStructures;
using Thesis.Helpers.AI.ObjectDetection.YoloParser;
using Thesis.Helpers.Procedural;
using Thesis.Infrastructure;
using Thesis.Models;

namespace Thesis.Controllers
{
    public class ImageController : Controller
    {
        ThesisContext context = new ThesisContext();

        // GET: Image
        public ActionResult CustomImage()
        {
            string email = User.Identity.Name;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            IdenticonImage.GenerateCustomImage(email).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] bitmapData = ms.ToArray();
            return File(bitmapData, "image/jpeg");
        }

        public ActionResult SimpleImage()
        {
            string email = User.Identity.Name;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            IdenticonImage.GenerateSimpleImage(email).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] bitmapData = ms.ToArray();
            return File(bitmapData, "image/jpeg");
        }

        public ActionResult NormalImage()
        {
            string email = User.Identity.Name;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            IdenticonImage.GenerateNormalImage(email).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] bitmapData = ms.ToArray();
            return File(bitmapData, "image/jpeg");
        }


        public ActionResult SaveCustomImage()
        {
            ApplicationUser user = null;
            string id = User.Identity.GetUserId();
            if (id != string.Empty && id != null)
                user = context.Users.Where(a => a.Id == id).First();

            var img = IdenticonImage.GenerateCustomImage(user.Email);
            img = ResizeImage(img, 500, 500);
            var filename = Guid.NewGuid() + ".jpg"; // Unikalny identyfikator + rozszerzenie
                                                    //W jakim folderze ma byc umiesczony dany plik oraz jego nazwa! Oraz zapis
            var path = Path.Combine(Server.MapPath(AppConfig.ImagesUserFolder), filename);
            img.Save(path);
            user.MainPicture = filename;

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] bitmapData = ms.ToArray();
            return File(bitmapData, "image/jpeg");
        }

        public ActionResult SaveSimpleImage()
        {
            ApplicationUser user = null;
            string id = User.Identity.GetUserId();
            if (id != string.Empty && id != null)
                user = context.Users.Where(a => a.Id == id).First();

            var img = IdenticonImage.GenerateSimpleImage(user.Email);
            img = ResizeImage(img, 500, 500);
            var filename = Guid.NewGuid() + ".jpg"; // Unikalny identyfikator + rozszerzenie
                                                    //W jakim folderze ma byc umiesczony dany plik oraz jego nazwa! Oraz zapis
            var path = Path.Combine(Server.MapPath(AppConfig.ImagesUserFolder), filename);
            img.Save(path);
            user.MainPicture = filename;

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] bitmapData = ms.ToArray();
            return File(bitmapData, "image/jpeg");
        }

        public ActionResult SaveNormalImage()
        {
            ApplicationUser user = null;
            string id = User.Identity.GetUserId();
            if (id != string.Empty && id != null)
                user = context.Users.Where(a => a.Id == id).First();

            var img = IdenticonImage.GenerateCustomImage(user.Email);
            img = ResizeImage(img, 500, 500);
            var filename = Guid.NewGuid() + ".jpg"; // Unikalny identyfikator + rozszerzenie
                                                    //W jakim folderze ma byc umiesczony dany plik oraz jego nazwa! Oraz zapis
            var path = Path.Combine(Server.MapPath(AppConfig.ImagesUserFolder), filename);
            img.Save(path);
            user.MainPicture = filename;

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] bitmapData = ms.ToArray();
            return File(bitmapData, "image/jpeg");
        }



        public ActionResult Fractal1()
        {
            string email = User.Identity.Name;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            FractalGenerator.GenereateFractal1(email).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] bitmapData = ms.ToArray();
            return File(bitmapData, "image/jpeg");
        }
        public ActionResult Fractal2()
        {
            string email = User.Identity.Name;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            FractalGenerator.GenereateFractal2(email).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] bitmapData = ms.ToArray();
            return File(bitmapData, "image/jpeg");
        }
        public ActionResult Fractal3()
        {
            string email = User.Identity.Name;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            FractalGenerator.GenereateFractal3
(email).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] bitmapData = ms.ToArray();
            return File(bitmapData, "image/jpeg");
        }


        public ActionResult SaveUserImage(HttpPostedFileBase file)
        {
            ApplicationUser user = null;
            string id = User.Identity.GetUserId();
            if (id != string.Empty && id != null)
                user = context.Users.Where(a => a.Id == id).First();

            var sourceImage = Image.FromStream(file.InputStream);

            sourceImage = ResizeImage(sourceImage, 500, 500);

            //Generowanie plik
            var fileExt = Path.GetExtension(file.FileName);
            var filename = Guid.NewGuid() + fileExt; // Unikalny identyfikator + rozszerzenie

            //W jakim folderze ma byc umiesczony dany plik oraz jego nazwa! Oraz zapis
            var path = Path.Combine(Server.MapPath(AppConfig.ImagesUserFolder), filename);
            //file.SaveAs(path);
            sourceImage.Save(path);
            user.MainPicture = filename;

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            sourceImage.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] bitmapData = ms.ToArray();
            return File(bitmapData, "image/jpeg");
        }

        [NonAction]
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        [HttpPost]
        public JsonResult CheckImage()
        {
            string isPersonOnImage = "no";

            var file = Request.Files[0];
            var currentPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            var modelFilePath = currentPath + @"\Content\Model\TinyYolo2_model.onnx";
            //var modelFilePath = @"C:\Users\Ragnus\Desktop\120819\praca\Thesis\Thesis\Content\Model\TinyYolo2_model.onnx";

            var sourceImage = Image.FromStream(file.InputStream);
            var fileExt = Path.GetExtension(file.FileName);
            var filename = Guid.NewGuid() + fileExt; // Unikalny identyfikator + rozszerzenie

            var outputFolder = currentPath + @"\Content\ImageToReturn\";
            
            //var outputFolder = @"C:\Users\Ragnus\Desktop\120819\praca\Thesis\Thesis\Content\ImageToReturn\";
            var path = Path.Combine(Server.MapPath(AppConfig.ImagesUserFolder), filename);
            sourceImage.Save(path);

            MLContext mlContext = new MLContext();
            IEnumerable<ImageNetData> images = ImageNetData.ReadFrom1File(path);
            IDataView imageDataView = mlContext.Data.LoadFromEnumerable(images);
            var modelScorer = new OnnxModelScorer(path, modelFilePath, mlContext);
            IEnumerable<float[]> probabilities = modelScorer.Score(imageDataView);
            YoloOutputParser parser = new YoloOutputParser();

            var boundingBoxes =
                probabilities
                .Select(probability => parser.ParseOutputs(probability))
                .Select(boxes => parser.FilterBoundingBoxes(boxes, 5, .5F));

            // Draw bounding boxes for detected objects in each of the images
            for (var i = 0; i < images.Count(); i++)
            {
                string imageFileName = images.ElementAt(i).Label;
                IList<YoloBoundingBox> detectedObjects = boundingBoxes.ElementAt(i);

                if (detectedObjects.Count > 0 && detectedObjects[0].Label == "person")
                    isPersonOnImage = "yes";
                DrawBoundingBox(path, outputFolder, imageFileName, detectedObjects);

                LogDetectedObjects(imageFileName, detectedObjects);
            }

            var img = Image.FromFile(outputFolder + filename);
            
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
            System.IO.File.Delete(path);

            img.Dispose();
            System.IO.File.Delete(outputFolder + filename);
            return Json(new { isPerson = isPersonOnImage }
          , JsonRequestBehavior.AllowGet);
        }

        public JsonResult ReturmCheckImage()
        {
            var file = Request.Files[0];
            var currentPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            var modelFilePath = currentPath + @"\Content\Model\TinyYolo2_model.onnx";

            //var modelFilePath = @"C:\Users\Ragnus\Desktop\120819\praca\Thesis\Thesis\Content\Model\TinyYolo2_model.onnx";

            var sourceImage = Image.FromStream(file.InputStream);
            var fileExt = Path.GetExtension(file.FileName);
            var filename = Guid.NewGuid() + fileExt; // Unikalny identyfikator + rozszerzenie

            var outputFolder = currentPath + @"\Content\ImageToReturn\";

//            var outputFolder = @"C:\Users\Ragnus\Desktop\120819\praca\Thesis\Thesis\Content\ImageToReturn\";

            var path = Path.Combine(Server.MapPath(AppConfig.ImagesUserFolder), filename);
            sourceImage.Save(path);

            MLContext mlContext = new MLContext();
            IEnumerable<ImageNetData> images = ImageNetData.ReadFrom1File(path);
            IDataView imageDataView = mlContext.Data.LoadFromEnumerable(images);
            var modelScorer = new OnnxModelScorer(path, modelFilePath, mlContext);
            IEnumerable<float[]> probabilities = modelScorer.Score(imageDataView);
            YoloOutputParser parser = new YoloOutputParser();

            var boundingBoxes =
                probabilities
                .Select(probability => parser.ParseOutputs(probability))
                .Select(boxes => parser.FilterBoundingBoxes(boxes, 5, .5F));

            // Draw bounding boxes for detected objects in each of the images
            for (var i = 0; i < images.Count(); i++)
            {
                string imageFileName = images.ElementAt(i).Label;
                IList<YoloBoundingBox> detectedObjects = boundingBoxes.ElementAt(i);

                var mn = detectedObjects.Count;
                DrawBoundingBox(path, outputFolder, imageFileName, detectedObjects);

                LogDetectedObjects(imageFileName, detectedObjects);
            }

            var img = Image.FromFile(outputFolder + filename);


            

            byte[] bytes;
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                bytes = ms.ToArray();
            }

            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
            System.IO.File.Delete(path);

            img.Dispose();
            System.IO.File.Delete(outputFolder + filename);
            return Json(new { base64imgage = Convert.ToBase64String(bytes) }
          , JsonRequestBehavior.AllowGet);
        }

        private static void DrawBoundingBox(string inputImageLocation, string outputImageLocation, string imageName, IList<YoloBoundingBox> filteredBoundingBoxes)
        {
            Image image = Image.FromFile(inputImageLocation);

            var originalImageHeight = image.Height;
            var originalImageWidth = image.Width;

            foreach (var box in filteredBoundingBoxes)
            {
                // Get Bounding Box Dimensions
                var x = (uint)Math.Max(box.Dimensions.X, 0);
                var y = (uint)Math.Max(box.Dimensions.Y, 0);
                var width = (uint)Math.Min(originalImageWidth - x, box.Dimensions.Width);
                var height = (uint)Math.Min(originalImageHeight - y, box.Dimensions.Height);

                // Resize To Image
                x = (uint)originalImageWidth * x / OnnxModelScorer.ImageNetSettings.imageWidth;
                y = (uint)originalImageHeight * y / OnnxModelScorer.ImageNetSettings.imageHeight;
                width = (uint)originalImageWidth * width / OnnxModelScorer.ImageNetSettings.imageWidth;
                height = (uint)originalImageHeight * height / OnnxModelScorer.ImageNetSettings.imageHeight;

                // Bounding Box Text
                string text = $"{box.Label} ({(box.Confidence * 100).ToString("0")}%)";

                using (Graphics thumbnailGraphic = Graphics.FromImage(image))
                {
                    thumbnailGraphic.CompositingQuality = CompositingQuality.HighQuality;
                    thumbnailGraphic.SmoothingMode = SmoothingMode.HighQuality;
                    thumbnailGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    // Define Text Options
                    Font drawFont = new Font("Arial", 12, FontStyle.Bold);
                    SizeF size = thumbnailGraphic.MeasureString(text, drawFont);
                    SolidBrush fontBrush = new SolidBrush(Color.Black);
                    Point atPoint = new Point((int)x, (int)y - (int)size.Height - 1);

                    // Define BoundingBox options
                    Pen pen = new Pen(box.BoxColor, 3.2f);
                    SolidBrush colorBrush = new SolidBrush(box.BoxColor);

                    // Draw text on image 
                    thumbnailGraphic.FillRectangle(colorBrush, (int)x, (int)(y - size.Height - 1), (int)size.Width, (int)size.Height);
                    thumbnailGraphic.DrawString(text, drawFont, fontBrush, atPoint);

                    // Draw bounding box on image
                    thumbnailGraphic.DrawRectangle(pen, x, y, width, height);
                }
            }

            if (!Directory.Exists(outputImageLocation))
            {
                Directory.CreateDirectory(outputImageLocation);
            }

            image.Save(Path.Combine(outputImageLocation, imageName));
        }

        private static void LogDetectedObjects(string imageName, IList<YoloBoundingBox> boundingBoxes)
        {
            Console.WriteLine($".....The objects in the image {imageName} are detected as below....");

            foreach (var box in boundingBoxes)
            {
                Console.WriteLine($"{box.Label} and its Confidence score: {box.Confidence}");
            }

            Console.WriteLine("");
        }
    }
}