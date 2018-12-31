using Adapters;
using Autofac;
using Autofac.Integration.WebApi;
using Interfaces;
using Managers;
using NTicTacToe.Controllers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace NTicTacToe
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            var diContainer = BuildContainer();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(diContainer);

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        private IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            // controllers 
            builder.RegisterType<GameController>().InstancePerRequest();

            builder.RegisterType<DataManager>().As<IDataManager>().InstancePerLifetimeScope();
            builder.RegisterType<GameManagerFactory>().As<IGameManagerFactory>().InstancePerLifetimeScope();

            // data adapters
            var connectionString = ConfigurationManager.ConnectionStrings["sql-dev"].ToString();
            builder.Register(x =>
            {
                return new SqlServerAdapter(connectionString);
            }).As<IGenericDataAdapter<ITicTacToeData>>().InstancePerLifetimeScope();

            return builder.Build();
        }
    }
}
