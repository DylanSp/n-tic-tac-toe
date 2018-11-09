using Autofac;
using Data;
using Interfaces;
using Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameConsole
{
    class Program
    {
        private static IContainer Container { get; set; }
        static void Main(string[] args)
        {
            ConfigureAutofac();
            using (var scope = Container.BeginLifetimeScope())
            {
                var manager = scope.Resolve<IDataManager>();
                manager.CreateAndSaveGame();
                var startingData = manager.GetGameData();
                Console.WriteLine(startingData);
            }
        }

        private static void ConfigureAutofac()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<TicTacToeData>().As<ITicTacToeData>();
            builder.RegisterType<TTTDataAdapter>().As<IGenericDataAdapter<ITicTacToeData>>();
            builder.RegisterType<GameManager>().As<IGameManager>();
            builder.RegisterType<DataManager>().As<IDataManager>();
            Container = builder.Build();
        }
    }
}
