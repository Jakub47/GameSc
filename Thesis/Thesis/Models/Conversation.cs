using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Thesis.Models
{
    public class Conversation
    {
        
        public int ConversationId { get; set; }

        // AspNetUsers
        public virtual ApplicationUser Sender { get; set; }
        public virtual ApplicationUser Receiver { get; set; }

        // SenderId of the message
        [Required]
        [ForeignKey("Sender")]
        [Display(Name = "Sender")]
        //  [InverseProperty("MessageSenderId")]
        public string SenderId { get; set; }

        // ReceiverId of this message
        [Required]
        [ForeignKey("Receiver")]
        [Display(Name = "Receiver")]
        //   [InverseProperty("MessageReceiverId")]
        public string ReceiverId { get; set; }


        public bool SenderReceived { get; set; }
        public bool ReceiverReceived { get; set; }
        public DateTime LastDateTimeSend { get; set; }

        public virtual ICollection<Content> Contents { get; set; }

    }
}