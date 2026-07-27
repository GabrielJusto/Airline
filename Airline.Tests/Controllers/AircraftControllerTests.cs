using System.Collections.Generic;
using System.Threading.Tasks;

using Airline.Controllers;
using Airline.DTO;
using Airline.Exceptions;
using Airline.Models;
using Airline.Repositories.Interfaces;
using Airline.RequestBodies;

using Microsoft.AspNetCore.Http;

using Moq;

using Xunit;

namespace Airline.Tests.Controllers;

public class AircraftControllerTests
{
    private readonly Mock<IAircraftRepository> _aircraftRepositoryMock;
    private readonly AircraftController _controller;

    public AircraftControllerTests()
    {
        _aircraftRepositoryMock = new Mock<IAircraftRepository>();
        _controller = new AircraftController(_aircraftRepositoryMock.Object);
    }

    private static int GetStatusCode(IResult result)
    {
        IStatusCodeHttpResult statusResult = Assert.IsAssignableFrom<IStatusCodeHttpResult>(result);
        return statusResult.StatusCode ?? 0;
    }

    [Fact]
    public void Create_ShouldCallRepositoryInsert_AndReturnCreated()
    {
        // Arrange
        AircraftCreateDTO aircraftCreateDto = new(
            Model: "Boeing 737",
            Capacity: 180,
            Range: 5000.0,
            AverageFuelConsumption: 2500.0
        );

        // Act
        IResult result = _controller.Create(aircraftCreateDto);

        // Assert
        Assert.Equal(StatusCodes.Status201Created, GetStatusCode(result));
        _aircraftRepositoryMock.Verify(r => r.Insert(aircraftCreateDto), Times.Once);
    }

    [Fact]
    public void Create_ShouldPassCorrectDataToRepository()
    {
        // Arrange
        AircraftCreateDTO aircraftCreateDto = new("Boeing 787", 242, 14800.0, 5400.0);

        // Act
        _controller.Create(aircraftCreateDto);

        // Assert
        _aircraftRepositoryMock.Verify(
            r => r.Insert(It.Is<AircraftCreateDTO>(d =>
                d.Model == "Boeing 787" &&
                d.Capacity == 242 &&
                d.Range == 14800.0 &&
                d.AverageFuelConsumption == 5400.0
            )),
            Times.Once
        );
    }

    [Fact]
    public void List_ShouldReturnOk_WithAircraftsFromRepository()
    {
        // Arrange
        List<Aircraft> aircrafts = new()
        {
            new Aircraft { AircraftID = 1, Model = "Boeing 737", Capacity = 180, Range = 5000.0, AverageFuelConsumption = 2500.0 },
            new Aircraft { AircraftID = 2, Model = "Airbus A320", Capacity = 150, Range = 6000.0, AverageFuelConsumption = 2300.0 }
        };

        _aircraftRepositoryMock
            .Setup(r => r.ListAircrafts())
            .Returns(aircrafts);

        // Act
        IResult result = _controller.List();

        // Assert
        Assert.Equal(StatusCodes.Status200OK, GetStatusCode(result));
        _aircraftRepositoryMock.Verify(r => r.ListAircrafts(), Times.Once);
    }

    [Fact]
    public void Detail_ShouldReturnOk_WhenAircraftExists()
    {
        // Arrange
        Aircraft aircraft = new() { AircraftID = 1, Model = "Boeing 737", Capacity = 180, Range = 5000.0, AverageFuelConsumption = 2500.0 };

        _aircraftRepositoryMock
            .Setup(r => r.GetAircraft(1))
            .Returns(aircraft);

        // Act
        IResult result = _controller.Detail(1);

        // Assert
        Assert.Equal(StatusCodes.Status200OK, GetStatusCode(result));
        _aircraftRepositoryMock.Verify(r => r.GetAircraft(1), Times.AtLeastOnce);
    }

    [Fact]
    public void Detail_ShouldReturnNotFound_WhenAircraftDoesNotExist()
    {
        // Arrange
        _aircraftRepositoryMock
            .Setup(r => r.GetAircraft(It.IsAny<int>()))
            .Returns((Aircraft)null!);

        // Act
        IResult result = _controller.Detail(999);

        // Assert
        Assert.Equal(StatusCodes.Status404NotFound, GetStatusCode(result));
    }

    [Fact]
    public void Update_ShouldReturnOk_WhenAircraftExists()
    {
        // Arrange
        AircraftUpdateRequestBody updateBody = new(Capacity: 200, Range: 6000, AverageFuelConsumption: 2400.0);

        _aircraftRepositoryMock
            .Setup(r => r.Update(It.IsAny<AircraftUpdateDTO>()));

        // Act
        IResult result = _controller.Update(updateBody, 1);

        // Assert
        Assert.Equal(StatusCodes.Status200OK, GetStatusCode(result));
        _aircraftRepositoryMock.Verify(r => r.Update(It.IsAny<AircraftUpdateDTO>()), Times.Once);
    }

    [Fact]
    public void Update_ShouldReturnNotFound_WhenRepositoryThrowsEntityNotFound()
    {
        // Arrange
        AircraftUpdateRequestBody updateBody = new(Capacity: 200, Range: 6000, AverageFuelConsumption: 2400.0);

        _aircraftRepositoryMock
            .Setup(r => r.Update(It.IsAny<AircraftUpdateDTO>()))
            .Throws(new EntityNotFoundException("Aircraft not found"));

        // Act
        IResult result = _controller.Update(updateBody, 999);

        // Assert
        Assert.Equal(StatusCodes.Status404NotFound, GetStatusCode(result));
    }

    [Fact]
    public async Task Remove_ShouldReturnOk_WhenAircraftExists()
    {
        // Arrange
        _aircraftRepositoryMock
            .Setup(r => r.DeleteAsync(1))
            .Returns(Task.CompletedTask);

        // Act
        IResult result = await _controller.RemoveAsync(1);

        // Assert
        Assert.Equal(StatusCodes.Status200OK, GetStatusCode(result));
        _aircraftRepositoryMock.Verify(r => r.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task Remove_ShouldReturnNotFound_WhenRepositoryThrowsEntityNotFound()
    {
        // Arrange
        _aircraftRepositoryMock
            .Setup(r => r.DeleteAsync(It.IsAny<int>()))
            .ThrowsAsync(new EntityNotFoundException("Aircraft not found"));

        // Act
        IResult result = await _controller.RemoveAsync(999);

        // Assert
        Assert.Equal(StatusCodes.Status404NotFound, GetStatusCode(result));
    }
}
