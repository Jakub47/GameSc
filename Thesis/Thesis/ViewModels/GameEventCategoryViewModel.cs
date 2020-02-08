using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Thesis.Models;

namespace Thesis.ViewModels
{
    public class GameEventCategoryViewModel
    {
        public List<CategoryEvent> CategoryEvents { get; set; }
        public GamingEvent GamingEvent { get; set; }
    }
}