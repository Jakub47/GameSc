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
using Thesis.Helpers;
using Thesis.Helpers.Procedural;
using Thesis.Infrastructure;
using Thesis.Models;
using Thesis.ViewModels;

namespace Thesis.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

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

        // GET: Account
        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            HttpPostedFileBase file = model.ImageFile;

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, UserInformation = new UserInformation(model.Email),Nickname = model.NickName};
                if (file != null && file.ContentLength > 0 && model.IsPersonOnImage == "yes")
                {
                    if (Path.GetExtension(file.FileName).ToLower() == ".jpg" || Path.GetExtension(file.FileName).ToLower() == ".png")
                    {
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
                    }
                }
                else
                {
                    Random rand = new Random();
                    Image img;

                    while (true)
                    {
                        //Generate custom image 
                        if(rand.NextDouble() <= 0.33)
                        {
                            img = IdenticonImage.GenerateCustomImage(model.Email);
                            break;
                        }

                        //Generate normal image 
                        if (rand.NextDouble() <= 0.33)
                        {
                            img = IdenticonImage.GenerateNormalImage(model.Email);
                            break;
                        }
                        //Generate simple image 
                        if (rand.NextDouble() <= 0.33)
                        {
                            img = IdenticonImage.GenerateSimpleImage(model.Email);
                            break;
                        }
                    }
                    img = ResizeImage(img, 500, 500);
                    var filename = Guid.NewGuid() + ".jpg"; // Unikalny identyfikator + rozszerzenie
                    //W jakim folderze ma byc umiesczony dany plik oraz jego nazwa! Oraz zapis
                    var path = Path.Combine(Server.MapPath(AppConfig.ImagesUserFolder), filename);
                    img.Save(path);
                    user.MainPicture = filename;
                }
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Post");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Post");
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

        public FileContentResult Js()
        {
            return File(System.IO.File.ReadAllBytes(Server.MapPath("~/Scripts/worker.nude.js")), "text/javascript");
        }

        [ChildActionOnly]
        public string GetMainImageOfLoggedUSer()
        {
            var user =  UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.MainPicture;
            }
            else
            {
                return "";
            }
        }

        public ActionResult ReturnProceduralNames(string choice)
        {
            var markov = new MarkovChainNameGenerator(choice);
            return Json(markov.ReturnAll(choice),JsonRequestBehavior.AllowGet);
        }
    }
}