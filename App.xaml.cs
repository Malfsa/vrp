using System;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WpfApp2.Locator;

namespace WpfApp2 { 
    public partial class App : Application
    {

        private static IHost _Host;

        public static IHost Host => _Host
                ??= Program.CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

        public static IServiceProvider Services => Host.Services;


        internal static void ConfigureServices(HostBuilderContext host, IServiceCollection services) => services

          .AddDatabase(host.Configuration.GetSection("Database"))
           //.AddServices()
           .AddViewModels()
           ;

        protected override async void OnStartup(StartupEventArgs e)
        {

            var host = Host;

            //using (var scope = Services.CreateScope())
            // scope.ServiceProvider.GetRequiredService<DbInitializer>().InitializeAsync().Wait();

            /* using (var scope = Services.CreateScope())
             {
                 var dbInitializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
                 await dbInitializer.InitializeAsync();
             }*/



            //MainWindow main = new Autorization();
            //auth.Show();

            //base.OnStartup(e);
            base.OnStartup(e);
            await host.StartAsync();

        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using var host = Host;
            base.OnExit(e);
            await host.StopAsync();

        }
    }
}
