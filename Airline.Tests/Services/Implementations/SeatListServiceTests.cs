using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using Airline.DTO;
using Airline.DTO.SeatDTOs;
using Airline.Enuns;
using Airline.Models;
using Airline.Repositories.Interfaces;
using Airline.Services.Implementations;

using Moq;

using Xunit;

namespace Airline.Tests.Services.Implementations;

public class SeatListServiceTests
{
    private readonly Mock<ISeatRepository> _seatRepositoryMock;
    private readonly SeatListService _service;

    public SeatListServiceTests()
    {
        _seatRepositoryMock = new Mock<ISeatRepository>();
        _service = new SeatListService(_seatRepositoryMock.Object);
    }

    #region ListAvailableSeatsForTicket Tests

    [Fact]
    public async Task ListAvailableSeatsForTicket_ShouldReturnEmptyList_WhenNoSeatsFound()
    {
        // Arrange
        SeatListFilterDTO filters = new()
        {
            FlightId = 1,
            FromIATACode = "GRU",
            ToIATACode = "GIG"
        };

        _seatRepositoryMock
            .Setup(r => r.ListAsync(filters))
            .ReturnsAsync(new List<Seat>());

        // Act
        List<SeatTicketListDTO> result = await _service.ListAvailableSeatsForTicket(filters);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        _seatRepositoryMock.Verify(r => r.ListAsync(filters), Times.Once);
    }

    [Fact]
    public async Task ListAvailableSeatsForTicket_ShouldReturnEmptyList_WhenAllSeatsAreUnavailable()
    {
        // Arrange
        SeatListFilterDTO filters = new()
        {
            FlightId = 1,
            FromIATACode = "GRU",
            ToIATACode = "GIG"
        };

        Aircraft aircraft = new()
        {
            AircraftID = 1,
            Model = "Boeing 737",
            Capacity = 180,
            AverageFuelConsumption = 2.5
        };

        Flight flight = new()
        {
            FlightId = 1,
            Departure = DateTime.UtcNow.AddHours(2),
            Arrival = DateTime.UtcNow.AddHours(4),
            Aircraft = aircraft,
            Route = new Route
            {
                FromAirport = new Airport { IATACode = "GRU", City = "São Paulo", Country = "Brazil", Name = "Guarulhos" },
                ToAirport = new Airport { IATACode = "GIG", City = "Rio de Janeiro", Country = "Brazil", Name = "Galeão" },
                Distance = 400
            }
        };

        List<Seat> seats = new()
        {
            new()
            {
                SeatId = 1,
                SeatNumber = 1,
                Row = "A",
                IsAvailable = false,
                Price = 150.00m,
                SeatClass = SeatClassEnum.Economic,
                FlightId = 1,
                Flight = flight
            },
            new()
            {
                SeatId = 2,
                SeatNumber = 2,
                Row = "A",
                IsAvailable = false,
                Price = 250.00m,
                SeatClass = SeatClassEnum.Executive,
                FlightId = 1,
                Flight = flight
            }
        };

        _seatRepositoryMock
            .Setup(r => r.ListAsync(filters))
            .ReturnsAsync(seats);

        // Act
        List<SeatTicketListDTO> result = await _service.ListAvailableSeatsForTicket(filters);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task ListAvailableSeatsForTicket_ShouldReturnOnlyAvailableSeats()
    {
        // Arrange
        SeatListFilterDTO filters = new()
        {
            FlightId = 1,
            FromIATACode = "GRU",
            ToIATACode = "GIG"
        };

        Aircraft aircraft = new()
        {
            AircraftID = 1,
            Model = "Boeing 737",
            Capacity = 180,
            AverageFuelConsumption = 2.5
        };

        Flight flight = new()
        {
            FlightId = 1,
            Departure = DateTime.UtcNow.AddHours(2),
            Arrival = DateTime.UtcNow.AddHours(4),
            Aircraft = aircraft,
            Route = new Route
            {
                FromAirport = new Airport { IATACode = "GRU", City = "São Paulo", Country = "Brazil", Name = "Guarulhos" },
                ToAirport = new Airport { IATACode = "GIG", City = "Rio de Janeiro", Country = "Brazil", Name = "Galeão" },
                Distance = 400
            }
        };

        List<Seat> seats = new()
        {
            new()
            {
                SeatId = 1,
                SeatNumber = 1,
                Row = "A",
                IsAvailable = true,
                Price = 150.00m,
                SeatClass = SeatClassEnum.Economic,
                FlightId = 1,
                Flight = flight
            },
            new()
            {
                SeatId = 2,
                SeatNumber = 2,
                Row = "A",
                IsAvailable = false,
                Price = 250.00m,
                SeatClass = SeatClassEnum.Executive,
                FlightId = 1,
                Flight = flight
            },
            new()
            {
                SeatId = 3,
                SeatNumber = 3,
                Row = "B",
                IsAvailable = true,
                Price = 300.00m,
                SeatClass = SeatClassEnum.FirstClass,
                FlightId = 1,
                Flight = flight
            }
        };

        _seatRepositoryMock
            .Setup(r => r.ListAsync(filters))
            .ReturnsAsync(seats);

        // Act
        List<SeatTicketListDTO> result = await _service.ListAvailableSeatsForTicket(filters);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.All(result, seat => Assert.True(seat != null));
    }

    [Fact]
    public async Task ListAvailableSeatsForTicket_ShouldReturnOneSeatPerPrice_WhenMultipleSameClassAvailable()
    {
        // Arrange
        SeatListFilterDTO filters = new()
        {
            FlightId = 1,
            FromIATACode = "GRU",
            ToIATACode = "GIG"
        };

        Aircraft aircraft = new()
        {
            AircraftID = 1,
            Model = "Boeing 737",
            Capacity = 180,
            AverageFuelConsumption = 2.5
        };

        Flight flight = new()
        {
            FlightId = 1,
            Departure = DateTime.UtcNow.AddHours(2),
            Arrival = DateTime.UtcNow.AddHours(4),
            Aircraft = aircraft,
            Route = new Route
            {
                FromAirport = new Airport { IATACode = "GRU", City = "São Paulo", Country = "Brazil", Name = "Guarulhos" },
                ToAirport = new Airport { IATACode = "GIG", City = "Rio de Janeiro", Country = "Brazil", Name = "Galeão" },
                Distance = 400
            }
        };

        List<Seat> seats = new()
        {
            new()
            {
                SeatId = 1,
                SeatNumber = 1,
                Row = "A",
                IsAvailable = true,
                Price = 150.00m,
                SeatClass = SeatClassEnum.Economic,
                FlightId = 1,
                Flight = flight
            },
            new()
            {
                SeatId = 2,
                SeatNumber = 2,
                Row = "A",
                IsAvailable = true,
                Price = 155.00m,
                SeatClass = SeatClassEnum.Economic,
                FlightId = 1,
                Flight = flight
            },
            new()
            {
                SeatId = 3,
                SeatNumber = 3,
                Row = "B",
                IsAvailable = true,
                Price = 250.00m,
                SeatClass = SeatClassEnum.Executive,
                FlightId = 1,
                Flight = flight
            },
            new()
            {
                SeatId = 4,
                SeatNumber = 4,
                Row = "C",
                IsAvailable = true,
                Price = 300.00m,
                SeatClass = SeatClassEnum.FirstClass,
                FlightId = 1,
                Flight = flight
            },
            new()
            {
                SeatId = 5,
                SeatNumber = 5,
                Row = "C",
                IsAvailable = true,
                Price = 310.00m,
                SeatClass = SeatClassEnum.FirstClass,
                FlightId = 1,
                Flight = flight
            }
        };

        _seatRepositoryMock
            .Setup(r => r.ListAsync(filters))
            .ReturnsAsync(seats);

        // Act
        List<SeatTicketListDTO> result = await _service.ListAvailableSeatsForTicket(filters);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result.Count);

        List<decimal> prices = result.Select(s => s.Price).ToList();
        Assert.Equal(prices.Count, prices.Distinct().Count());
    }

