

using System.Text.Json.Serialization;

using Airline.Configuration;
using Airline.Database;
using Airline.Models;
using Airline.Repositories.Implementations;
using Airline.Repositories.Interfaces;
using Airline.Services.Implementations;
using Airline.Services.Interfaces;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<AirlineContext>(options =>
    options.UseNpgsql(builder.Configuration["ConnectionStrings:AirlineDB"]));


builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddIdentity<AirlineUser, IdentityRole<int>>()
    .AddEntityFrameworkStores<AirlineContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireDigit = true;
});


builder.Services.AddScoped<IAircraftRepository, AircraftRepository>();
builder.Services.AddScoped<IRouteRepository, RouteRepository>();
builder.Services.AddScoped<IRouteService, RouteService>();
builder.Services.AddScoped<IFlightRepository, FlightRepository>();
builder.Services.AddScoped<IFlightCreateService, FlightCreateService>();
builder.Services.AddScoped<IFlightDetailService, FlightDetailService>();
builder.Services.AddScoped<ISeatCreateService, SeatCreateService>();
builder.Services.AddScoped<ISeatRepository, SeatRepository>();
builder.Services.AddScoped<SeatListService>();
builder.Services.AddScoped<TicketPurchaseService>();
builder.Services.AddScoped<TicketRepository>();
builder.Services.AddScoped<AirlineUserRepository>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IAirportRepository, AirportRepository>();
builder.Services.AddScoped<IAirportService, AirportService>();

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection("Jwt"));

var app = builder.Build();


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
