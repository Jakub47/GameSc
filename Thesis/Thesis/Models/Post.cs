using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Thesis.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public int CategoryPostId { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Required(ErrorMessage = "Proszę dodać tytuł")]
        [StringLength(75, ErrorMessage = "Tytul nie moze miec wiecej niz 100 znakow")]
        [DisplayName("Tytuł")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Proszę dodać tresc")]
        [StringLength(750, ErrorMessage = "Tresc nie moze miec wiecej niz 750 znakow")]
        [DisplayName("Tresc")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        public DateTime DateOfInsert { get; set; }
        public int Likes { get; set; }
        public int UnLikes { get; set; }

        [DisplayName("Główny obrazek")]
        public string MainPicture { get; set; }
        public bool IsHappy { get; set; }

        public virtual CategoryPost Category { get; set; }
        public virtual ICollection<CommentPost> Comments { get; set; }

        public virtual ICollection<ImagePost> ImagePost { get; set; }

        public virtual ICollection<ApplicationUser> LikePostdBy { get; set; }
        public virtual ICollection<ApplicationUser> UnLikedPostBy { get; set; }
    }
}