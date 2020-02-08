using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Thesis.Models;

namespace Thesis.ViewModels
{
    public class PostUserViewModel
    {
        public ApplicationUser LoggedUser { get; set; }
        public List<Post> Posts { get; set; }
    }
}