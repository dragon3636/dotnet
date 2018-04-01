using OnlineShow.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Common;
namespace OnlineShow.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        protected override void Initialize(RequestContext requestContext)
        {
            // initialize culture on  controller initialization
            base.Initialize(requestContext);
            var session = Session[CommonConstants.CurrentCulture];
            if (session != null)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(session.ToString());
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(session.ToString());
            }
            else
            {
                Session[CommonConstants.CurrentCulture] = "vi";
                Thread.CurrentThread.CurrentCulture = new CultureInfo("vi");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("vi");
            }
        }
        // change culture
        public ActionResult ChangeCulture(string  ddlCulture ,string returnUrl)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(ddlCulture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(ddlCulture);
            Session[CommonConstants.CurrentCulture] = ddlCulture;
            return Redirect(returnUrl);

        }
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            if (session == null)
            {
                filterContext.Result = new RedirectToRouteResult(new
                  RouteValueDictionary(new { controller = "Login", action = "Index", Area = "Admin" }));
            }
            base.OnActionExecuted(filterContext);
        }    
        protected void SetAllExcuting(string message, string type)
        {
            TempData["AlertMessage"] = message;
            if (type == "succeses")
            {
                TempData["AlertType"] = "alert -success";
            }
            else if (type == "warning")
            {
                TempData["AlertType"] = "alert -warning";
            }
            else if (type == "error")
            {
                TempData["AlertType"] = "alert -error";
            }
        }

    }
}