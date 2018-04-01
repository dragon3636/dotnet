using OnlineShow.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace OnlineShow
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        //protected void Application_Error( object sender,EventArgs e)
        //{
        //    Exception exception = Server.GetLastError();
        //    Response.Clear();
        //    HttpException httpexcetion = exception as HttpException;
        //    RouteData route = new RouteData();
        //    route.Values.Add("controller","Error");
        //    if (httpexcetion !=null)
        //    {
        //        switch (httpexcetion.GetHttpCode())
        //        {
        //            case 404:
        //                route.Values.Add("action", "http404");
        //                break;
        //            case 500:
        //                route.Values.Add("action", "http500");
        //                break;
        //            default:
        //                route.Values.Add("action", "General");
        //                break;
        //        }
        //        Server.ClearError();
        //        Response.TrySkipIisCustomErrors=true;
        //    }
        //    IController errorcontroller = new ErrorController();
        //    errorcontroller.Execute(new RequestContext(new HttpContextWrapper(Context), route));
           
        //}
    }
}
