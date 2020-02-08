using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Thesis.Models;

namespace Thesis.ViewModels
{
    public class GameEventViewModel
    {
        public ApplicationUser LoggedUser { get; set; }
        public List<GamingEvent> GamingEvents { get; set; }
    }
}