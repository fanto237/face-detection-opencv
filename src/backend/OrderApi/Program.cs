using MassTransit.MultiBus;
using Microsoft.EntityFrameworkCore;
using OrderApi.Data;
using OrderApi.Mapping;
using MassTransit;
using OrderApi.Consumers;
using OrderApi.Repository;
using SharedLib;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("orderDbConnectionString");

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(
    // configuring the context to use the postgres provider
    options => options.UseNpgsql(connectionString) 
);
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IFaceRepository, FaceRepository>();
builder.Services.AddSingleton<IMapper, Mapper>();

// registering and configuring asp.net.cors services to allow api call from others url or port
builder.Services.AddCors(op => op.AddPolicy("cors-policy", policyBuilder =>
{
    policyBuilder.WithOrigins("https://metavision.fantodev.com/").AllowAnyHeader().AllowAnyMethod();
}));

builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<OrderProcessedConsumer>();
    config.AddConsumer<OrderSentConsumer>();
    config.UsingRabbitMq((context, configTrans) =>
    {
        configTrans.Host(RabbitMqConstants.RmqUri, "/", configHost =>
        {
            configHost.Username(RabbitMqConstants.RmqUsername);
            configHost.Password(RabbitMqConstants.RmqPassword);
        });
        
        configTrans.ReceiveEndpoint(RabbitMqConstants.OrderProcessedEventQueueName, configEndpoint =>
        {
            configEndpoint.ConfigureConsumer<OrderProcessedConsumer>(context);
        });
        
        configTrans.ReceiveEndpoint(RabbitMqConstants.OrderSentEventQueueName, configEp =>
        {
            configEp.ConfigureConsumer<OrderSentConsumer>(context);
        });
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// adding a middleware for using asp.net.cors
app.UseCors("cors-policy");

app.UseAuthorization();

app.MapControllers();

app.Run();