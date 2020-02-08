using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
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
using Thesis.Helpers.Procedural;
using Thesis.Infrastructure;
using Thesis.Models;
using Thesis.ViewModels;

namespace Thesis.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        ThesisContext context = new ThesisContext();

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            Error
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        // GET: Manage
        public async Task<ActionResult> Index(ManageMessageId? message, string errorMessage = null, string successfull = null)
        {
            if (TempData["ViewData"] != null)
            {
                ViewData = (ViewDataDictionary)TempData["ViewData"];
            }

            if (errorMessage != null)
            {
                ViewBag.ErrorMessage = errorMessage;
            }

            if (successfull != null)
            {
                ViewBag.Successfull = successfull;
            }

            if (User.IsInRole("Admin"))
                ViewBag.UserIsAdmin = true;
            else
                ViewBag.UserIsAdmin = false;

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }

            var model = new ManageCredentialsViewModel
            {
                Message = message,
                UserInformation = user.UserInformation,
                User = user
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeProfile([Bind(Prefix = "UserInformation")]UserInformation userInformation)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                user.UserInformation = userInformation;
                var result = await UserManager.UpdateAsync(user);

                AddErrors(result);
            }

            if (!ModelState.IsValid)
            {
                TempData["ViewData"] = ViewData;
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index", new { successfull = "Poprawnie zapisano dane" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword([Bind(Prefix = "ChangePasswordViewModel")]ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ViewData"] = ViewData;
                return RedirectToAction("Index");
            }

            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess, successfull = "Poprawnie zmieniono hasło" });
            }
            AddErrors(result);

            if (!ModelState.IsValid)
            {
                TempData["ViewData"] = ViewData;
                return RedirectToAction("Index");
            }

            var message = ManageMessageId.ChangePasswordSuccess;
            return RedirectToAction("Index", new { Message = message, successfull = "Poprawnie zmieniono hasło" });
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("password-error", error);
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        public async Task<ActionResult> Games(string message = null)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            var userGames = user.Games.ToList();
            if (message != null)
                ViewBag.Message = message;
            //var exchanges = context.Exchanges.In
            return View(userGames);
        }

        public async Task<ActionResult> AddGame()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user.UserInformation.Adress == null || user.UserInformation.Email == null || user.UserInformation.FirstName == null || user.UserInformation.LastName == null || user.UserInformation.PhoneNumber == null || user.UserInformation.PhoneNumber == null || user.UserInformation.PostCode == null || user.UserInformation.Town == null)
            {
                return RedirectToAction("Index", new { errorMessage = "Proszę wypełnić swoje dane przed dodaniem gry" });
            }

            var vm = new GameCategoryViewModel()
            {
                GameCategories = context.CategoryGames.ToList(),
                Game = new Game()
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<ActionResult> AddGame(GameCategoryViewModel vm, HttpPostedFileBase file)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user.UserInformation == null)
            {
                RedirectToAction("Index", new { errorMessage = "Proszę wypełnić swoje dane przed dodaniem gry" });
            }
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

            game.UserId = user.Id;

            context.Games.Add(game);
            user.Games.Add(game);
            context.SaveChanges();
            //context.Exchanges.Add(new Exchange() { GameId = game.GameId, Game = game, DateOfInsert = DateTime.Now });
            //context.SaveChanges();

            return RedirectToAction("Index", "Manage");
        }

        public async Task<ActionResult> Events()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            var events = user.UserGamingEvent.ToList();
            //var exchanges = context.Exchanges.In
            return View(events);
        }


        public ActionResult Post()
        {
            ApplicationUser user = null;
            string id = User.Identity.GetUserId();
            if (id != string.Empty && id != null)
                user = context.Users.Where(a => a.Id == id).First();

            var posts = user.Posts.ToList();
            //var exchanges = context.Exchanges.In
            return View(posts);
        }

        public async Task<ActionResult> MessagesOfUser()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            var messages = context.Conversations.Where(a => a.SenderId == user.Id || a.ReceiverId == user.Id).OrderByDescending(a => a.LastDateTimeSend).ToList();

            if (messages.Count > 1)
            {
                var sortedList = new List<Conversation>();
                messages.ForEach(a => sortedList.Add(a)); 

                foreach (var item in messages)
                {
                    if ((item.ReceiverId == user.Id && item.ReceiverReceived == false)
                        || (item.SenderId == user.Id && item.SenderReceived == false))
                    {
                        sortedList.Remove(item);
                        sortedList.Insert(0, item);
                    }
                }

                return View(sortedList);
            }
            //var exchanges = context.Exchanges.In
            return View(messages);
        }

        public async Task<ActionResult> ConversationOfUser(int id)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            var conversation = context.Conversations.Where(a => a.ConversationId == id).FirstOrDefault();

            if (conversation == null)
                return RedirectToAction("Index", "Manage");

            if (conversation.ReceiverId == user.Id || conversation.SenderId == user.Id)
            {
                if (conversation.ReceiverId == user.Id)
                    conversation.ReceiverReceived = true;
                else
                    conversation.SenderReceived = true;
                context.SaveChanges();

                return View(conversation);
            }
            //In case if userid is not recerived id of if not sender id return to home page
            return RedirectToAction("Index", "Manage");

        }

        
        public ActionResult SendNewContent(string content,int conversationId)
        {
            ApplicationUser user = null;
            string id = User.Identity.GetUserId();
            if (id != string.Empty && id != null)
                user = context.Users.Where(a => a.Id == id).First();

            var conversation = context.Conversations.Where(a => a.ConversationId == conversationId).FirstOrDefault();
            if(conversation != null)
            {
                Content con = new Content()
                {
                    ConversationId = conversation.ConversationId,
                    Conversation = conversation,
                    MessageContent = content,
                    SendDate = DateTime.Now,
                    UserId = user.Id,
                    UserSender = user
                };

                //Check who is sender of this message and set flag for notification
                if(conversation.ReceiverId == user.Id)
                {
                    conversation.ReceiverReceived = true;
                    conversation.SenderReceived = false;
                }
                else
                {
                    conversation.SenderReceived = true;
                    conversation.ReceiverReceived = false;
                }

                conversation.LastDateTimeSend = con.SendDate;

                context.Contents.Add(con);
                conversation.Contents.Add(con);
                context.SaveChanges();

                return PartialView("_MessagesList", conversation);

            }


            return null;
        }




        [HttpPost]
        public ActionResult AddEvent(EventCategoryViewModel vm, HttpPostedFileBase file)
        {

            ApplicationUser user = null;
            string id = User.Identity.GetUserId();
            if (id != string.Empty && id != null)
                user = context.Users.Where(a => a.Id == id).First();

            var GameEvent = vm.GamingEvent;
            string fileToSave = "";

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
                    fileToSave = filename;
                }
            }

            GameEvent.UserId = user.Id;
            if (fileToSave != string.Empty)
            {
                GameEvent.MainPicture = fileToSave;
            }
            context.GamingEvents.Add(GameEvent);
            user.UserGamingEvent.Add(GameEvent);
            context.SaveChanges();

            return RedirectToAction("Index", "Manage");
        }

        public ActionResult SaveImage(string ImgOption, HttpPostedFileBase file)
        {
            int choiceOfUser = Convert.ToInt32(ImgOption);
            var m = Request.Files.Count;
            switch (choiceOfUser)
            {
                case 1:
                    SaveNormalImage();
                    break;
                case 2:
                    SaveSimpleImage();
                    break;
                case 3:
                    SaveCustomImage();
                    break;
                case 4:
                    SaveUserImage(file);
                    break;
                default:
                    return RedirectToAction("Index");
            }
            return RedirectToAction("Index", new {successfull = "Poprawnie zmieniono obrazek" });
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



        private void SaveCustomImage()
        {
            ApplicationUser user = null;
            string id = User.Identity.GetUserId();
            if (id != string.Empty && id != null)
                user = context.Users.Where(a => a.Id == id).First();

            var img = IdenticonImage.GenerateCustomImage(user.Email);
            img = ResizeImage(img, 500, 500);
            var filename = Guid.NewGuid() + ".jpg"; // Unikalny identyfikator + rozszerzenie
            var currentImg = user.MainPicture;

            var path = Path.Combine(Server.MapPath(AppConfig.ImagesUserFolder), filename);
            var pathToDelete = Path.Combine(Server.MapPath(AppConfig.ImagesUserFolder), currentImg);
            System.IO.File.Delete(pathToDelete);

            img.Save(path);
            user.MainPicture = filename;
            context.SaveChanges();
        }

        private void SaveSimpleImage()
        {
            ApplicationUser user = null;
            string id = User.Identity.GetUserId();
            if (id != string.Empty && id != null)
                user = context.Users.Where(a => a.Id == id).First();

            var img = IdenticonImage.GenerateSimpleImage(user.Email);
            img = ResizeImage(img, 500, 500);
            var filename = Guid.NewGuid() + ".jpg"; // Unikalny identyfikator + rozszerzenie
            var currentImg = user.MainPicture;

            var path = Path.Combine(Server.MapPath(AppConfig.ImagesUserFolder), filename);
            var pathToDelete = Path.Combine(Server.MapPath(AppConfig.ImagesUserFolder), currentImg);
            System.IO.File.Delete(pathToDelete);

            img.Save(path);
            user.MainPicture = filename;
            context.SaveChanges();
        }

        private void SaveNormalImage()
        {
            ApplicationUser user = null;
            string id = User.Identity.GetUserId();
            if (id != string.Empty && id != null)
                user = context.Users.Where(a => a.Id == id).First();

            var img = IdenticonImage.GenerateNormalImage(user.Email);
            img = ResizeImage(img, 500, 500);
            var filename = Guid.NewGuid()  + ".jpg"; // Unikalny identyfikator + rozszerzenie
            var currentImg = user.MainPicture;

            var path = Path.Combine(Server.MapPath(AppConfig.ImagesUserFolder), filename);
            var pathToDelete = Path.Combine(Server.MapPath(AppConfig.ImagesUserFolder), currentImg);
            System.IO.File.Delete(pathToDelete);

            img.Save(path);
            user.MainPicture = filename;
            context.SaveChanges();
        }

        private void SaveUserImage(HttpPostedFileBase file)
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
            var currentImg = user.MainPicture;

            var path = Path.Combine(Server.MapPath(AppConfig.ImagesUserFolder), filename);
            var pathToDelete = Path.Combine(Server.MapPath(AppConfig.ImagesUserFolder), currentImg);
            System.IO.File.Delete(pathToDelete);

            sourceImage.Save(path);
            user.MainPicture = filename;
            context.SaveChanges();
        }
        //public async Task<ActionResult> Exchange()
        //{
        //    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
        //    if (user == null)
        //    {
        //        return View("Error");
        //    }

        //    List<Exchange> userExchanges;
        //}
    }
}
