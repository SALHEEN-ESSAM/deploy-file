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
    public class AuthorizeFrontController : ControllerHelper
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContextFront)
        {
            //if ((string)requestContextFront.RouteData.Values["Controller"] != "Home" && (string)requestContextFront.RouteData.Values["Action"] != "Index")
            //{
            if (requestContextFront.HttpContext.Session["UserDetails"] == null)
            {
                HttpCookie userCookie = requestContextFront.HttpContext.Request.Cookies["UserLoginDetails"];
                if (userCookie != null)
                {
                    string email = userCookie["Email"];
                    string password = StringCipher.Decrypt(userCookie["Passowrd"], true);
                    C_UserItems user = db.C_UserItems.SingleOrDefault(x => x.Email == email && x.Password == password);
                    if (user != null)
                    {
                        requestContextFront.HttpContext.Session["UserDetails"] = user;
                    }
                }
                else
                {
                    requestContextFront.HttpContext.Response.Redirect("/" + Amana.GlobalResources.Cpanel.Lang + "/User/Login/?return=" + HttpUtility.UrlEncode(requestContextFront.HttpContext.Request.Url.AbsoluteUri));
                }
            }
            //}
            base.Initialize(requestContextFront);
        }
    }
}