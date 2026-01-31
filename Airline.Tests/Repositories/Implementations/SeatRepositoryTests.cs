using System;
using System.Collections.Generic;
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
        List<Seat> result = (await repo.ListAsync(filters)).ToList();

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
        List<Seat> result = (await repo.ListAsync(filters)).ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task ListAsync_FiltersByIataCodes_ReturnsOnlyMatching()
    {
        // Arrange
        AirlineContext ctx = CreateContext(Guid.NewGuid().ToString());

        Airport fromA = new() { IATACode = "AAA", City = "CityA", Country = "CT", Name = "A" };
        Airport toB = new() { IATACode = "BBB", City = "CityB", Country = "CT", Name = "B" };
        Airport toC = new() { IATACode = "CCC", City = "CityC", Country = "CT", Name = "C" };
        ctx.Airports.AddRange(fromA, toB, toC);

        Route routeAB = new() { FromAirport = fromA, ToAirport = toB, Distance = 100 };
        Route routeAC = new() { FromAirport = fromA, ToAirport = toC, Distance = 200 };
        ctx.Routes.AddRange(routeAB, routeAC);

        Aircraft aircraft = new() { AircraftID = 1, Model = "A1", Capacity = 100, AverageFuelConsumption = 1.0 };
        ctx.Aircrafts.Add(aircraft);

        await ctx.SaveChangesAsync();

        // Flights on same date
        Flight flightAB = new()
        {
            Departure = new DateTimeOffset(2026, 3, 1, 10, 0, 0, TimeSpan.Zero),
            Arrival = new DateTimeOffset(2026, 3, 1, 12, 0, 0, TimeSpan.Zero),
            Aircraft = aircraft,
            Route = routeAB
        };

        Flight flightAC = new()
        {
            Departure = new DateTimeOffset(2026, 3, 1, 11, 0, 0, TimeSpan.Zero),
            Arrival = new DateTimeOffset(2026, 3, 1, 13, 0, 0, TimeSpan.Zero),
            Aircraft = aircraft,
            Route = routeAC
        };

        ctx.Flights.AddRange(flightAB, flightAC);
        await ctx.SaveChangesAsync();

        Seat seat1 = new() { SeatNumber = 10, Row = "A", IsAvailable = true, Price = 100m, SeatClass = SeatClassEnum.Economic, Flight = flightAB };
        Seat seat2 = new() { SeatNumber = 11, Row = "A", IsAvailable = true, Price = 150m, SeatClass = SeatClassEnum.Economic, Flight = flightAC };

        ctx.Seats.AddRange(seat1, seat2);
        await ctx.SaveChangesAsync();

        SeatRepository repo = new(ctx);
        SeatListFilterDTO filters = new() { FromIATACode = "AAA", ToIATACode = "BBB" };

        // Act
        List<Seat> result = (await repo.ListAsync(filters)).ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(10, result[0].SeatNumber);
    }

    [Fact]
    public async Task ListAsync_FiltersByFlightId_ReturnsSeatsForGivenFlight()
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

        Flight flight1 = new()
        {
            Departure = new DateTimeOffset(2026, 4, 1, 8, 0, 0, TimeSpan.Zero),
            Arrival = new DateTimeOffset(2026, 4, 1, 10, 0, 0, TimeSpan.Zero),
            Aircraft = aircraft,
            Route = route
        };

        Flight flight2 = new()
        {
            Departure = new DateTimeOffset(2026, 4, 2, 8, 0, 0, TimeSpan.Zero),
            Arrival = new DateTimeOffset(2026, 4, 2, 10, 0, 0, TimeSpan.Zero),
            Aircraft = aircraft,
            Route = route
        };

        ctx.Flights.AddRange(flight1, flight2);
        await ctx.SaveChangesAsync();

        Seat seat1 = new() { SeatNumber = 21, Row = "B", IsAvailable = true, Price = 100m, SeatClass = SeatClassEnum.Economic, Flight = flight1 };
        Seat seat2 = new() { SeatNumber = 22, Row = "B", IsAvailable = true, Price = 100m, SeatClass = SeatClassEnum.Economic, Flight = flight2 };

        ctx.Seats.AddRange(seat1, seat2);
        await ctx.SaveChangesAsync();

        SeatRepository repo = new(ctx);
        SeatListFilterDTO filters = new() { FlightId = flight1.FlightId };

        // Act
        var result = (await repo.ListAsync(filters)).ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(21, result[0].SeatNumber);
    }
}
