using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thesis.DAL;
using Thesis.Helpers.AI.SentimentalAnalisys;
using Thesis.Helpers.AI.SpamDetection;
using Thesis.Helpers.Procedural;
using Thesis.Infrastructure;
using Thesis.Models;
using Thesis.ViewModels;

namespace Thesis.Controllers
{
    public class GamingEventController : Controller
    {
        private ThesisContext context = new ThesisContext();

        public Func<string> GetUserId; //For testing
        public GamingEventController()
        {
            GetUserId = () => User.Identity.GetUserId();
        }

        // GET: GamingEvent
        public ActionResult Index(string nameOfEventCategory = null)
        {
            var events = nameOfEventCategory != null ? context.GamingEvents.Where(a => a.CategoryEvent.Name == nameOfEventCategory).ToList() :
                                                    context.GamingEvents.ToList();

            ApplicationUser user = null;
            string id = GetUserId();
            if (id != string.Empty && id != null)
                user = context.Users.Where(a => a.Id == id).First();



            GameEventViewModel vm = new GameEventViewModel
            {
                LoggedUser = user,
                GamingEvents = events
            };

            return View(vm);
        }

        [ChildActionOnly]
        public PartialViewResult MenuCategoryEvents()
        {
            //without interface use : HttpContext.Cache.Add("Some value");
            ICache abstractCache = new AbstractCache();

            List<GamingEvent> randomEvents;
            List<CategoryEvent> gameEventsCategories;

            if (abstractCache.IsSet(ConfigurationManager.AppSettings["RandomEventOffer"]))
            {
                randomEvents = abstractCache.Get(ConfigurationManager.AppSettings["RandomEventOffer"]) as List<GamingEvent>;
            }
            else
            {
                randomEvents = context.GamingEvents.OrderByDescending(g => Guid.NewGuid()).Take(3).ToList();
                abstractCache.Set(ConfigurationManager.AppSettings["RandomEventOffer"], randomEvents, 60);
            }

            if (abstractCache.IsSet(ConfigurationManager.AppSettings["GameEventsCategories"]))
            {
                gameEventsCategories = abstractCache.Get(ConfigurationManager.AppSettings["GameEventsCategories"]) as List<CategoryEvent>;
            }
            else
            {
                gameEventsCategories = context.CategoryEvents.ToList();
                abstractCache.Set(ConfigurationManager.AppSettings["GameEventsCategories"], gameEventsCategories, 1440);
            }


            var vm = new GameEventCategoryMenuViewModel()
            {
                GamingEvents  = randomEvents,
                CategoryEvents = gameEventsCategories
            };

            return PartialView("_MenuCategoryGameEvents", vm);
        }
    
