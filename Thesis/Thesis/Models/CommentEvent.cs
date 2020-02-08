using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Thesis.Models
{
    public class CommentEvent
    {
        public int CommentEventId { get; set; }

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

        //FK
        public int? GamingEventId { get; set; }

        //navigation property
        public virtual GamingEvent GamingEvent { get; set; }

        public virtual ICollection<ApplicationUser> LikedCommentsEvents { get; set; }
        public virtual ICollection<ApplicationUser> UnlikeCommentEvents { get; set; }

        public int? ParentCommentEventId { get; set; }
        public virtual CommentEvent Parent { get; set; }
        public virtual ICollection<CommentEvent> Children { get; set; }

    }
}