using Airline.DTO;
using Airline.DTO.SeatDTOs;
using Airline.Exceptions;
using Airline.Models;
using Airline.Repositories.Interfaces;
using Airline.Services.Interfaces;

namespace Airline.Services.Implementations;

public class SeatService : ISeatService
{
    private readonly ISeatRepository _seatRepository;
    private readonly IFlightService _flightService;

    public SeatService(
        ISeatRepository seatRepository,
        IFlightService flightService
    )
    {
        _seatRepository = seatRepository;
        _flightService = flightService;
    }

    public async Task CreateAsync(SeatCreateRequestDTO data)
    {
        Flight? flight = await _flightService.GetByIdAsync(data.FlightId);
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

    public async Task<IReadOnlyList<SeatTicketListDTO>> ListAvailableSeatsForTicketAsync(SeatListFilterDTO filters)
    {
        ListAvailableSeatsForTicketValidate(filters);

        IEnumerable<Seat> seats = await _seatRepository.ListAsync(filters);

        List<SeatTicketListDTO> tickets = seats
            .Where(s => s.IsAvailable)
            .Select(s => new SeatTicketListDTO(s))
            .GroupBy(s => new { s.Price, s.FlightId })
            .Select(g => g.First())
            .Select(s => s with
            {
                Departure = filters.StartDate?.Offset != null
                    ? s.Departure.ToOffset(filters.StartDate.Value.Offset)
                    : s.Departure,
                Arrival = filters.StartDate?.Offset != null
                    ? s.Arrival.ToOffset(filters.StartDate.Value.Offset)
                    : s.Arrival
            })
            .ToList();


        return tickets;
    }

    public async Task<IReadOnlyList<SeatDetailDTO>> ListAsync(SeatListFilterDTO filters, CancellationToken cancellationToken = default)
    {
        IEnumerable<Seat> seats = await _seatRepository.ListAsync(filters);
        return seats.Select(s => new SeatDetailDTO(s)).ToList();
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

    private static void ListAvailableSeatsForTicketValidate(SeatListFilterDTO filters)
    {
        Dictionary<string, List<string>> errors = new();

        if(filters.FromIATACode == null)
        {
            errors.Add(nameof(SeatListFilterDTO.FromIATACode), ["fromIATACode can not be null."]);
        }
        if(filters.ToIATACode == null)
        {
            errors.Add(nameof(SeatListFilterDTO.ToIATACode), ["toIATACode can not be null."]);
        }

        if(errors.Count > 0)
        {
            throw new DtoValidationException(errors);
        }
    }
}