    [Fact]
    public async Task ListAvailableSeatsForTicket_ShouldReturnDTOWithCorrectFlightInfo()
    {
        // Arrange
        SeatListFilterDTO filters = new()
        {
            FlightId = 1,
            FromIATACode = "GRU",
            ToIATACode = "GIG"
        };

        Aircraft aircraft = new()
        {
            AircraftID = 1,
            Model = "Boeing 737",
            Capacity = 180,
            AverageFuelConsumption = 2.5
        };

        Flight flight = new()
        {
            FlightId = 1,
            Departure = new DateTime(2026, 2, 15, 10, 0, 0, DateTimeKind.Utc),
            Arrival = new DateTime(2026, 2, 15, 12, 0, 0, DateTimeKind.Utc),
            Aircraft = aircraft,
            Route = new Route
            {
                FromAirport = new Airport { IATACode = "GRU", City = "São Paulo", Country = "Brazil", Name = "Guarulhos" },
                ToAirport = new Airport { IATACode = "GIG", City = "Rio de Janeiro", Country = "Brazil", Name = "Galeão" },
                Distance = 400
            }
        };

        Seat seat = new()
        {
            SeatId = 1,
            SeatNumber = 1,
            Row = "A",
            IsAvailable = true,
            Price = 150.00m,
            SeatClass = SeatClassEnum.Economic,
            FlightId = 1,
            Flight = flight
        };

        _seatRepositoryMock
            .Setup(r => r.ListAsync(filters))
            .ReturnsAsync(new List<Seat> { seat });

        // Act
        List<SeatTicketListDTO> result = await _service.ListAvailableSeatsForTicket(filters);

        // Assert
        Assert.NotEmpty(result);
        SeatTicketListDTO dto = result.First();

        Assert.Equal("GRU", dto.FromIATACode);
        Assert.Equal("São Paulo", dto.FromCity);
        Assert.Equal("GIG", dto.ToIATACode);
        Assert.Equal("Rio de Janeiro", dto.ToCity);
        Assert.Equal(150.00m, dto.Price);
        Assert.Equal(SeatClassEnum.Economic.ToString(), dto.SeatClass);
    }

    [Fact]
    public async Task ListAvailableSeatsForTicket_ThrowsValidationException_WhenFromIataIsNull()
    {
        // Arrange
        SeatListFilterDTO filters = new()
        {
            FromIATACode = null,
            ToIATACode = "GIG"
        };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _service.ListAvailableSeatsForTicket(filters));
    }

    [Fact]
    public async Task ListAvailableSeatsForTicket_ThrowsValidationException_WhenToIataIsNull()
    {
        // Arrange
        SeatListFilterDTO filters = new()
        {
            FromIATACode = "GRU",
            ToIATACode = null
        };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _service.ListAvailableSeatsForTicket(filters));
    }

    #endregion
}
