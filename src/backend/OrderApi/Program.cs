using Microsoft.EntityFrameworkCore;
using OrderApi.Data;
using OrderApi.Mapping;
using OrderApi.Models;
using OrderApi.Repository;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("orderDbConnectionString");
var frontEndUrl = builder.Configuration.GetValue<string>("frontend_url");

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(
    // configuring the context to use the postgres provider
    options => options.UseNpgsql(connectionString) 
);
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddSingleton<IMapper, Mapper>();

// registering and configuring asp.net.cors services to allow api call from others url or port
builder.Services.AddCors(op => op.AddPolicy("cors-policy", policyBuilder =>
{
    policyBuilder.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod();
}));

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