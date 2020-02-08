using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Thesis.Models
{
    public class Question
    {
        public int QuestionId { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser Sender { get; set; }

        public int GameId { get; set; }
        public virtual Game Game { get; set; }

        [Required(ErrorMessage = "Wprowadz pytanie")]
        [StringLength(100, ErrorMessage = "Pytanie nie moze miec wiecej niz 100 znakow")]
        public string QuestionContent { get; set; }

        public string ReplyContent { get; set; }

        public bool isReadedBySender { get; set; }
        public bool isReadedByReceiver { get; set; }

        public DateTime QuestionDate { get; set; }
        public DateTime ReplyDate { get; set; }
    }
}