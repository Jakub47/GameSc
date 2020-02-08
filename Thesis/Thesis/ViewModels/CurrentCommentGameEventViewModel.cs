using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Thesis.Models;

namespace Thesis.ViewModels
{
    public class CurrentCommentGameEventViewModel
    {
        public ApplicationUser LoggedUser { get; set; }
        public CommentEvent Com { get; set; }
    }
}