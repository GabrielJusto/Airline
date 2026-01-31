using System.ComponentModel.DataAnnotations;

using Airline.DTO;
using Airline.DTO.SeatDTOs;
using Airline.Models;
using Airline.Repositories.Interfaces;

namespace Airline.Services.Implementations;

public class SeatListService(ISeatRepository seatRepository)
{
    private readonly ISeatRepository _seatRepository = seatRepository;

    public async Task<List<SeatTicketListDTO>> ListAvailableSeatsForTicket(SeatListFilterDTO filters)
    {
        if(filters.FromIATACode == null || filters.ToIATACode == null)
        {
            throw new ValidationException("fomIATACode and toIATACode can not be null");
        }

        IEnumerable<Seat> seats = await _seatRepository.ListAsync(filters);

        List<SeatTicketListDTO> tickets = seats
            .Where(s => s.IsAvailable)
            .Select(s => new SeatTicketListDTO(s))
            .GroupBy(s => new { s.Price, s.FlightNumber })
            .Select(g => g.First())
            .ToList();

        return tickets;
    }
}