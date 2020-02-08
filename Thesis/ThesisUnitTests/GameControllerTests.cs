using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MvcContrib.TestHelper;
using Thesis.Controllers;
using Thesis.DAL;
using Thesis.Models;
using Thesis.ViewModels;

namespace ThesisUnitTests
{
    [TestClass]
    public class GameControllerTests
    {
        private ThesisContext db = new ThesisContext();

        [TestMethod]
        public void MenuCategoryGamesData()
        {
            HttpContext.Current = AbstractHttpContext.FakeHttpContext(
    new Dictionary<string, object> { { "UserExpandedState", 5 },
                                        { "MaxId", 1000 } },
                                        "http://localhost:55024/api/v1/");


            var gameController = new GameController();
            PartialViewResult pResult = gameController.MenuCategoryGames() as PartialViewResult;
            var gameCategories = pResult.ViewData.Model as GameCategoryMenuViewModel;
            Assert.AreEqual(db.CategoryGames.ToList().Count(), gameCategories.GamesCategory.Count());
        }

        [TestMethod]
        public void DetailTest()
        {
            HttpContext.Current = AbstractHttpContext.FakeHttpContext(
     new Dictionary<string, object> { { "UserExpandedState", 5 },
                                        { "MaxId", 1000 } },
                                         "http://localhost:55024/api/v1/");


            //var context = new Mock<ControllerContext>();
            //var session = new Mock<HttpSessionStateBase>();

            //context.Setup(m => m.HttpContext.Session).Returns(session.Object);

            var id = 2;

            //Get Game from db
            var gameFromDb = db.Games.Where(a => a.GameId == id).FirstOrDefault();
            //Get Game from view
            var gameController = new GameController();
            //    gameController.ControllerContext = context.Object;


            var detailResult = gameController.Detail(id) as ViewResult;
            var game = detailResult.Model as Game;

            //Check if they are the same
            Assert.AreEqual(gameFromDb.Title, game.Title);
        }

        [TestMethod]
        public void EditGameTest()
        {
            HttpContext.Current = AbstractHttpContext.FakeHttpContext(
     new Dictionary<string, object> { { "UserExpandedState", 5 },
                                        { "MaxId", 1000 } },
                                         "http://localhost:55024/api/v1/");


            //var context = new Mock<ControllerContext>();
            //var session = new Mock<HttpSessionStateBase>();

            //context.Setup(m => m.HttpContext.Session).Returns(session.Object);

            var id = 2;

            //Get Game from db
            var gameFromDb = db.Games.Where(a => a.GameId == id).FirstOrDefault();
            //Get Game from view
            var gameController = new GameController()
            {
                GetUserId = () => "14a66224-b316-407a-a1bc-507ea56fa8eb"
            };
            //    gameController.ControllerContext = context.Object;


            var detailResult = gameController.EditGame(id) as ViewResult;
            if(detailResult == null)
            {
                //It means that there was redirect cause user didnt write his information to db
                var redirectResult = gameController.EditGame(id) as RedirectToRouteResult;
                var s = redirectResult.RouteValues.First().Value;
                Assert.AreEqual("Proszę wypełnić swoje dane przed dodaniem gry", redirectResult.RouteValues.First().Value);
                return;
            }
            var game = detailResult.Model as GameCategoryViewModel;
            
            //Check if they are the same
            Assert.AreEqual(gameFromDb.Title, game.Game.Title);
        }
    }
}
