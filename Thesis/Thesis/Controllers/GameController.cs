using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Thesis.App_Start;
using Thesis.DAL;
using Thesis.Helpers.AI.SpamDetection;
using Thesis.Helpers.Procedural;
using Thesis.Infrastructure;
using Thesis.Models;
using Thesis.ViewModels;

namespace Thesis.Controllers
{
    public class GameController : Controller
    {
        private ThesisContext context = new ThesisContext();
        public Func<string> GetUserId; //For testing
        public GameController()
        {
            GetUserId = () => User.Identity.GetUserId();
        }


        // GET: Game
        public ActionResult Index(string nameOfGameCategory = null)
        {
            var games = nameOfGameCategory != null ? context.Games.Where(a => a.Category.Name == nameOfGameCategory).ToList() :
                                                     context.Games.ToList();
            ObserveController obs = new ObserveController();

            games.ForEach(a =>
            {
                if(obs.IsInObserverGame(a.GameId))
                {
                    a.IsInObserver = true;
                }
            });


            return View(games);
        }

        public ActionResult Detail(int? id)
        {
            if(!id.HasValue || id == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            var game = context.Games.Where(a => a.GameId == id).FirstOrDefault();
            ObserveController obs = new ObserveController();
            if (obs.IsInObserverGame(game.GameId))
                game.IsInObserver = true;
            return View(game);
        }

        public ActionResult Search(string game)
        {
            var games = context.Games.Where(a => !a.Hidden && a.Title.ToLower().Contains(game.ToLower()))
                         .Take(5).ToList();
            return View("Index", games);
        }

        [ChildActionOnly]
        public PartialViewResult MenuCategoryGames()
        {
            //without interface use : HttpContext.Cache.Add("Some value");
            ICache abstractCache = new AbstractCache();

            List<Game> randomGameOffer;
            List<CategoryGame> gameCategories;

            if (abstractCache.IsSet(ConfigurationManager.AppSettings["RandomGameOffer"]))
            {
                randomGameOffer = abstractCache.Get(ConfigurationManager.AppSettings["RandomGameOffer"]) as List<Game>;
            }
            else
            {
                randomGameOffer = context.Games.OrderByDescending(g => Guid.NewGuid()).Take(3).ToList();
                abstractCache.Set(ConfigurationManager.AppSettings["RandomGameOffer"], randomGameOffer, 60);
            }

            if (abstractCache.IsSet(ConfigurationManager.AppSettings["GameCategories"]))
            {
                gameCategories = abstractCache.Get(ConfigurationManager.AppSettings["GameCategories"]) as List<CategoryGame>;
            }
            else
            {
                gameCategories = context.CategoryGames.ToList();
                abstractCache.Set(ConfigurationManager.AppSettings["GameCategories"], gameCategories, 1440);
            }


            var vm = new GameCategoryMenuViewModel()
            {
                Games = randomGameOffer,
                GamesCategory = gameCategories
            };

            return PartialView("_MenuCategoryGames", vm);
        }

        public PartialViewResult AskQuestion(int gameId, string question)
        {
            if (SpamDetector.IsContentSpam(question))
                return null;

            Game game = context.Games.Where(a => a.GameId == gameId).First();
            ApplicationUser user = null;
            string id = User.Identity.GetUserId();
            if (id != string.Empty && id != null)
                user = context.Users.Where(a => a.Id == id).First();

            var qs = new Question()
            {
                UserId = id,
                Sender = user,
                GameId = game.GameId,
                Game = game,
                QuestionContent = question,
                ReplyContent = "",
                isReadedBySender = true,
                isReadedByReceiver = false,
                QuestionDate = DateTime.Now,
                ReplyDate = DateTime.Now
            };
            context.Questions.Add(qs);
            game.Questions.Add(qs);
            user.Questions.Add(qs);
            context.SaveChanges();

            return PartialView("_Questions", game.Questions);
        }

        public PartialViewResult ReplyToQuestion(int questionId, string reply)
        {
            if (SpamDetector.IsContentSpam(reply))
                return null;
            Question question = context.Questions.Where(a => a.QuestionId == questionId).First();
            ApplicationUser user = null;
            string id = User.Identity.GetUserId();
            if (id != string.Empty && id != null)
                user = context.Users.Where(a => a.Id == id).First();

            question.isReadedByReceiver = true;
            question.isReadedBySender = false;
            question.ReplyContent = reply;
            question.ReplyDate = DateTime.Now;
            context.SaveChanges();

            return PartialView("_Questions", question.Game.Questions);
        }

        public PartialViewResult QuestionsOfGame(Game game)
        {
            if (game.Questions.Count() > 0)
                return PartialView("_Questions", game.Questions);
            else
                return PartialView("_Questions", null);
        }



        public ActionResult AddGame()
        {
            ApplicationUser user = null;
            string id = GetUserId();
            if (id != string.Empty && id != null)
                user = context.Users.Where(a => a.Id == id).First();

            if (user.UserInformation.Adress == null || user.UserInformation.Email == null || user.UserInformation.FirstName == null || user.UserInformation.LastName == null || user.UserInformation.PhoneNumber == null || user.UserInformation.PhoneNumber == null || user.UserInformation.PostCode == null || user.UserInformation.Town == null)
            {
                return RedirectToAction("Index","Manage", new { errorMessage = "Proszę wypełnić swoje dane przed dodaniem gry" });
            }

            var vm = new GameCategoryViewModel()
            {
                GameCategories = context.CategoryGames.ToList(),
                Game = new Game()
            };

            return View("~/Views/Game/AddGame.cshtml", vm);
        }

        public ActionResult EditGame(int id)
        {
            ApplicationUser user = null;
            string Userid = GetUserId();
            if (Userid != string.Empty && Userid != null)
                user = context.Users.Where(a => a.Id == Userid).First();

            if (user.UserInformation.Adress == null || user.UserInformation.Email == null || user.UserInformation.FirstName == null || user.UserInformation.LastName == null || user.UserInformation.PhoneNumber == null || user.UserInformation.PhoneNumber == null || user.UserInformation.PostCode == null || user.UserInformation.Town == null)
            {
                return RedirectToAction("Index","Manage",new { errorMessage = "Proszę wypełnić swoje dane przed dodaniem gry" });
            }

            var game = context.Games.Find(id);
            if(game == null)
                return RedirectToAction("Index", new { errorMessage = "Nie ma takiej gry" });

            var vm = new GameCategoryViewModel()
            {
                GameCategories = context.CategoryGames.ToList(),
                Game = game
            };

            return View("~/Views/Game/EditGame.cshtml", vm);
        }

        [HttpPost]
        public ActionResult SaveGame(GameCategoryViewModel vm, HttpPostedFileBase file, string newItem = null)
        {
            //Verify whether user has neccessary data 
            ApplicationUser user = null;
            string id = User.Identity.GetUserId();
            if (id != string.Empty && id != null)
                user = context.Users.Where(a => a.Id == id).First();
            if (user.UserInformation == null)
            {
                RedirectToAction("Index", new { errorMessage = "Proszę wypełnić swoje dane przed dodaniem gry" });
            }

            int idd = vm.Game.GameId;

            var game = vm.Game;
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
                    var path = Path.Combine(Server.MapPath(AppConfig.ImagesGameFolder), filename);
                    //file.SaveAs(path);
                    sourceImage.Save(path);
                    game.MainPicture = filename;
                }

            }
            else if (file == null && newItem != null && newItem != String.Empty)
            {
                var fractImg = FractalGenerator.GenereateFractal1(user.Email);
                var filename = Guid.NewGuid() + ".jpg";
                var path = Path.Combine(Server.MapPath(AppConfig.ImagesGameFolder), filename);
                fractImg.Save(path);
                game.MainPicture = filename;
            }

            game.GamesForExchange = System.Text.RegularExpressions.Regex.Replace(game.GamesForExchange, "[^0-9a-zA-ZżźćńółęąśŻŹĆĄŚĘŁÓŃ ]+", "|");
            game.UserId = user.Id;
            
            
            context.Games.AddOrUpdate(game);
            context.SaveChanges();

            return RedirectToAction("Games", "Manage");
        }

        [HttpPost]
        public string DeleteGame(int? id)
        {
            var game = context.Games.Find(id);
            if (game != null)
            {
                context.Games.Remove(game);
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

    }
}