using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Thesis.Models
{
    public class Content
    {
        public int ContentId { get; set; }

        public int ConversationId { get; set; }
        public virtual Conversation Conversation { get; set; }

        //Sended by
        public string UserId { get; set; }
        public virtual ApplicationUser UserSender { get; set; }

        public string MessageContent { get; set; }
        public bool IsHappy { get; set; }
        public DateTime SendDate { get; set; }
    }
}