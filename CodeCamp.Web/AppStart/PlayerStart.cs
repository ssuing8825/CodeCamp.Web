using System;
using System.Configuration;
using Microsoft.ApplicationServer.Http.Activation;
using Microsoft.ApplicationServer.Http.Description;
using System.Web.Routing;
using CodeCamp.Web.Api;
using StructureMap;
using CodeCamp.Web.Repository;
using Microsoft.ApplicationServer.Http;

[assembly: WebActivator.PreApplicationStartMethod(typeof(CodeCamp.Web.AppStart.ApiStart), "Start")]
namespace CodeCamp.Web.AppStart
{
    public class ApiStart
    {
        public static void Start()
        {

            // Set up StructureMap Container
            ObjectFactory.Initialize(c =>
                {
                    c.For<IPlayerRepository>().Singleton().Use<PlayerRepository>();
                });

            var iocContainer = ObjectFactory.Container;

            // Get Configuration for Web API
            var config = new WebApiConfiguration
            {
                EnableTestClient = true,
                EnableHelpPage = true,
                IncludeExceptionDetail = true,
            };

            //Set the delegate for creating the instance of the service
            config.CreateInstance = (serviceType, context, request) =>
            {
                if (serviceType == null)
                {
                    return null;
                }

                try
                {
                    return serviceType.IsAbstract || serviceType.IsInterface
                               ? iocContainer.TryGetInstance(serviceType)
                               : iocContainer.GetInstance(serviceType);
                }
                catch (System.Exception)
                {
                    return null;
                }
            };

            //Set the configuration for all the services
            RouteTable.Routes.SetDefaultHttpConfiguration(config);
            RouteTable.Routes.MapServiceRoute<PlayersApi>("Players");
        }
    }
}