using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Thesis.Controllers;
using Thesis.DAL;
using Thesis.Models;
using Thesis.ViewModels;

namespace ThesisUnitTests
{
    [TestClass]
    public class PostControllerTests
    {
        private ThesisContext db = new ThesisContext();

        [TestMethod]
        public void IndexView()
        {
            HttpContext.Current = AbstractHttpContext.FakeHttpContext(
   new Dictionary<string, object> { { "UserExpandedState", 5 },
                                        { "MaxId", 1000 } },
                                       "http://localhost:55024/api/v1/");

            
            PostController postController = new PostController()
            {
                GetUserId = () => "14a66224-b316-407a-a1bc-507ea56fa8eb"
            };
            ViewResult result = postController.Index() as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void IndexData()
        {
            HttpContext.Current = AbstractHttpContext.FakeHttpContext(
   new Dictionary<string, object> { { "UserExpandedState", 5 },
                                        { "MaxId", 1000 } },
                                       "http://localhost:55024/api/v1/");

            PostController postController = new PostController()
            {
                GetUserId = () => "14a66224-b316-407a-a1bc-507ea56fa8eb"
            };
            ViewResult result = postController.Index() as ViewResult;
            PostUserViewModel posts = result.ViewData.Model as PostUserViewModel;
            Assert.AreEqual(db.Posts.Count(), posts.Posts.Count);
        }
    }
}
