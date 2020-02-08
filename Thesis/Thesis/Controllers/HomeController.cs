using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using Thesis.DAL;
using Thesis.Helpers.AI.SentimentalAnalisys;
using Thesis.Helpers.AI.SpamDetection;
using Thesis.Models;

namespace Thesis.Controllers
{
    public class HomeController : Controller
    {
        private ThesisContext context = new ThesisContext();

        // GET: Home
        public ActionResult Index()
        {
             var sentimentalInt = new SentimentalInterpreter();
            var m = sentimentalInt.IsHappy("zła gra nie polecam");
            Game game = context.Games.First();
            return View(game);
        }

        public ActionResult ItemPrompt(string term)
        {
           var games = context.Games.Where(a => !a.Hidden && a.Title.ToLower().Contains(term.ToLower()))
                        .Take(5).Select(a => new { label = a.Title }).ToList();

            return Json(games, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsSpam(string text)
        {
            bool isSpam = SpamDetector.IsContentSpam(text);
            return Json(isSpam, JsonRequestBehavior.AllowGet);
        }
    }
}