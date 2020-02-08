using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Thesis.Controllers;
using Thesis.Models;

namespace ThesisUnitTests
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void IndexView()
        {
            HttpContext.Current = AbstractHttpContext.FakeHttpContext(
   new Dictionary<string, object> { { "UserExpandedState", 5 },
                                        { "MaxId", 1000 } },
                                       "http://localhost:55024/api/v1/");


            HomeController homeController = new HomeController();
            ViewResult result = homeController.Index() as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void IndexData()
        {
            HttpContext.Current = AbstractHttpContext.FakeHttpContext(
   new Dictionary<string, object> { { "UserExpandedState", 5 },
                                        { "MaxId", 1000 } },
                                       "http://localhost:55024/api/v1/");


            HomeController homeController = new HomeController();
            ViewResult result = homeController.Index() as ViewResult;
            Game game = result.ViewData.Model as Game;
            Assert.AreEqual("Doom", game.Title);
        }
    }
}
