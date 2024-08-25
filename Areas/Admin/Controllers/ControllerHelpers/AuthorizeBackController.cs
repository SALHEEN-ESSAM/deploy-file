
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Models.Entities;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Models.Utilities;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using System.Linq;
using System.Web;

namespace Amana.ControllerHelpers
{
    public abstract class AuthorizeBackController : ControllerHelper
    {
        //this method runs before any action method
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            //check user login
            if (!requestContext.HttpContext.Request.Url.AbsoluteUri.Contains("/Admin/C_Login"))
            {
                if (requestContext.HttpContext.Session["UserDetails"] == null)
                {
                    HttpCookie userCookie = requestContext.HttpContext.Request.Cookies["UserLoginDetails"];
                    if (userCookie != null)
                    {
                        string email = userCookie["Email"];
                        string password = StringCipher.Decrypt(userCookie["Passowrd"], true);
                        C_UserItems user = db.C_UserItems.SingleOrDefault(x => x.Email == email && x.Password == password);
                        if (user != null)
                        {
                            requestContext.HttpContext.Session["UserDetails"] = user;
                        }
                    }
                    else
                    {
                        requestContext.HttpContext.Response.Redirect("/" + Amana.GlobalResources.Cpanel.Lang + "/Admin/C_Login?return=" + HttpUtility.UrlEncode(requestContext.HttpContext.Request.Url.AbsoluteUri));
                    }
                }
            }
            base.Initialize(requestContext);
        }
    }
}