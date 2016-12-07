using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace Inventory.WebAPI
{
    /// <summary>
    /// Main application
    /// </summary>
    public class WebApiApplication : System.Web.HttpApplication
    {
        #region "Methods"

        /// <summary>
        /// Executed when the application starts
        /// </summary>
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        #endregion "Methods"
    }
}