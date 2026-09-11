using System.ComponentModel.DataAnnotations;

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

    public SeatService(ISeatRepository seatRepository)
    {
        _seatRepository = seatRepository;
    }
    public async Task<IReadOnlyList<SeatTicketListDTO>> ListAvailableSeatsForTicketAsync(SeatListFilterDTO filters)
    {
        ListAvailableSeatsForTicketValidate(filters);

        IEnumerable<Seat> seats = await _seatRepository.ListAsync(filters);

        List<SeatTicketListDTO> tickets = seats
            .Where(s => s.IsAvailable)
            .Select(s => new SeatTicketListDTO(s))
            .GroupBy(s => new { s.Price, s.FlightNumber })
            .Select(g => g.First())
            .Select(s => s with
            {
                Departure = filters.DepartureDate?.Offset != null
                    ? s.Departure.ToOffset(filters.DepartureDate.Value.Offset)
                    : s.Departure,
                Arrival = filters.DepartureDate?.Offset != null
                    ? s.Arrival.ToOffset(filters.DepartureDate.Value.Offset)
                    : s.Arrival
            })
            .ToList();


        return tickets;
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