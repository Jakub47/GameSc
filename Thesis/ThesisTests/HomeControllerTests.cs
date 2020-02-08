using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Thesis.Controllers;
using Thesis.DAL;
using Thesis.Models;

namespace ThesisTests
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void IndexView()
        {
            HomeController homeController = new HomeController();
            ViewResult result = homeController.Index() as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void IndexData()
        {
            HomeController homeController = new HomeController();
            ViewResult result = homeController.Index() as ViewResult;
            Game game = result.ViewData.Model as Game;
            Assert.AreEqual("Doom", game.Title);
        }
    }
}
