using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thesis.DAL;
using Thesis.Helpers.AI.SentimentalAnalisys;
using Thesis.Helpers.AI.SpamDetection;
using Thesis.Models;
using Thesis.ViewModels;

namespace Thesis.Controllers
{
    public class CommentController : Controller
    {
        private ThesisContext context = new ThesisContext();

        public Func<string> GetUserId; //For testing
        public CommentController()
        {
            GetUserId = () => User.Identity.GetUserId();
        }


        // GET: Comment
        public JsonResult LikePostComment(int commentId)
        {
            var comment = context.CommentPosts.Find(commentId);
            bool isNew = false;

            ApplicationUser user = null;
            string id = GetUserId();
            if (id != string.Empty && id != null)
                user = context.Users.Where(a => a.Id == id).First();

            if (user.LikedCommentsPosts.Contains(comment))
            {
                isNew = false;
                comment.Likes--;
                user.LikedCommentsPosts.Remove(comment);
            }
            else
            {
                isNew = true;
                comment.Likes++;
                user.LikedCommentsPosts.Add(comment);
            }

            context.SaveChanges();

            return Json(new { amount = comment.Likes, isNewComment = isNew }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UnLikePostComment(int commentId)
        {
            var comment = context.CommentPosts.Find(commentId);
            bool isNew = false;

            ApplicationUser user = null;
            string id = GetUserId();
            if (id != string.Empty && id != null)
                user = context.Users.Where(a => a.Id == id).First();

            if (user.UnlikeCommentPosts.Contains(comment))
            {
                isNew = false;
                comment.UnLikes--;
                user.UnlikeCommentPosts.Remove(comment);
            }
            else
            {
                isNew = true;
                comment.UnLikes++;
                user.UnlikeCommentPosts.Add(comment);
            }

            context.SaveChanges();

            return Json(new { amount = comment.UnLikes, isNewComment = isNew }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LikeGameEventComment(int commentId)
        {
            var comment = context.CommentEvents.Find(commentId);
            bool isNew = false;

            ApplicationUser user = null;
            string id = GetUserId();
            if (id != string.Empty && id != null)
                user = context.Users.Where(a => a.Id == id).First();

            if (user.LikedCommentsEvents.Contains(comment))
            {
                isNew = false;
                comment.Likes--;
                user.LikedCommentsEvents.Remove(comment);
            }
            else
            {
                isNew = true;
                comment.Likes++;
                user.LikedCommentsEvents.Add(comment);
            }

            context.SaveChanges();

            return Json(new { amount = comment.Likes, isNewComment = isNew }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UnLikeGameEventComment(int commentId)
        {
            var comment = context.CommentEvents.Find(commentId);
            bool isNew = false;

            ApplicationUser user = null;
            string id = GetUserId();
            if (id != string.Empty && id != null)
                user = context.Users.Where(a => a.Id == id).First();

            if (user.UnlikeCommentEvents.Contains(comment))
            {
                isNew = false;
                comment.UnLikes--;
                user.UnlikeCommentEvents.Remove(comment);
            }
            else
            {
                isNew = true;
                comment.UnLikes++;
                user.UnlikeCommentEvents.Add(comment);
            }

            context.SaveChanges();

            return Json(new { amount = comment.UnLikes, isNewComment = isNew }, JsonRequestBehavior.AllowGet);
        }


        public PartialViewResult AddChildCommentPost(int idOfParentId, string content)
        {
            if (SpamDetector.IsContentSpam(content))
                return null;

            var parentComment = context.CommentPosts.Find(idOfParentId);
            Post post = context.Posts.Where(a => a.PostId == parentComment.PostId).First();

            ApplicationUser user = null;
            string id = User.Identity.GetUserId();
            if (id != string.Empty && id != null)
                user = context.Users.Where(a => a.Id == id).First();

            CommentPost commentPost = new CommentPost()
            {
                Body = content,
                DateOfInsert = DateTime.Now,
                PostId = post.PostId,
                UserId = User.Identity.GetUserId(),
                User = user,
                ParentCommentPostId = parentComment.CommentPostId,
                Parent = parentComment
            };

            var sentimentalInt = new SentimentalInterpreter();
            var isContentHappy = sentimentalInt.IsHappy(commentPost.Body);
            if (isContentHappy)
                commentPost.IsHappy = true;

            parentComment.Children.Add(commentPost);
            context.CommentPosts.Add(commentPost);

            context.SaveChanges();



            CurrentCommentPostViewModel vm = new CurrentCommentPostViewModel
            {
                LoggedUser = user,
                Com = parentComment
            };

            if (vm.Com.Children != null && vm.Com.Children.Count > 1)
                vm.Com.Children = vm.Com.Children.OrderBy(a => a.DateOfInsert).ToList();

            return PartialView("~/Views/Post/_ChildrenComments.cshtml", vm);
        }

        public PartialViewResult AddChildCommentEvent(int idOfParentId, string content)
        {
            if (SpamDetector.IsContentSpam(content))
                return null;

            var parentComment = context.CommentEvents.Find(idOfParentId);
            GamingEvent gameEvent = context.GamingEvents.Where(a => a.GamingEventId == parentComment.GamingEventId).First();

            ApplicationUser user = null;
            string id = User.Identity.GetUserId();
            if (id != string.Empty && id != null)
                user = context.Users.Where(a => a.Id == id).First();


            CommentEvent commentEvent = new CommentEvent()
            {
                Body = content,
                DateOfInsert = DateTime.Now,
                GamingEventId = gameEvent.GamingEventId,
                UserId = User.Identity.GetUserId(),
                User = user,
                ParentCommentEventId = parentComment.CommentEventId,
                Parent = parentComment
            };

            var sentimentalInt = new SentimentalInterpreter();
            var isContentHappy = sentimentalInt.IsHappy(commentEvent.Body);
            if (isContentHappy)
                commentEvent.IsHappy = true;

            parentComment.Children.Add(commentEvent);
            context.CommentEvents.Add(commentEvent);

            context.SaveChanges();


            CurrentCommentGameEventViewModel vm = new CurrentCommentGameEventViewModel
            {
                LoggedUser = user,
                Com = parentComment
            };


            if (vm.Com.Children != null && vm.Com.Children.Count > 1)
                vm.Com.Children = vm.Com.Children.OrderBy(a => a.DateOfInsert).ToList();
            return PartialView("~/Views/GamingEvent/_ChildrenComments.cshtml", vm);

        }

    }
}