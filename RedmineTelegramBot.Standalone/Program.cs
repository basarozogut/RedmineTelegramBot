using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RedmineTelegramBot.Core;
using RedmineTelegramBot.Core.Config;
using RedmineTelegramBot.Core.Modules;
using Serilog;
using System;
using System.Threading.Tasks;

namespace RedmineTelegramBot.Standalone
{
    class Program
    {
        public static IConfigurationRoot Configuration;

        static async Task Main(string[] args)
        {
            // Initialize serilog logger
            Log.Logger = new LoggerConfiguration()
                 .WriteTo.Console(Serilog.Events.LogEventLevel.Debug)
                 .MinimumLevel.Debug()
                 .Enrich.FromLogContext()
                 .CreateLogger();

            //setup our DI
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
                
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var bot = serviceProvider.GetRequiredService<IRedmineBot>();
            await bot.Start();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            await bot.Stop();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // Add logging
            services.AddSingleton(LoggerFactory.Create(builder =>
            {
                builder
                    .AddSerilog(dispose: true);
            }));

            services.AddLogging();

            BuildConfiguration();

            services.RegisterModule(new RedmineBotModule(Configuration.GetSection("TelegramBot").Get<BotOptions>()));
        }

        private static void BuildConfiguration()
        {
            var devEnvironmentVariable = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");

            var isDevelopment = string.IsNullOrEmpty(devEnvironmentVariable) ||
                                devEnvironmentVariable.ToLower() == "development";
            //Determines the working environment as IHostingEnvironment is unavailable in a console app

            var builder = new ConfigurationBuilder();
            // tell the builder to look for the appsettings.json file
            builder
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            //only add secrets in development
            if (isDevelopment)
            {
                builder.AddUserSecrets<Program>();
            }

            Configuration = builder.Build();
        }
    }
}
