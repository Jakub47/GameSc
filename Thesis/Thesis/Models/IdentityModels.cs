using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Thesis.Models
{
    public class ApplicationUser : IdentityUser
    {

        //Game
        public virtual ICollection<Game> Games { get; set; }
        public virtual ICollection<Question> Questions { get; set; }


        //Post Properties
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<CommentPost> CommentPosts { get; set; }

        [InverseProperty("LikePostdBy")]
        public virtual ICollection<Post> LikedPosts { get; set; }
        [InverseProperty("UnLikedPostBy")]
        public virtual ICollection<Post> UnLikePosts { get; set; }

        [InverseProperty("LikedBy")]
        public virtual ICollection<CommentPost> LikedCommentsPosts { get; set; }
        [InverseProperty("UnLikedBy")]
        public virtual ICollection<CommentPost> UnlikeCommentPosts { get; set; }


        //Gaming Events
        public virtual ICollection<CommentEvent> CommentEvents { get; set; }

        [InverseProperty("LikedCommentsEvents")]
        public virtual ICollection<CommentEvent> LikedCommentsEvents { get; set; }
        [InverseProperty("UnlikeCommentEvents")]
        public virtual ICollection<CommentEvent> UnlikeCommentEvents { get; set; }

        [InverseProperty("Publisher")]
        public virtual ICollection<GamingEvent> UserGamingEvent { get; set; }
        [InverseProperty("UsersToAttend")]
        public virtual ICollection<GamingEvent> AttendGamingEvent { get; set; }

        //Conversation
        [InverseProperty("Sender")]
        public virtual ICollection<Conversation> ConversationsSended { get; set; }
        [InverseProperty("Receiver")]
        public virtual ICollection<Conversation> ConversationsReceived { get; set; }


        //Properties
        public UserInformation UserInformation { get; set; }
        public string MainPicture { get; set; }
        public string Nickname { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}