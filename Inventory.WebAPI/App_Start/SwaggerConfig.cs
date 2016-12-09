using System.Web.Http;
using WebActivatorEx;
using Inventory.WebAPI;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace Inventory.WebAPI
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration 
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "Inventory.WebAPI");
                        c.DescribeAllEnumsAsStrings();
                    })
                .EnableSwaggerUi(c =>
                    {
                        c.DocExpansion(DocExpansion.List);
                    });
        }
    }
}