using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using VendorSystem.Repository;

namespace VendorSystem.Authorize
{
    public class AuthorizeShow : AuthorizeAttribute
    {

        public string PageName { get; set; }
        public TypeButton TypeButton { get; set; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            if (httpContext.Session["UserID"] == null)
            {
                return false;
            }
            // Else In any Other Case Make Sure User Have Permission
            else
            {
                bool IsVisiable = Authentication.IsVisiable(PageName, TypeButton, HttpContext.Current.Session["RoleID"].ToString());
                if (IsVisiable)
                {
                    return true;
                }
                return false;
            }
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var URL = filterContext.RequestContext.HttpContext.Request.FilePath;
            var OldURL = filterContext.RequestContext.HttpContext.Session["URL"] as string;

            var myAttributes = filterContext.ActionDescriptor.GetCustomAttributes(true);
            bool isHttpPost = false;
            foreach (var item in myAttributes)
            {
                if (item.GetType().Name.ToUpper().Contains("HTTPPOST"))
                {
                    isHttpPost = true;
                    break;
                }
            }
            if (!isHttpPost && !filterContext.ActionDescriptor.ActionName.StartsWith("Export") && OldURL != URL)
            {
                filterContext.RequestContext.HttpContext.Session["URL"] = URL;
            }

            else
            {
                filterContext.RequestContext.HttpContext.Session["URL"] = null;
            }

            filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(
                            new
                            {
                                controller = "Home",
                                action = "Login"
                            })
                        );
        }
    }
}