using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
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
    public static class AbstractHttpContext
    {
        public static HttpContext FakeHttpContext(Dictionary<string, object> sessionVariables, string path)
        {
            string username = "username";
            string userid = "15e50543-7897-48d2-a8c8-f1ca967e3ca3"; //could be a constant

            List<Claim> claims = new List<Claim>{
    new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", username),
    new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", userid)
};
            var genericIdentity = new GenericIdentity("");
            genericIdentity.AddClaims(claims);
            var genericPrincipal = new GenericPrincipal(genericIdentity, new string[] { "jakub4742@gmail.com" });


            var httpRequest = new HttpRequest(string.Empty, path, string.Empty);
            var stringWriter = new StringWriter();
            var httpResponce = new HttpResponse(stringWriter);
            var httpContext = new HttpContext(httpRequest, httpResponce);
            httpContext.User = genericPrincipal;
            Thread.CurrentPrincipal = genericPrincipal;
            var sessionContainer = new HttpSessionStateContainer(
              "id",
              new SessionStateItemCollection(),
              new HttpStaticObjectsCollection(),
              10,
              true,
              HttpCookieMode.AutoDetect,
              SessionStateMode.InProc,
              false);

            foreach (var var in sessionVariables)
            {
                sessionContainer.Add(var.Key, var.Value);
            }

            SessionStateUtility.AddHttpSessionStateToContext(httpContext, sessionContainer);
            return httpContext;
        }
    }
}
