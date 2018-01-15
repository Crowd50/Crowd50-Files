using System.Web.Mvc;

namespace Crowd50.Custom
{
    public class CheckSession : ActionFilterAttribute
    {
        private string Redirect;
        private string Key;
        public CheckSession(string key, string redirect, params int[] ids)
        {
            Key = key;
            Redirect = redirect;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(filterContext.HttpContext.Session[Key] == null)
            {
                filterContext.Result = new RedirectResult(Redirect, false);
            }
            base.OnActionExecuting(filterContext);
        }
    }
}