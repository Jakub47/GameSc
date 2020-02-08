using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Thesis.Controllers;
using Thesis.Models;

namespace ThesisTests
{
    [TestClass]
    public class PostControllerTests
    {
        [TestMethod]
        public void IndexView()
        {

            PostController postController = new PostController();
            ViewResult result = postController.Index() as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void IndexData()
        {
            PostController postController = new PostController();
            ViewResult result = postController.Index() as ViewResult;
            List<Post> posts = result.ViewData.Model as List<Post>;
            Assert.AreEqual("LOL ale fart :)", posts.First().Title);
        }
    }
}
