#nullable enable

using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Airline.DTO;
using Airline.Exceptions;
using Airline.Models;
using Airline.Repositories.Interfaces;
using Airline.Services.Implementations;

using Moq;
using Xunit;

namespace Airline.Tests.Services;

public class AircraftServiceTests
{
    private readonly Mock<IAircraftRepository> _aircraftRepositoryMock = new();

    [Fact]
    public void CreateAircraft_WhenRepositoryInsertSucceeds_ReturnsTrue()
    {
        AircraftService service = CreateService();
        AircraftCreateDTO createData = new("Boeing 737", 180, 5600, 2.4);

        bool result = service.CreateAircraft(createData);

        Assert.True(result);
        _aircraftRepositoryMock.Verify(repository => repository.Insert(createData), Times.Once);
    }

    [Fact]
    public void CreateAircraft_WhenRepositoryInsertThrows_ReturnsFalse()
    {
        AircraftService service = CreateService();
        AircraftCreateDTO createData = new("Boeing 737", 180, 5600, 2.4);

        _aircraftRepositoryMock
            .Setup(repository => repository.Insert(createData))
            .Throws(new InvalidOperationException("insert failed"));

        bool result = service.CreateAircraft(createData);

        Assert.False(result);
    }

    [Theory]
    [InlineData(-1, 5600d, 2.4d, "Capacity cannot be negative.")]
    [InlineData(180, -1d, 2.4d, "Range cannot be negative.")]
    [InlineData(180, 5600d, -1d, "Average fuel consumption cannot be negative.")]
    [InlineData(-1, -1d, -1d, "Capacity cannot be negative.; Range cannot be negative.; Average fuel consumption cannot be negative.")]
    public void CreateAircraft_WhenRequestContainsNegativeValues_ThrowsValidationExceptionAndDoesNotInsert(
        int capacity,
        double range,
        double averageFuelConsumption,
        string expectedMessage)
    {
        AircraftService service = CreateService();
        AircraftCreateDTO createData = new("Boeing 737", capacity, range, averageFuelConsumption);

        ValidationException exception = Assert.Throws<ValidationException>(() => service.CreateAircraft(createData));

        Assert.Equal(expectedMessage, exception.Message);
        _aircraftRepositoryMock.Verify(repository => repository.Insert(It.IsAny<AircraftCreateDTO>()), Times.Never);
    }

    [Fact]
    public void ListAircrafts_WhenRepositoryReturnsAircrafts_ReturnsMappedDtos()
    {
        AircraftService service = CreateService();
        AircraftListFiltersDTO filters = new() { Page = 2, PerPage = 5 };
        List<Aircraft> aircrafts =
        [
            CreateAircraft(1, "Airbus A320", 186, 6100, 2.3),
            CreateAircraft(2, "Boeing 777", 396, 9700, 6.8)
        ];

        _aircraftRepositoryMock
            .Setup(repository => repository.ListAircrafts(filters))
            .Returns(aircrafts);

        IReadOnlyList<AircraftDetailDTO> result = service.ListAircrafts(filters);

        Assert.Collection(
            result,
            aircraft =>
            {
                Assert.Equal(1, aircraft.AircraftID);
                Assert.Equal("Airbus A320", aircraft.Model);
                Assert.Equal(186, aircraft.Capacity);
                Assert.Equal(6100, aircraft.Range);
                Assert.Equal(2.3, aircraft.AverageFuelConsumption);
            },
            aircraft =>
            {
                Assert.Equal(2, aircraft.AircraftID);
                Assert.Equal("Boeing 777", aircraft.Model);
                Assert.Equal(396, aircraft.Capacity);
                Assert.Equal(9700, aircraft.Range);
                Assert.Equal(6.8, aircraft.AverageFuelConsumption);
            });
    }

    [Fact]
    public void ListAircrafts_WhenRepositoryReturnsEmptyList_ReturnsEmptyList()
    {
        AircraftService service = CreateService();
        AircraftListFiltersDTO filters = new() { Page = 1, PerPage = 10 };
        List<Aircraft> aircrafts = [];

        _aircraftRepositoryMock
            .Setup(repository => repository.ListAircrafts(filters))
            .Returns(aircrafts);

        IReadOnlyList<AircraftDetailDTO> result = service.ListAircrafts(filters);

        Assert.Empty(result);
        _aircraftRepositoryMock.Verify(repository => repository.ListAircrafts(filters), Times.Once);
    }

    [Fact]
    public void ListAircrafts_WhenRepositoryThrows_RethrowsException()
    {
        AircraftService service = CreateService();
        AircraftListFiltersDTO filters = new() { Page = 1, PerPage = 10 };
        InvalidOperationException exception = new("list failed");

        _aircraftRepositoryMock
            .Setup(repository => repository.ListAircrafts(filters))
            .Throws(exception);

        InvalidOperationException thrownException = Assert.Throws<InvalidOperationException>(() => service.ListAircrafts(filters));

        Assert.Same(exception, thrownException);
    }

