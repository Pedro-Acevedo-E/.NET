/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AUTH.VALIDATE
{
    public class ValidateLogin : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session.GetString("_UserSession") != null) {

                filterContext.Result = new RedirectResult("~/Home/Login");
            }

            base.OnActionExecuting(filterContext);
        }


    }
}
*/