using ComputerVisionService.Consumers;
using ComputerVisionService.Services;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedLib;

namespace ComputerVisionService;
// Note: actual namespace depends on the project name.

public class Program
{
    private static async Task Main(string[] args)
    {
        await CreateHostBuilder(args).Build().RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostcontext, services) =>
            {
                services.AddMassTransit(config =>
                {
                    config.AddConsumer<OrderRegisteredConsumer>();
                    config.UsingRabbitMq((ctx, cfg) =>
                    {
                        cfg.Host(RabbitMqConstants.RmqUri, "/", cfgHost =>
                        {
                            cfgHost.Username(RabbitMqConstants.RmqUsername);
                            cfgHost.Password(RabbitMqConstants.RmqPassword);
                        });

                        cfg.ReceiveEndpoint(RabbitMqConstants.OrderRegisteredEventQueueName,
                            configEndpoint => { configEndpoint.ConfigureConsumer<OrderRegisteredConsumer>(ctx); });
                    });
                });

                services.AddSingleton<IFaceDetectionServices, FaceDetectionServices>();
            })
            .ConfigureAppConfiguration((hostcontext, config) => { });
        return host;
    }
}