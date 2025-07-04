using System.Text.Json.Serialization;

using Airline.DAL;
using Airline.Database;

using AirlineAPI;
using AirlineAPI.Models;
using AirlineAPI.Services.Implementations;
using AirlineAPI.Services.Interfaces;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<AirlineContext>((options) =>
{
    options
    .UseSqlServer(builder.Configuration["ConnectionStrings:AirlineDB"])
    .UseLazyLoadingProxies();
});
builder.Services.AddTransient<DAL<Aircraft>>();

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);


builder.Services.AddScoped<IAircraftRepository, AircraftRepository>();
builder.Services.AddScoped<IRouteRepository, RouteRepository>();
builder.Services.AddScoped<ICalculateRoutePriceService, CalculateRoutePriceService>();

var app = builder.Build();
// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
