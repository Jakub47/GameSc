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

namespace Thesis.Controllers
{
    public class MessageController : Controller
    {
        private ThesisContext context = new ThesisContext();
        public Func<string> GetUserId; //For testing
        public MessageController()
        {
            GetUserId = () => User.Identity.GetUserId();
        }


        // GET: Message
        public string SendMessage(string content,string userName)
        {
            if (SpamDetector.IsContentSpam(content))
                return null;

            ApplicationUser user = null;
            string id = GetUserId();
            if (id != string.Empty && id != null)
                user = context.Users.Where(a => a.Id == id).First();

            ApplicationUser Receiveruser =context.Users.Where(a => a.UserName == userName).First();
            
            var conversation = context.Conversations.Where(a => (a.SenderId == user.Id && a.Receiver.UserName == userName) || (a.ReceiverId == user.Id && a.Sender.UserName == userName)).FirstOrDefault();


            if(conversation == null)
            {
                //We know that this is their first message so add new Conversation
                conversation = new Conversation()
                {
                    ReceiverId = Receiveruser.Id,
                    Receiver = Receiveruser,
                    SenderId = user.Id,
                    Sender = user,
                    SenderReceived = true,
                    ReceiverReceived = false,
                };
                context.Conversations.Add(conversation);
            }
            else
            {
                //Check who is sender of this message and set flag for notification
                if (conversation.ReceiverId == user.Id)
                {
                    conversation.ReceiverReceived = true;
                    conversation.SenderReceived = false;
                }
                else
                {
                    conversation.SenderReceived = true;
                    conversation.ReceiverReceived = false;
                }
            }

            Content con = new Content()
            {
                ConversationId = conversation.ConversationId,
                Conversation = conversation,
                MessageContent = content,
                SendDate = DateTime.Now,
                UserId = user.Id,
                UserSender = user
            };

            var sentimentalInt = new SentimentalInterpreter();
            var isContentHappy = sentimentalInt.IsHappy(con.MessageContent);
            if (isContentHappy)
                con.IsHappy = true;

            conversation.LastDateTimeSend = con.SendDate;
            context.Contents.Add(con);
            conversation.Contents.Add(con);
            context.SaveChanges();

            return "OK";
        }

        public bool IsNewMessage()
        {
            ApplicationUser user = null;
            string id = GetUserId();
            if (id != string.Empty && id != null)
                user = context.Users.Where(a => a.Id == id).First();

            var conversationsOfReceivedUser = context.Conversations.Where(a => a.ReceiverId == id).ToList();
            var conversationsOfSendedUser = context.Conversations.Where(a => a.SenderId == id).ToList();

            if (conversationsOfReceivedUser == null || conversationsOfSendedUser == null)
                return false;

            if(conversationsOfReceivedUser.Any(a => a.ReceiverReceived == false) 
                || conversationsOfSendedUser.Any(a => a.SenderReceived == false))
                return true;

            return false;
        }

        public int NewMessages()
        {
            ApplicationUser user = null;
            string id = GetUserId();
            if (id != string.Empty && id != null)
                user = context.Users.Where(a => a.Id == id).First();

            var conversationsOfReceivedUser = context.Conversations.Where(a => a.ReceiverId == id).ToList();
            var conversationsOfSendedUser = context.Conversations.Where(a => a.SenderId == id).ToList();

            if (conversationsOfReceivedUser == null || conversationsOfSendedUser == null)
                return 0;

            int convRece = conversationsOfReceivedUser.Where(a => a.ReceiverReceived == false).Count();
            int convSend = conversationsOfSendedUser.Where(a => a.SenderReceived == false).Count();

            return convRece + convSend;
        }
    }
}