using Microsoft.AspNetCore.Mvc.Filters;

namespace Frontend.WebUI.Helper
{
  public class SessionOutActionFilter : ActionFilterAttribute
  {
    public override void OnActionExecuting(ActionExecutingContext context)
    {
      //LoginUserSession _userClaimObj = context.HttpContext.Session.GetComplexData<LoginUserSession>(CommonConstantUI.currentUserLogin);

      //if (_userClaimObj == null)
      //{
      //    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
      //    {
      //        controller = "Account",
      //        action = "Login"
      //    }));
      //}
      //else
      //{

      //}

    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
      // Do something after the action executes.
    }
  }
}
