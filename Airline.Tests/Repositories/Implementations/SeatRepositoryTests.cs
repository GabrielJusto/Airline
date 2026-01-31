using System;
using System.Linq;
using System.Threading.Tasks;

using Airline.Database;
using Airline.DTO;
using Airline.Enuns;
using Airline.Models;
using Airline.Repositories.Implementations;

using Microsoft.EntityFrameworkCore;

using Xunit;

namespace Airline.Tests.Repositories.Implementations;

public class SeatRepositoryTests
{
    private static AirlineContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<AirlineContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        return new AirlineContext(options);
    }

    [Fact]
    public async Task ListAsync_ReturnsSeatsForGivenDateUtc()
    {
        // Arrange
        AirlineContext ctx = CreateContext(Guid.NewGuid().ToString());

        Airport from = new() { IATACode = "AAA", City = "CityA", Country = "CT", Name = "A" };
        Airport to = new() { IATACode = "BBB", City = "CityB", Country = "CT", Name = "B" };
        ctx.Airports.AddRange(from, to);

        Route route = new() { FromAirport = from, ToAirport = to, Distance = 100 };
        ctx.Routes.Add(route);

        Aircraft aircraft = new() { AircraftID = 1, Model = "A1", Capacity = 100, AverageFuelConsumption = 1.0 };
        ctx.Aircrafts.Add(aircraft);

        await ctx.SaveChangesAsync();

        // Flights
        Flight flightUtc = new()
        {
            Departure = new DateTimeOffset(2026, 2, 15, 10, 0, 0, TimeSpan.Zero),
            Arrival = new DateTimeOffset(2026, 2, 15, 12, 0, 0, TimeSpan.Zero),
            Aircraft = aircraft,
            Route = route
        };

        Flight flightOffset = new()
        {
            Departure = new DateTimeOffset(2026, 2, 15, 23, 0, 0, TimeSpan.FromHours(-3)),
            Arrival = new DateTimeOffset(2026, 2, 16, 1, 0, 0, TimeSpan.FromHours(-3)),
            Aircraft = aircraft,
            Route = route
        };

        Flight flightUtc2 = new()
        {
            Departure = new DateTimeOffset(2026, 2, 15, 1, 0, 0, TimeSpan.Zero),
            Arrival = new DateTimeOffset(2026, 2, 15, 3, 0, 0, TimeSpan.Zero),
            Aircraft = aircraft,
            Route = route
        };

        ctx.Flights.AddRange(flightUtc, flightOffset, flightUtc2);
        await ctx.SaveChangesAsync();

        Seat seat1 = new() { SeatNumber = 1, Row = "A", IsAvailable = true, Price = 100m, SeatClass = SeatClassEnum.Economic, Flight = flightUtc };
        Seat seat2 = new() { SeatNumber = 2, Row = "A", IsAvailable = true, Price = 150m, SeatClass = SeatClassEnum.Executive, Flight = flightOffset };
        Seat seat3 = new() { SeatNumber = 3, Row = "B", IsAvailable = true, Price = 200m, SeatClass = SeatClassEnum.Economic, Flight = flightUtc2 };

        ctx.Seats.AddRange(seat1, seat2, seat3);
        await ctx.SaveChangesAsync();

        SeatRepository repo = new(ctx);
        SeatListFilterDTO filters = new() { DepartureDate = new DateTimeOffset(2026, 2, 15, 0, 0, 0, TimeSpan.FromHours(-3)) };

        // Act
        System.Collections.Generic.List<Seat> result = (await repo.ListAsync(filters)).ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, s => s.SeatNumber == 1);
        Assert.Contains(result, s => s.SeatNumber == 2);
    }

    [Fact]
    public async Task ListAsync_ReturnsEmpty_WhenNoMatch()
    {
        // Arrange
        AirlineContext ctx = CreateContext(Guid.NewGuid().ToString());

        Airport from = new() { IATACode = "AAA", City = "CityA", Country = "CT", Name = "A" };
        Airport to = new() { IATACode = "BBB", City = "CityB", Country = "CT", Name = "B" };
        ctx.Airports.AddRange(from, to);

        Route route = new() { FromAirport = from, ToAirport = to, Distance = 100 };
        ctx.Routes.Add(route);

        Aircraft aircraft = new() { AircraftID = 1, Model = "A1", Capacity = 100, AverageFuelConsumption = 1.0 };
        ctx.Aircrafts.Add(aircraft);

        await ctx.SaveChangesAsync();

        Flight flight = new()
        {
            Departure = new DateTimeOffset(2026, 2, 14, 10, 0, 0, TimeSpan.Zero),
            Arrival = new DateTimeOffset(2026, 2, 14, 12, 0, 0, TimeSpan.Zero),
            Aircraft = aircraft,
            Route = route
        };

        ctx.Flights.Add(flight);
        await ctx.SaveChangesAsync();

        Seat seat = new() { SeatNumber = 1, Row = "A", IsAvailable = true, Price = 100m, SeatClass = SeatClassEnum.Economic, Flight = flight };
        ctx.Seats.Add(seat);
        await ctx.SaveChangesAsync();

        SeatRepository repo = new(ctx);
        SeatListFilterDTO filters = new() { DepartureDate = new DateTimeOffset(2026, 2, 15, 0, 0, 0, TimeSpan.FromHours(-3)) };

        // Act
        System.Collections.Generic.List<Seat> result = (await repo.ListAsync(filters)).ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
