using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Thesis.Controllers;
using Thesis.DAL;

namespace ThesisUnitTests
{
    [TestClass]
    public class CommentControllerTests
    {
        private ThesisContext db = new ThesisContext();

        [TestMethod]
        public void LikePostCommentTest()
        {
            int id = 2;
            HttpContext.Current = AbstractHttpContext.FakeHttpContext(
  new Dictionary<string, object> { { "UserExpandedState", 5 },
                                        { "MaxId", 1000 } },
                                      "http://localhost:55024/api/v1/");

            var comment = db.CommentPosts.Find(id);

            CommentController commentController = new CommentController()
            {
                GetUserId = () => "14a66224-b316-407a-a1bc-507ea56fa8eb"
            };
            JsonResult result = commentController.LikePostComment(id) as JsonResult;
            IDictionary<string, object> wrapper = (IDictionary<string, object>)new System.Web.Routing.RouteValueDictionary(result.Data);

            int? cc = wrapper["amount"] as int?;

            if(cc > comment.Likes)
            {
                Assert.AreEqual(comment.Likes + 1, cc);
            }
            else
            {
                Assert.AreEqual(comment.Likes - 1, cc);
            }
        }



    }
}

