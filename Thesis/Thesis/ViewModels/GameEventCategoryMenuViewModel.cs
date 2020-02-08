using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Thesis.Models;

namespace Thesis.ViewModels
{
    public class GameEventCategoryMenuViewModel
    {
        public IEnumerable<GamingEvent> GamingEvents { get; set; }
        public IEnumerable<CategoryEvent> CategoryEvents { get; set; }
    }
}