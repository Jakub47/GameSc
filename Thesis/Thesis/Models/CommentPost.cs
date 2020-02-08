using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Thesis.Models
{
    public class CommentPost
    {
        public int CommentPostId { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Required(ErrorMessage = "Wprowadz tresc komentarza")]
        [StringLength(100, ErrorMessage = "Tresc nie moze miec wiecej niz 100 znakow")]
        public string Body { get; set; }

        //TODO: Dodać walidacje do daty
        public DateTime DateOfInsert { get; set; }
        public int Likes { get; set; }
        public int UnLikes { get; set; }

        public bool Hidden { get; set; }
        public bool IsHappy { get; set; }


        public int PostId { get; set; }
        public virtual Post Post { get; set; }

        public virtual ICollection<ApplicationUser> LikedBy { get; set; }
        public virtual ICollection<ApplicationUser> UnLikedBy { get; set; }

        public int? ParentCommentPostId { get; set; }
        public virtual CommentPost Parent { get; set; }
        public virtual ICollection<CommentPost> Children { get; set; }


    }
}