using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Thesis.Models;

namespace Thesis.ViewModels
{
    public class EventCategoryViewModel
    {
        public List<CategoryEvent> EventCategory { get; set; }
        public GamingEvent GamingEvent { get; set; }
    }
}