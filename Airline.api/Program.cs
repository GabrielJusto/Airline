

using System.Diagnostics;
using System.Text.Json.Serialization;

using Airline.Configuration;
using Airline.Database;
using Airline.Exceptions.Handler;
using Airline.Models;
using Airline.Repositories.Implementations;
using Airline.Repositories.Interfaces;
using Airline.Services.Implementations;
using Airline.Services.Interfaces;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .WithOrigins("http://localhost:8081")
            .AllowAnyMethod()
            .AllowAnyHeader()

    );
});

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
builder.Services.AddScoped<IFlightService, FlightService>();
builder.Services.AddScoped<ISeatCreateService, SeatCreateService>();
builder.Services.AddScoped<ISeatRepository, SeatRepository>();
builder.Services.AddScoped<ISeatService, SeatService>();
builder.Services.AddScoped<TicketPurchaseService>();
builder.Services.AddScoped<TicketRepository>();
builder.Services.AddScoped<AirlineUserRepository>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IAirportRepository, AirportRepository>();
builder.Services.AddScoped<IAirportService, AirportService>();
builder.Services.AddScoped<IAircraftService, AircraftService>();

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddProblemDetails(options =>
{
    // Applied to every problem details response, including the ones the framework writes by itself.
    options.CustomizeProblemDetails = context =>
    {
        // Path only, without the query string: it carries the device api key.
        context.ProblemDetails.Instance = context.HttpContext.Request.Path;

        // Same value that Serilog stamps on the log entries, so a reported id finds the trace.
        context.ProblemDetails.Extensions["traceId"] =
            Activity.Current?.TraceId.ToString() ?? context.HttpContext.TraceIdentifier;
    };
});

var app = builder.Build();

app.UseExceptionHandler();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseCors("AllowFrontend");

app.Run();
