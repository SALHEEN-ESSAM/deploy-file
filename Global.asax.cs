using System.Web.Mvc;
using System.Web.Routing;

namespace Amana
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //this resources for system validation data type 'date, integer etc...'
            ClientDataTypeModelValidatorProvider.ResourceClassKey = "SystemValidation";
            DefaultModelBinder.ResourceClassKey = "SystemValidation";
        }
    }
}
