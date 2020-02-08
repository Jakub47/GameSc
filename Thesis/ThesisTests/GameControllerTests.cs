using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Thesis.Controllers;
using Thesis.DAL;
using Thesis.Models;

namespace ThesisTests
{
    [TestClass]
    public class GameControllerTests
    {
        private ThesisContext db = new ThesisContext();

        [TestMethod]
        public void MenuCategoryGamesData()
        {
            var gameController = new GameController();
            PartialViewResult pResult = gameController.MenuCategoryGames() as PartialViewResult;
            var gameCategories = pResult.ViewData.Model as List<CategoryGame>;
            Assert.AreEqual(db.CategoryGames.ToList().Count, gameCategories.Count);
        }

        [TestMethod]
        public void DetailTest()
        {
            var id = 2;

            //Get Game from db
            var gameFromDb = db.Games.Where(a => a.GameId == id).FirstOrDefault();

            //Get Game from view
            var gameController = new GameController();
            var detailResult = gameController.Detail(id) as ViewResult;
            var game = detailResult.Model as Game;

            //Check if they are the same
            Assert.AreEqual(gameFromDb.Title, game.Title);
        }
    }
}
