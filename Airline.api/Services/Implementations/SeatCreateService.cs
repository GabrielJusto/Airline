using Airline.DTO;
using Airline.Exceptions;
using Airline.Models;
using Airline.Repositories.Interfaces;
using Airline.Services.Interfaces;

namespace Airline.Services.Implementations;

public class SeatCreateService(
    IFlightRepository flightRepository,
    ISeatRepository seatRepository
) : ISeatCreateService
{
    private readonly IFlightRepository _flightRepository = flightRepository;
    private readonly ISeatRepository _seatRepository = seatRepository;
    public async Task CreateAsync(SeatCreateRequestDTO data)
    {
        Flight? flight = await _flightRepository.GetByIdAsync(data.FlightId);
        if(flight is null)
            throw new EntityNotFoundException(nameof(Flight), data.FlightId);

        List<Seat> newSeats = new();

        newSeats.AddRange(AddSeats(flight, data.QuantityEconomic, Enuns.SeatClassEnum.Economic, 6));
        newSeats.AddRange(AddSeats(flight, data.QuantityExecutive, Enuns.SeatClassEnum.Executive, 6));
        newSeats.AddRange(AddSeats(flight, data.QuantityFirstClass, Enuns.SeatClassEnum.FirstClass, 4));

        foreach(var seat in newSeats)
        {
            flight.Seats.Add(seat);
        }

        await _seatRepository.AddRangeAsync(newSeats);

    }

    private static List<Seat> AddSeats(Flight flight, int quantity, Enuns.SeatClassEnum seatClass, int seatsPerRow)
    {
        List<Seat> seatsList = new();
        for(int i = 0; i < quantity; i++)
        {
            SeatCreateDTO seatData = new()
            {
                SeatNumber = (i / seatsPerRow) + 1,
                Row = ((char)('A' + (i % seatsPerRow))).ToString(),
                IsAvailable = true,
                SeatClass = seatClass,
                AircraftAverageFuelConsumption = flight.Aircraft.AverageFuelConsumption,
                AircraftCapacity = flight.Aircraft.Capacity,
                Distance = flight.Route.Distance
            };
            Seat seat = Seat.Create(seatData);
            seat.FlightId = flight.FlightId;
            seatsList.Add(seat);
        }
        return seatsList;
    }
}