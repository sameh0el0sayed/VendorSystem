using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using VendorSystem.Repository;

namespace VendorSystem.Authorize
{
     class AuthorizeLogin : AuthorizeAttribute
    {
        //protected override bool AuthorizeCore(HttpContextBase httpContext)
        //{
        //    if (httpContext.Session["UserID"] == null)
        //    {
        //        return false;
        //    }

        //    return true;
        //}
        //protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        //{
        //    filterContext.Result = new RedirectToRouteResult(
        //                new RouteValueDictionary(
        //                    new
        //                    {
        //                        controller = "Home",
        //                        action = "Login"
        //                    })
        //                );
        //}
    }
}