        public JsonResult AddUserToGameEvent(int gameEventId = 1)
        {
            var gameEvent = context.GamingEvents.Where(a => a.GamingEventId == gameEventId).First();

            ApplicationUser user = null;
            string id = User.Identity.GetUserId();
            if (id != string.Empty && id != null)
                user = context.Users.Where(a => a.Id == id).First();

            gameEvent.UsersToAttend.Add(user);
            user.AttendGamingEvent.Add(gameEvent);
            context.SaveChanges();

            return Json(gameEvent.UsersToAttend.Select(x => new
            {
                userName = x.UserName
            }), JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveUserFromGameEvent(int gameEventId = 1)
        {
            var gameEvent = context.GamingEvents.Where(a => a.GamingEventId == gameEventId).First();

            ApplicationUser user = null;
            string id = User.Identity.GetUserId();
            if (id != string.Empty && id != null)
                user = context.Users.Where(a => a.Id == id).First();

            gameEvent.UsersToAttend.Remove(user);
            user.AttendGamingEvent.Remove(gameEvent);
            context.SaveChanges();

            return Json(gameEvent.UsersToAttend.Select(x => new
            {
                userName = x.UserName
            }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddEvent()
        {
            var vm = new EventCategoryViewModel()
            {
                EventCategory = context.CategoryEvents.ToList(),
                GamingEvent = new GamingEvent()
            };
            return View("~/Views/GamingEvent/AddGameEvent.cshtml", vm);
        }

        public ActionResult EditEvent(int id)
        {
            var gameEvent = context.GamingEvents.Find(id);
            if (gameEvent == null)
                return RedirectToAction("Index", new { errorMessage = "Nie ma takiego eventu" });

            var vm = new EventCategoryViewModel()
            {
                EventCategory = context.CategoryEvents.ToList(),
                GamingEvent =gameEvent
            };

            return View("~/Views/GamingEvent/EditGameEvent.cshtml", vm);
        }


        [HttpPost]
        public ActionResult SaveEvent(EventCategoryViewModel vm, HttpPostedFileBase file, string newItem = null)
        {
            if (SpamDetector.IsContentSpam(vm.GamingEvent.Title))
                return RedirectToAction("Index", "Post");
            //Verify whether user has neccessary data 
            ApplicationUser user = null;
            string id = User.Identity.GetUserId();
            if (id != string.Empty && id != null)
                user = context.Users.Where(a => a.Id == id).First();


            int idd = vm.GamingEvent.GamingEventId;

            var gameEvent = vm.GamingEvent;
            if (file != null && file.ContentLength > 0)
            {
                if (Path.GetExtension(file.FileName).ToLower() == ".jpg" || Path.GetExtension(file.FileName).ToLower() == ".png")
                {
                    var sourceImage = Image.FromStream(file.InputStream);

                    sourceImage = ResizeImage(sourceImage, 500, 500);

                    //Generowanie plik
                    var fileExt = Path.GetExtension(file.FileName);
                    var filename = Guid.NewGuid() + fileExt; // Unikalny identyfikator + rozszerzenie

                    //W jakim folderze ma byc umiesczony dany plik oraz jego nazwa! Oraz zapis
                    var path = Path.Combine(Server.MapPath(AppConfig.ImagesGamingEventFolder), filename);
                    //file.SaveAs(path);
                    sourceImage.Save(path);
                    gameEvent.MainPicture = filename;
                }

            }
            else if (file == null && newItem != null && newItem != String.Empty)
            {
                var fractImg = FractalGenerator.GenereateFractal1(user.Email);
                var filename = Guid.NewGuid() + ".jpg";
                var path = Path.Combine(Server.MapPath(AppConfig.ImagesGamingEventFolder), filename);
                fractImg.Save(path);
                gameEvent.MainPicture = filename;
            }

            gameEvent.UserId = user.Id;
            gameEvent.Publisher = user;

            context.GamingEvents.AddOrUpdate(gameEvent);
            context.SaveChanges();

            return RedirectToAction("Events", "Manage");
        }

        [HttpPost]
        public string DeleteEvent(int? id)
        {
            var gameEvent = context.GamingEvents.Find(id);
            if (gameEvent != null)
            {
                context.GamingEvents.Remove(gameEvent);
                context.SaveChanges();
                return "Ok";
            }
            else
                return null;
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
        public PartialViewResult AddComment(int gamingEventId, string content)
        {
            if (SpamDetector.IsContentSpam(content))
                return null;

            var gameEvent = context.GamingEvents.Where(a => a.GamingEventId == gamingEventId).First();
            ApplicationUser user = null;
            string id = User.Identity.GetUserId();
            if (id != string.Empty && id != null)
                user = context.Users.Where(a => a.Id == id).First();

            CommentEvent commentEvent = new CommentEvent()
            { Body = content, DateOfInsert = DateTime.Now, GamingEventId = gameEvent.GamingEventId,
                UserId = User.Identity.GetUserId(), User = user };

            var sentimentalInt = new SentimentalInterpreter();
            var isContentHappy = sentimentalInt.IsHappy(commentEvent.Body);
            if (isContentHappy)
                commentEvent.IsHappy = true;

            context.CommentEvents.Add(commentEvent);
            context.SaveChanges();

            gameEvent.Comments.Add(commentEvent);

            CurrentGameEventUserViewModel vm = new CurrentGameEventUserViewModel
            {
                LoggedUser = user,
                GamingEvent = gameEvent
            };

            vm.GamingEvent.Comments = vm.GamingEvent.Comments.OrderBy(a => a.DateOfInsert).ToList();
            return PartialView("_ListOfComments", vm);
        }


        public PartialViewResult ListOfEventComment(GamingEvent element)
        {

            ApplicationUser user = null;
            string id = GetUserId();
            if (id != string.Empty && id != null)
                user = context.Users.Where(a => a.Id == id).First();



            CurrentGameEventUserViewModel vm = new CurrentGameEventUserViewModel
            {
                LoggedUser = user,
                GamingEvent = element
            };

            return PartialView("_ListOfComments", vm);
        }

        public PartialViewResult ChildrenEventComment(CommentEvent childCom)
        {
            ApplicationUser user = null;
            string id = GetUserId();
            if (id != string.Empty && id != null)
                user = context.Users.Where(a => a.Id == id).First();



            CurrentCommentGameEventViewModel vm = new CurrentCommentGameEventViewModel
            {
                LoggedUser = user,
                Com = childCom
            };

            if (vm.Com.Children != null && vm.Com.Children.Count > 1)
                vm.Com.Children = vm.Com.Children.OrderBy(a => a.DateOfInsert).ToList();

            return PartialView("_ChildrenComments", vm);
        }
    }
}