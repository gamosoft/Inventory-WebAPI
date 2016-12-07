using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Inventory.WebAPI
{
    /// <summary>
    /// Class used to define configuration values for the Web API
    /// </summary>
    public static class WebApiConfig
    {
        #region "Methods"

        /// <summary>
        /// Main registration method
        /// </summary>
        /// <param name="config">HttpConfiguration</param>
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "InventoryApi",
                routeTemplate: "api/{controller}/{label}",
                defaults: new { label = RouteParameter.Optional }
            );
        }

        #endregion "Methods"
    }
}