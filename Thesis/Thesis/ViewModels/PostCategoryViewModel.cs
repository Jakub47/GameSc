using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Thesis.Models;

namespace Thesis.ViewModels
{
    public class PostCategoryViewModel
    {
        public List<CategoryPost> PostCategories { get; set; }
        public Post Post { get; set; }
    }
}