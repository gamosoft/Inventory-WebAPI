﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Inventory.WebAPI
{
    public static class WebApiConfig
    {
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
    }
}