    [Fact]
    public void GetAircraftDetail_WhenAircraftExists_ReturnsMappedDto()
    {
        AircraftService service = CreateService();
        Aircraft aircraft = CreateAircraft(10, "Embraer E195", 132, 4260, 2.1);

        _aircraftRepositoryMock
            .Setup(repository => repository.GetAircraft(10))
            .Returns(aircraft);

        AircraftDetailDTO? result = service.GetAircraftDetail(10);

        Assert.NotNull(result);
        Assert.Equal(10, result.AircraftID);
        Assert.Equal("Embraer E195", result.Model);
        Assert.Equal(132, result.Capacity);
        Assert.Equal(4260, result.Range);
        Assert.Equal(2.1, result.AverageFuelConsumption);
    }

    [Fact]
    public void GetAircraftDetail_WhenAircraftDoesNotExist_ThrowsEntityNotFoundException()
    {
        AircraftService service = CreateService();

        _aircraftRepositoryMock
            .Setup(repository => repository.GetAircraft(99))
            .Returns((Aircraft?)null);

        EntityNotFoundException exception = Assert.Throws<EntityNotFoundException>(() => service.GetAircraftDetail(99));

        Assert.Equal("Aircraft with ID 99 not found.", exception.Message);
    }

    [Fact]
    public void GetAircraftDetail_WhenRepositoryThrowsGenericException_RethrowsException()
    {
        AircraftService service = CreateService();
        InvalidOperationException exception = new("detail failed");

        _aircraftRepositoryMock
            .Setup(repository => repository.GetAircraft(15))
            .Throws(exception);

        InvalidOperationException thrownException = Assert.Throws<InvalidOperationException>(() => service.GetAircraftDetail(15));

        Assert.Same(exception, thrownException);
    }

    [Theory]
    [InlineData(null, null, null, 72, 1528d, 0.9d)]
    [InlineData(74, null, null, 74, 1528d, 0.9d)]
    [InlineData(null, 1600d, null, 72, 1600d, 0.9d)]
    [InlineData(null, null, 1.1d, 72, 1528d, 1.1d)]
    [InlineData(74, 1600d, null, 74, 1600d, 0.9d)]
    [InlineData(74, null, 1.1d, 74, 1528d, 1.1d)]
    [InlineData(null, 1600d, 1.1d, 72, 1600d, 1.1d)]
    [InlineData(74, 1600d, 1.1d, 74, 1600d, 1.1d)]
    public void UpdateAircraft_WhenAircraftExists_UpdatesOnlyProvidedFieldsAndReturnsTrue(
        int? capacity,
        double? range,
        double? averageFuelConsumption,
        int expectedCapacity,
        double expectedRange,
        double expectedAverageFuelConsumption)
    {
        AircraftService service = CreateService();
        Aircraft aircraft = CreateAircraft(7, "ATR 72", 72, 1528, 0.9);
        AircraftUpdateDTO updateData = new(7, capacity, range, averageFuelConsumption);

        _aircraftRepositoryMock
            .Setup(repository => repository.GetAircraft(7))
            .Returns(aircraft);

        bool result = service.UpdateAircraft(updateData);

        Assert.True(result);
        Assert.Equal(expectedCapacity, aircraft.Capacity);
        Assert.Equal(expectedRange, aircraft.Range);
        Assert.Equal(expectedAverageFuelConsumption, aircraft.AverageFuelConsumption);
        _aircraftRepositoryMock.Verify(repository => repository.GetAircraft(7), Times.Once);
        _aircraftRepositoryMock.Verify(repository => repository.Update(aircraft), Times.Once);
    }

    [Fact]
    public void UpdateAircraft_WhenAircraftDoesNotExist_ThrowsEntityNotFoundException()
    {
        AircraftService service = CreateService();
        AircraftUpdateDTO updateData = new(8, 120, 3000, 1.8);

        _aircraftRepositoryMock
            .Setup(repository => repository.GetAircraft(8))
            .Returns((Aircraft?)null);

        EntityNotFoundException exception = Assert.Throws<EntityNotFoundException>(() => service.UpdateAircraft(updateData));

        Assert.Equal("Aircraft not found", exception.Message);
        _aircraftRepositoryMock.Verify(repository => repository.GetAircraft(8), Times.Once);
        _aircraftRepositoryMock.Verify(repository => repository.Update(It.IsAny<Aircraft>()), Times.Never);
    }

