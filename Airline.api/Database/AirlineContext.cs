using Airline.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Route = Airline.Models.Route;

namespace Airline.Database;

public class AirlineContext(DbContextOptions<AirlineContext> options) : IdentityDbContext<AirlineUser, IdentityRole<int>, int>(options)
{

    public DbSet<Aircraft> Aircrafts { get; set; }
    public DbSet<Route> Routes { get; set; }
    public DbSet<Flight> Flights { get; set; }
    public DbSet<Seat> Seats { get; set; }
    public DbSet<SeatClass> SeatClasses { get; set; }
    public DbSet<AirlineUser> AirlineUsers { get; set; }
    public DbSet<Ticket> Tickets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SeatClass>().HasData(
            new SeatClass { Id = 0, Description = "Economic" },
            new SeatClass { Id = 1, Description = "Executive" },
            new SeatClass { Id = 2, Description = "FirstClass" }
        );
    }

}
