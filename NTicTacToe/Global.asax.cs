﻿using Adapters;
using Autofac;
using Autofac.Integration.WebApi;
using Interfaces;
using Managers;
using NTicTacToe.Controllers;
using StackExchange.Redis;
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

            bool isUsingCache;
            var isUsingCacheSetting = ConfigurationManager.AppSettings["useCache"];
            if (bool.TryParse(isUsingCacheSetting, out isUsingCache) && isUsingCache)
            {
                // data adapters
                builder.Register(x =>
                {
                    var redisConnectionString = ConfigurationManager.ConnectionStrings["redis-azure"].ToString();
                    return ConnectionMultiplexer.Connect(redisConnectionString);
                }).As<ConnectionMultiplexer>().SingleInstance();

                builder.Register(x =>
                {
                    var sqlConnectionString = ConfigurationManager.ConnectionStrings["sql-azure"].ToString();
                    var sqlAdapter = new SqlServerAdapter(sqlConnectionString);
                    var connectionMultiplexer = x.Resolve<ConnectionMultiplexer>();
                    var redisAdapter = new RedisAdapter(connectionMultiplexer);
                    return new SqlServerCachedAdapter(sqlAdapter, redisAdapter);
                }).As<IGenericDataAdapter<ITicTacToeData>>().InstancePerLifetimeScope();
            }
            else
            {
                builder.Register(x =>
                {
                    var sqlConnectionString = ConfigurationManager.ConnectionStrings["sql-azure"].ToString();
                    return new SqlServerAdapter(sqlConnectionString);
                }).As<IGenericDataAdapter<ITicTacToeData>>().InstancePerLifetimeScope();
            }

            

            return builder.Build();
        }
    }
}
