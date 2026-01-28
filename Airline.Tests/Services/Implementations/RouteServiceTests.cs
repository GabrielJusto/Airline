using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using Airline.DTO;
using Airline.Exceptions;
using Airline.Models;
using Airline.Repositories.Interfaces;
using Airline.RequestBodies;
using Airline.Services.Implementations;
using Airline.Services.Interfaces;

using Moq;

using Xunit;

using Route = Airline.Models.Route;

namespace Airline.Tests.Services.Implementations;

public class RouteServiceTests
{
    private readonly Mock<IRouteRepository> _routeRepositoryMock;
    private readonly Mock<IAirportService> _airportServiceMock;
    private readonly RouteService _service;

    public RouteServiceTests()
    {
        _routeRepositoryMock = new Mock<IRouteRepository>();
        _airportServiceMock = new Mock<IAirportService>();
        _service = new RouteService(_routeRepositoryMock.Object, _airportServiceMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnRouteId_WhenDataIsValid()
    {
        // Arrange
        RouteInsertRequestBody routeData = new(
            FromAirportId: 1,
            ToAirportId: 2,
            Distance: 1500.0
        );

        int expectedRouteId = 1;

        Airport fromAirport = new() { AirportId = 1, IATACode = "GRU", Name = "São Paulo", City = "São Paulo", Country = "Brazil" };
        Airport toAirport = new() { AirportId = 2, IATACode = "GIG", Name = "Rio de Janeiro", City = "Rio de Janeiro", Country = "Brazil" };

        _airportServiceMock
            .Setup(s => s.GetAirportByIdAsync(1))
            .ReturnsAsync(fromAirport);

        _airportServiceMock
            .Setup(s => s.GetAirportByIdAsync(2))
            .ReturnsAsync(toAirport);

        _routeRepositoryMock
            .Setup(r => r.List(It.IsAny<RouteListFiltersDTO>()))
            .Returns(new System.Collections.Generic.List<RouteListDTO>());

        _routeRepositoryMock
            .Setup(r => r.InsertAsync(It.IsAny<Route>()))
            .ReturnsAsync(expectedRouteId);

        // Act
        int result = await _service.CreateAsync(routeData);

        // Assert
        Assert.Equal(expectedRouteId, result);
        _routeRepositoryMock.Verify(r => r.InsertAsync(It.IsAny<Route>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenFromAirportNotFound()
    {
        // Arrange
        RouteInsertRequestBody routeData = new(
            FromAirportId: 999,
            ToAirportId: 2,
            Distance: 1500.0
        );

        _airportServiceMock
            .Setup(s => s.GetAirportByIdAsync(999))
            .ReturnsAsync((Airport)null);

        _routeRepositoryMock
            .Setup(r => r.List(It.IsAny<RouteListFiltersDTO>()))
            .Returns(new System.Collections.Generic.List<RouteListDTO>());

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _service.CreateAsync(routeData)
        );

        _routeRepositoryMock.Verify(r => r.InsertAsync(It.IsAny<Route>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenToAirportNotFound()
    {
        // Arrange
        RouteInsertRequestBody routeData = new(
            FromAirportId: 1,
            ToAirportId: 999,
            Distance: 1500.0
        );

        Airport fromAirport = new() { AirportId = 1, IATACode = "GRU", Name = "São Paulo", City = "São Paulo", Country = "Brazil" };

        _airportServiceMock
            .Setup(s => s.GetAirportByIdAsync(1))
            .ReturnsAsync(fromAirport);

        _airportServiceMock
            .Setup(s => s.GetAirportByIdAsync(999))
            .ReturnsAsync((Airport)null);

        _routeRepositoryMock
            .Setup(r => r.List(It.IsAny<RouteListFiltersDTO>()))
            .Returns(new System.Collections.Generic.List<RouteListDTO>());

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _service.CreateAsync(routeData)
        );

        _routeRepositoryMock.Verify(r => r.InsertAsync(It.IsAny<Route>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenRouteAlreadyExists()
    {
        // Arrange
        RouteInsertRequestBody routeData = new(
            FromAirportId: 1,
            ToAirportId: 2,
            Distance: 1500.0
        );

        Airport fromAirport = new() { AirportId = 1, IATACode = "GRU", Name = "São Paulo", City = "São Paulo", Country = "Brazil" };
        Airport toAirport = new() { AirportId = 2, IATACode = "GIG", Name = "Rio de Janeiro", City = "Rio de Janeiro", Country = "Brazil" };

        _airportServiceMock
            .Setup(s => s.GetAirportByIdAsync(1))
            .ReturnsAsync(fromAirport);

        _airportServiceMock
            .Setup(s => s.GetAirportByIdAsync(2))
            .ReturnsAsync(toAirport);

        _routeRepositoryMock
            .Setup(r => r.List(It.IsAny<RouteListFiltersDTO>()))
            .Returns(new System.Collections.Generic.List<RouteListDTO>
            {
                new(RouteID: 1)
            });

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(
            () => _service.CreateAsync(routeData)
        );

        _routeRepositoryMock.Verify(r => r.InsertAsync(It.IsAny<Route>()), Times.Never);
    }

}
