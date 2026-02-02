using System.Collections.Generic;
using System.Threading.Tasks;

using Airline.DTO.AirportDTOs;
using Airline.Exceptions;
using Airline.Models;
using Airline.Repositories.Interfaces;
using Airline.Services.Implementations;

using Moq;

using Xunit;

namespace Airline.Tests.Services.Implementations;

public class AirportServiceTests
{
    private readonly Mock<IAirportRepository> _airportRepositoryMock;
    private readonly AirportService _service;

    public AirportServiceTests()
    {
        _airportRepositoryMock = new Mock<IAirportRepository>();
        _service = new AirportService(_airportRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateAirport_ShouldReturnAirportId_WhenDataIsValid()
    {
        // Arrange
        AirportCreateDTO airportCreateDto = new(
            "GRU",
            "São Paulo International Airport",
            "São Paulo",
            "Brazil",
            "Brasilia Standard Time"
        );

        int expectedAirportId = 1;

        _airportRepositoryMock
            .Setup(r => r.InsertAirportAsync(It.IsAny<Airport>()))
            .ReturnsAsync(expectedAirportId);

        // Act
        int result = await _service.CreateAirport(airportCreateDto);

        // Assert
        Assert.Equal(expectedAirportId, result);
        _airportRepositoryMock.Verify(r => r.InsertAirportAsync(It.IsAny<Airport>()), Times.Once);
    }

    [Fact]
    public async Task CreateAirport_ShouldThrow_WhenIATACodeLengthIsNotThree()
    {
        // Arrange
        AirportCreateDTO airportCreateDto = new(
            "GRUSP",
            "São Paulo International Airport",
            "São Paulo",
            "Brazil",
            "Brasilia Standard Time"
        );

        // Act & Assert
        await Assert.ThrowsAsync<ValidationServiceException>(
            () => _service.CreateAirport(airportCreateDto)
        );

        _airportRepositoryMock.Verify(r => r.InsertAirportAsync(It.IsAny<Airport>()), Times.Never);
    }

    [Fact]
    public async Task CreateAirport_ShouldThrow_WhenIATACodeIsNotUppercase()
    {
        // Arrange
        AirportCreateDTO airportCreateDto = new(
            "gru",
            "São Paulo International Airport",
            "São Paulo",
            "Brazil",
            "Brasilia Standard Time"
        );

        // Act & Assert
        await Assert.ThrowsAsync<ValidationServiceException>(
            () => _service.CreateAirport(airportCreateDto)
        );

        _airportRepositoryMock.Verify(r => r.InsertAirportAsync(It.IsAny<Airport>()), Times.Never);
    }

    [Fact]
    public async Task CreateAirport_ShouldThrow_WhenIATACodeContainsNonLetters()
    {
        // Arrange
        AirportCreateDTO airportCreateDto = new(
            "GR1",
            "São Paulo International Airport",
            "São Paulo",
            "Brazil",
            "Brasilia Standard Time"
        );

        // Act & Assert
        await Assert.ThrowsAsync<ValidationServiceException>(
            () => _service.CreateAirport(airportCreateDto)
        );

        _airportRepositoryMock.Verify(r => r.InsertAirportAsync(It.IsAny<Airport>()), Times.Never);
    }

    [Fact]
    public async Task CreateAirport_ShouldCallRepositoryInsertAirport_WithCorrectAirportObject()
    {
        // Arrange
        AirportCreateDTO airportCreateDto = new(
            "JPA",
            "João Pessoa International Airport",
            "João Pessoa",
            "Brazil",
            "Brasilia Standard Time"
        );

        _airportRepositoryMock
            .Setup(r => r.InsertAirportAsync(It.IsAny<Airport>()))
            .ReturnsAsync(5);

        // Act
        await _service.CreateAirport(airportCreateDto);

        // Assert
        _airportRepositoryMock.Verify(
            r => r.InsertAirportAsync(It.Is<Airport>(a =>
                a.IATACode == "JPA" &&
                a.Name == "João Pessoa International Airport" &&
                a.City == "João Pessoa" &&
                a.Country == "Brazil" &&
                a.TimeZoneName == "Brasilia Standard Time"
            )),
            Times.Once
        );
    }

    [Fact]
    public async Task ListAirports_ShouldReturnListOfAirports_WithoutFilters()
    {
        // Arrange
        AirportListFilters filters = new(null, null);

        List<Airport> expectedAirports = new()
        {
            new Airport { AirportId = 1, IATACode = "GRU", Name = "São Paulo International Airport", City = "São Paulo", Country = "Brazil" },
            new Airport { AirportId = 2, IATACode = "GIG", Name = "Rio de Janeiro International Airport", City = "Rio de Janeiro", Country = "Brazil" },
            new Airport { AirportId = 3, IATACode = "JFK", Name = "John F. Kennedy International Airport", City = "New York", Country = "United States" }
        };

        _airportRepositoryMock
            .Setup(r => r.ListAirportsAsync(It.IsAny<AirportListFilters>()))
            .ReturnsAsync(expectedAirports);

        // Act
        List<AirportListDetailDTO> result = await _service.ListAirportsAsync(filters);

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal(expectedAirports.Count, result.Count);
        Assert.All(result, a => Assert.NotNull(a));
        _airportRepositoryMock.Verify(r => r.ListAirportsAsync(filters), Times.Once);
    }

    [Fact]
    public async Task ListAirports_ShouldReturnFilteredAirports_ByCity()
    {
        // Arrange
        AirportListFilters filters = new("São Paulo", null);

        List<Airport> expectedAirports = new()
        {
            new Airport { AirportId = 1, IATACode = "GRU", Name = "São Paulo International Airport", City = "São Paulo", Country = "Brazil" }
        };

        _airportRepositoryMock
            .Setup(r => r.ListAirportsAsync(It.IsAny<AirportListFilters>()))
            .ReturnsAsync(expectedAirports);

        // Act
        List<AirportListDetailDTO> result = await _service.ListAirportsAsync(filters);

        // Assert
        Assert.Single(result);
        Assert.Equal("São Paulo", result[0].City);
        _airportRepositoryMock.Verify(r => r.ListAirportsAsync(filters), Times.Once);
    }

    [Fact]
    public async Task ListAirports_ShouldReturnFilteredAirports_ByCountry()
    {
        // Arrange
        AirportListFilters filters = new(null, "Brazil");

        List<Airport> expectedAirports = new()
        {
            new Airport { AirportId = 1, IATACode = "GRU", Name = "São Paulo International Airport", City = "São Paulo", Country = "Brazil" },
            new Airport { AirportId = 2, IATACode = "GIG", Name = "Rio de Janeiro International Airport", City = "Rio de Janeiro", Country = "Brazil" }
        };

        _airportRepositoryMock
            .Setup(r => r.ListAirportsAsync(It.IsAny<AirportListFilters>()))
            .ReturnsAsync(expectedAirports);

        // Act
        List<AirportListDetailDTO> result = await _service.ListAirportsAsync(filters);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, a => Assert.Equal("Brazil", a.Country));
        _airportRepositoryMock.Verify(r => r.ListAirportsAsync(filters), Times.Once);
    }

    [Fact]
    public async Task ListAirports_ShouldReturnEmptyList_WhenNoAirportsFound()
    {
        // Arrange
        AirportListFilters filters = new("NonExistentCity", null);

        List<Airport> expectedAirports = new();

        _airportRepositoryMock
            .Setup(r => r.ListAirportsAsync(It.IsAny<AirportListFilters>()))
            .ReturnsAsync(expectedAirports);

        // Act
        List<AirportListDetailDTO> result = await _service.ListAirportsAsync(filters);

        // Assert
        Assert.Empty(result);
        _airportRepositoryMock.Verify(r => r.ListAirportsAsync(filters), Times.Once);
    }

    [Fact]
    public async Task ListAirports_ShouldReturnFilteredAirports_ByBothCityAndCountry()
    {
        // Arrange
        AirportListFilters filters = new("São Paulo", "Brazil");

        List<Airport> expectedAirports = new()
        {
            new Airport { AirportId = 1, IATACode = "GRU", Name = "São Paulo International Airport", City = "São Paulo", Country = "Brazil" }
        };

        _airportRepositoryMock
            .Setup(r => r.ListAirportsAsync(It.IsAny<AirportListFilters>()))
            .ReturnsAsync(expectedAirports);

        // Act
        List<AirportListDetailDTO> result = await _service.ListAirportsAsync(filters);

        // Assert
        Assert.Single(result);
        Assert.Equal("São Paulo", result[0].City);
        Assert.Equal("Brazil", result[0].Country);
        _airportRepositoryMock.Verify(r => r.ListAirportsAsync(filters), Times.Once);
    }
}
