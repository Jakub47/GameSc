using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thesis.DAL;
using Thesis.Models;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Globalization;
using Thesis.App_Start;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Thesis.ViewModels;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Thesis.Infrastructure;
using Thesis.Helpers.Procedural;
using Thesis.Helpers.AI.SentimentalAnalisys;
using Thesis.Helpers.AI.SpamDetection;

namespace Thesis.Controllers
{
    public class PostController : Controller
    {
        public Func<string> GetUserId; //For testing
        public PostController()
        {
            GetUserId = () => User.Identity.GetUserId();
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

        private ThesisContext context = new ThesisContext();
        // GET: Post
        public ActionResult Index(string nameOfPostCategory = null)
        {
            var postCategory = context.CategoryPosts.Where(a => a.Name.ToLower() == nameOfPostCategory.ToLower()).FirstOrDefault();

            var posts = postCategory != null ? context.Posts.Where(a => a.CategoryPostId == postCategory.CategoryPostId).ToList() :
                                                    context.Posts.ToList();

            ApplicationUser user = null;
            string id = GetUserId();
            if (id != string.Empty && id != null)
                user = context.Users.Where(a => a.Id == id).First();



            PostUserViewModel vm = new PostUserViewModel
            {
                LoggedUser = user,
                Posts = posts
            };

            return View(vm);
        }


        [ChildActionOnly]
        [OutputCache(Duration = 60000)]
        public PartialViewResult MenuCategoryPosts()
        {
            var postCategories = context.CategoryPosts.ToList();
            return PartialView("_MenuCategoryPosts", postCategories);
        }

        [HttpPost]
        public PartialViewResult AddComment(int postId, string content)
        {
            if (SpamDetector.IsContentSpam(content))
                return null;
            Post post = context.Posts.Where(a => a.PostId == postId).First();
            ApplicationUser user = null;
            string id = User.Identity.GetUserId();
            if (id != string.Empty && id != null)
                user = context.Users.Where(a => a.Id == id).First();

            CommentPost commentPost = new CommentPost() { Body = content, DateOfInsert = DateTime.Now, PostId = post.PostId, UserId = User.Identity.GetUserId(), User = user };

            

            var sentimentalInt = new SentimentalInterpreter();
            var isContentHappy = sentimentalInt.IsHappy(commentPost.Body);
            if (isContentHappy)
                commentPost.IsHappy = true;

            context.CommentPosts.Add(commentPost);
            context.SaveChanges();

            post.Comments.Add(commentPost);

            CurrentPostUserViewModel vm = new CurrentPostUserViewModel
            {
                LoggedUser = user,
                Post = post
            };

            vm.Post.Comments = vm.Post.Comments.OrderBy(a => a.DateOfInsert).ToList();
            return PartialView("_ListOfComments", vm);
        }

        [HttpGet]
        public JsonResult LikePost(int postId)
        {
            bool isNew = false;

            ApplicationUser user = null;
            string id = User.Identity.GetUserId();
            if (id != string.Empty && id != null)
                user = context.Users.Where(a => a.Id == id).First();

            Post post = context.Posts.Where(a => a.PostId == postId).First();

            if (post != null &&  user.LikedPosts.Contains(post))
            {
                isNew = false;
                post.Likes--;
                user.LikedPosts.Remove(post);
            }
            else
            {
                isNew = true;
                post.Likes++;
                user.LikedPosts.Add(post);
            }

            context.SaveChanges();
            
            return Json(new { amount = post.Likes, isNewPost = isNew }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult UnlikePost(int postId)
        {
            bool isNew = false;

            ApplicationUser user = null;
            string id = User.Identity.GetUserId();
            if (id != string.Empty && id != null)
                user = context.Users.Where(a => a.Id == id).First();

            Post post = context.Posts.Where(a => a.PostId == postId).First();

            if (post != null && user.UnLikePosts.Contains(post))
            {
                isNew = false;
                post.UnLikes--;
                user.UnLikePosts.Remove(post);
            }
            else if(post != null && !(user.UnLikePosts.Contains(post)) )
            {
                isNew = true;
                post.UnLikes++;
                user.UnLikePosts.Add(post);
            }

            context.SaveChanges();
            
            return Json(new { amount = post.UnLikes, isNewPost = isNew }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AddPost()
        {
            var vm = new PostCategoryViewModel()
            {
                PostCategories = context.CategoryPosts.ToList(),
                Post = new Post()
            };

            return View("~/Views/Post/AddPost.cshtml", vm);
        }

        public ActionResult EditPost(int id)
        {
            var post = context.Posts.Find(id);
            if (post == null)
                return RedirectToAction("Index", new { errorMessage = "Nie ma takiego postu" });


            var vm = new PostCategoryViewModel()
            {
                PostCategories = context.CategoryPosts.ToList(),
                Post = post
            };

            return View("~/Views/Post/EditPost.cshtml", vm);
        }

        [HttpPost]
        public ActionResult SavePost(PostCategoryViewModel vm, HttpPostedFileBase file,string newItem = null)
        {
            #region Spam Detextion
            if (SpamDetector.IsContentSpam(vm.Post.Title))
                return RedirectToAction("Index", "Post");
            #endregion

            #region GetUserAndSaveUserImage
            //Verify whether user has neccessary data 
            ApplicationUser user = null;
            string id = User.Identity.GetUserId();
            if (id != string.Empty && id != null)
                user = context.Users.Where(a => a.Id == id).First();


            int idd = vm.Post.PostId;

            var post = vm.Post;
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
                    var path = Path.Combine(Server.MapPath(AppConfig.ImagesPostFolder), filename);
                    //file.SaveAs(path);
                    sourceImage.Save(path);
                    post.MainPicture = filename;
                }

            }
            else if(file == null && newItem != null && newItem != String.Empty)
            {
                var fractImg = FractalGenerator.GenereateFractal1(user.Email);
                var filename = Guid.NewGuid() + ".jpg";
                var path = Path.Combine(Server.MapPath(AppConfig.ImagesPostFolder), filename);
                fractImg.Save(path);
                post.MainPicture = filename;

            }

            #endregion

            #region Sentimental Analisys
            var sentimentalInt = new SentimentalInterpreter();
            var isContentHappy = sentimentalInt.IsHappy(post.Title);
            if (isContentHappy)
                post.IsHappy = true;
            #endregion

            post.DateOfInsert = DateTime.Now;
            post.UserId = user.Id;
            post.User = user;
            
            if (user.Posts.Where(a => a.PostId == post.PostId).Count() == 0)
            {
                user.Posts.Add(post);
            }

            context.Posts.AddOrUpdate(post);
            context.SaveChanges();



            //InsertOrUpdate(post);

            return RedirectToAction("Post", "Manage");
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
        public string DeletePost(int? id)
        {
            var post = context.Posts.Find(id);
            if (post != null)
            {
                context.Posts.Remove(post);
                context.SaveChanges();
                return "Ok";
            }
            else
                return null;
        }

        public PartialViewResult ListOfPostComment(Post element)
        {
            
            ApplicationUser user = null;
            string id = GetUserId();
            if (id != string.Empty && id != null)
                user = context.Users.Where(a => a.Id == id).First();



            CurrentPostUserViewModel vm = new CurrentPostUserViewModel
            {
                LoggedUser = user,
                Post = element
            };

            vm.Post.Comments = vm.Post.Comments.OrderBy(a => a.DateOfInsert).ToList();
            return PartialView("_ListOfComments",vm);
        }

        public PartialViewResult ChildrenPostComment(CommentPost childCom)
        {
            ApplicationUser user = null;
            string id = GetUserId();
            if (id != string.Empty && id != null)
                user = context.Users.Where(a => a.Id == id).First();



            CurrentCommentPostViewModel vm = new CurrentCommentPostViewModel
            {
                LoggedUser = user,
                Com = childCom
            };

            if(vm.Com.Children != null && vm.Com.Children.Count > 1)
                vm.Com.Children = vm.Com.Children.OrderBy(a => a.DateOfInsert).ToList();

            return PartialView("_ChildrenComments", vm);
        }

    }
}