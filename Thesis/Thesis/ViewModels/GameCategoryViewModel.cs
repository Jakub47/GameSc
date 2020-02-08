using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Thesis.Models;

namespace Thesis.ViewModels
{
    public class GameCategoryViewModel
    {
        public List<CategoryGame> GameCategories { get; set; }
        public Game Game { get; set; }
    }
}