    [Theory]
    [InlineData(-1, null, null, "Capacity cannot be negative.")]
    [InlineData(null, -1d, null, "Range cannot be negative.")]
    [InlineData(null, null, -1d, "Average fuel consumption cannot be negative.")]
    [InlineData(-1, -1d, -1d, "Capacity cannot be negative.; Range cannot be negative.; Average fuel consumption cannot be negative.")]
    public void UpdateAircraft_WhenRequestContainsNegativeValues_ThrowsValidationExceptionAndDoesNotUpdate(
        int? capacity,
        double? range,
        double? averageFuelConsumption,
        string expectedMessage)
    {
        AircraftService service = CreateService();
        AircraftUpdateDTO updateData = new(10, capacity, range, averageFuelConsumption);

        ValidationException exception = Assert.Throws<ValidationException>(() => service.UpdateAircraft(updateData));

        Assert.Equal(expectedMessage, exception.Message);
        _aircraftRepositoryMock.Verify(repository => repository.GetAircraft(It.IsAny<int>()), Times.Never);
        _aircraftRepositoryMock.Verify(repository => repository.Update(It.IsAny<Aircraft>()), Times.Never);
    }

    [Fact]
    public void UpdateAircraft_WhenRepositoryThrowsGenericException_ReturnsFalse()
    {
        AircraftService service = CreateService();
        Aircraft aircraft = CreateAircraft(9, "Boeing 767", 210, 11000, 4.7);
        AircraftUpdateDTO updateData = new(9, 220, 11500, 4.9);

        _aircraftRepositoryMock
            .Setup(repository => repository.GetAircraft(9))
            .Returns(aircraft);
        _aircraftRepositoryMock
            .Setup(repository => repository.Update(aircraft))
            .Throws(new InvalidOperationException("update failed"));

        bool result = service.UpdateAircraft(updateData);

        Assert.False(result);
        _aircraftRepositoryMock.Verify(repository => repository.GetAircraft(9), Times.Once);
        _aircraftRepositoryMock.Verify(repository => repository.Update(aircraft), Times.Once);
    }

    [Fact]
    public async Task DeleteAircraftAsync_WhenAircraftExists_DeletesAndReturnsTrue()
    {
        AircraftService service = CreateService();
        Aircraft aircraft = CreateAircraft(3, "Airbus A330", 280, 13450, 5.1);

        _aircraftRepositoryMock
            .Setup(repository => repository.GetAircraft(3))
            .Returns(aircraft);
        _aircraftRepositoryMock
            .Setup(repository => repository.DeleteAsync(3))
            .Returns(Task.CompletedTask);

        bool result = await service.DeleteAircraftAsync(3);

        Assert.True(result);
        _aircraftRepositoryMock.Verify(repository => repository.DeleteAsync(3), Times.Once);
    }

    [Fact]
    public async Task DeleteAircraftAsync_WhenAircraftDoesNotExist_ThrowsEntityNotFoundException()
    {
        AircraftService service = CreateService();

        _aircraftRepositoryMock
            .Setup(repository => repository.GetAircraft(4))
            .Returns((Aircraft?)null);

        EntityNotFoundException exception = await Assert.ThrowsAsync<EntityNotFoundException>(() => service.DeleteAircraftAsync(4));

        Assert.Equal("Aircraft not found", exception.Message);
        _aircraftRepositoryMock.Verify(repository => repository.GetAircraft(4), Times.Once);
        _aircraftRepositoryMock.Verify(repository => repository.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAircraftAsync_WhenRepositoryThrowsGenericException_ReturnsFalse()
    {
        AircraftService service = CreateService();
        Aircraft aircraft = CreateAircraft(5, "Boeing 747", 416, 13800, 10.2);

        _aircraftRepositoryMock
            .Setup(repository => repository.GetAircraft(5))
            .Returns(aircraft);
        _aircraftRepositoryMock
            .Setup(repository => repository.DeleteAsync(5))
            .ThrowsAsync(new InvalidOperationException("delete failed"));

        bool result = await service.DeleteAircraftAsync(5);

        Assert.False(result);
        _aircraftRepositoryMock.Verify(repository => repository.GetAircraft(5), Times.Once);
        _aircraftRepositoryMock.Verify(repository => repository.DeleteAsync(5), Times.Once);
    }

    private AircraftService CreateService()
    {
        return new AircraftService(_aircraftRepositoryMock.Object);
    }

    private static Aircraft CreateAircraft(
        int aircraftId,
        string model,
        int capacity,
        double range,
        double averageFuelConsumption)
    {
        return new Aircraft
        {
            AircraftID = aircraftId,
            Model = model,
            Capacity = capacity,
            Range = range,
            AverageFuelConsumption = averageFuelConsumption
        };
    }
}