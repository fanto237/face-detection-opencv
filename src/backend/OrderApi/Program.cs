using System.Text;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OrderApi.Consumers;
using OrderApi.Data;
using OrderApi.Mapping;
using OrderApi.Repository;
using OrderApi.Services;
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
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IFaceRepository, FaceRepository>();
builder.Services.AddScoped<IFileProcessingService, FileProcessingService>();
builder.Services.AddAutoMapper(typeof(MapperProfile));

// configuring the authentication and authorization
builder.Services.AddAuthentication(
    options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }
).AddJwtBearer(
        options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer= builder.Configuration["Jwt:Issuer"],
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Secret"])),
                ValidateIssuerSigningKey = true
            };
        }
    );

// registering and configuring asp.net.cors services to allow api call from others url or port
builder.Services.AddCors(op => op.AddPolicy("cors-policy", policyBuilder =>
{
    // policyBuilder.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod();
    policyBuilder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
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

        configTrans.ReceiveEndpoint(RabbitMqConstants.OrderProcessedEventQueueName,
            configEndpoint => { configEndpoint.ConfigureConsumer<OrderProcessedConsumer>(context); });

        configTrans.ReceiveEndpoint(RabbitMqConstants.OrderSentEventQueueName,
            configEp => { configEp.ConfigureConsumer<OrderSentConsumer>(context); });
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme.  \r\n\r\n" +
                          "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
                          "Example: \"Bearer 123\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Scheme = "Bearer",
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                },
                new List<string>()
            }
        });
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// adding a middleware for using asp.net.cors
app.UseCors("cors-policy");

// app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();