using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Thesis.Models;

namespace Thesis.ViewModels
{
    public class CurrentGameEventUserViewModel
    {
        public ApplicationUser LoggedUser { get; set; }
        public GamingEvent GamingEvent { get; set; }
    }
}