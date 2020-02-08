using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Thesis.Models;

namespace Thesis.ViewModels
{
    public class CurrentCommentPostViewModel
    {
        public ApplicationUser LoggedUser { get; set; }
        public CommentPost Com { get; set; }
    }
}