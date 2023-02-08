using MailService.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedLib;

namespace MailService;
// Note: actual namespace depends on the project name.

public class Program
{
    public static async Task Main(string[] args)
    {
        await CreateHostBuilder(args).Build().RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        var hostBuilder = Host.CreateDefaultBuilder(args)
            .ConfigureHostConfiguration(configHost =>
            {
                configHost.SetBasePath(Directory.GetCurrentDirectory());
                configHost.AddJsonFile("appsettings.json", false);
                configHost.AddEnvironmentVariables();
                configHost.AddCommandLine(args);
            })
            .ConfigureAppConfiguration((hostContext, configApp) =>
            {
                configApp.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", false);
            })
            // to configure all services ( like in the program.cs for asp.net apps )
            .ConfigureServices((hostContext, services) =>
            {
                var emailConfig = hostContext.Configuration
                    .GetSection("EmailConfiguration")
                    .Get<EmaiConfig>();
                services.AddSingleton(emailConfig);
                services.AddMassTransit(config =>
                {
                    config.AddConsumer<OrderSendConsumer>();
                    config.UsingRabbitMq((ctx, configTrans) =>
                    {
                        configTrans.Host(RabbitMqConstants.RmqUri, "/", configHost =>
                        {
                            configHost.Username(RabbitMqConstants.RmqUsername);
                            configHost.Password(RabbitMqConstants.RmqPassword);
                        });
                        configTrans.ReceiveEndpoint(RabbitMqConstants.OrderSendEventQueueName, configEndpoint =>
                        {
                            configEndpoint.ConfigureConsumer<OrderSendConsumer>(ctx);
                            // set the prefetch count todo find out what is really is
                            // configEndpoint.PrefetchCount = 16;
                            // configEndpoint.UseMessageRetry( configRetry => configRetry.Interval(2, TimeSpan.FromSeconds(10)));
                            ;
                        });
                    });
                });
            });


        return hostBuilder;
    }
}