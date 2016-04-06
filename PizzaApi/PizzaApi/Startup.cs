using Newtonsoft.Json.Serialization;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;
using Swashbuckle.Application;

namespace PizzaApi
{
    public class Startup
    {
        HttpConfiguration _configuration;

        public Startup()
        {
            _configuration = new HttpConfiguration();

        }

        public void Configuration(IAppBuilder app)
        {
            ConfigureWebApi();
            ConfigureJsonFormatter();

            _configuration.EnableSwagger(c =>
            {
                c.SingleApiVersion("v1", "MassTransit Configuration Sample");
                c.IncludeXmlComments(GetXmlCommentsPath());
            }).EnableSwaggerUi();

            app.UseWebApi(_configuration);
        }

        protected static string GetXmlCommentsPath()
        {
            return String.Format(@"{0}\PizzaApi.XML",
                    AppDomain.CurrentDomain.BaseDirectory);
        }

        private void ConfigureWebApi()
        {
            _configuration = new HttpConfiguration();
            _configuration.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional });

            _configuration.Routes.MapHttpRoute(
                name: "PostChild",
                routeTemplate: "api/{controller}/{id}/{action}",
                defaults: new { id = RouteParameter.Optional },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Post) }
            );

            _configuration.Routes.MapHttpRoute(
                name: "PutChild",
                routeTemplate: "api/{controller}/{id}/{action}",
                defaults: new { id = RouteParameter.Optional },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Put) }
            );
        }

        private void ConfigureJsonFormatter()
        {
            _configuration.Formatters.Clear();
            _configuration.Formatters.Add(new JsonMediaTypeFormatter());
            _configuration.Formatters.JsonFormatter.SerializerSettings =
                new Newtonsoft.Json.JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
        }
    }
}
