using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Thesis.Models;

namespace Thesis.ViewModels
{
    public class GameCategoryMenuViewModel
    {
        public IEnumerable<Game> Games { get; set; }
        public IEnumerable<CategoryGame> GamesCategory { get; set; }
    }
}