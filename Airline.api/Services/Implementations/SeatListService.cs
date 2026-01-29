using Airline.DTO;
using Airline.DTO.SeatDTOs;
using Airline.Models;
using Airline.Repositories.Interfaces;

namespace Airline.Services.Implementations;

public class SeatListService(ISeatRepository seatRepository)
{
    private readonly ISeatRepository _seatRepository = seatRepository;

    public async Task<List<SeatListDTO>> ListAsync(SeatListFilterDTO filters)
    {
        IEnumerable<Seat> seats = await _seatRepository.ListAsync(filters);
        return seats.Select(s => new SeatListDTO(s)).ToList();
    }

    public async Task<List<SeatTicketListDTO>> ListAvailableSeatsForTicket(SeatListFilterDTO filters)
    {
        IEnumerable<Seat> seats = await _seatRepository.ListAsync(filters);

        List<SeatTicketListDTO> tickets = seats
            .Where(s => s.IsAvailable)
            .Select(s => new SeatTicketListDTO(s))
            .GroupBy(s => s.SeatClass)
            .Select(g => g.First())
            .ToList();

        return tickets;
    }
}