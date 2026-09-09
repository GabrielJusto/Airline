
using Airline.DTO;
using Airline.Exceptions;
using Airline.Models;
using Airline.Repositories.Implementations;
using Airline.Repositories.Interfaces;

namespace Airline.Services.Implementations;

public class TicketPurchaseService(
    TicketRepository ticketRepository,
    AirlineUserRepository airlineUserRepository,
    ISeatRepository seatRepository
)
{
    private readonly TicketRepository _ticketRepository = ticketRepository;
    private readonly AirlineUserRepository _airlineUserRepository = airlineUserRepository;
    private readonly ISeatRepository _seatRepository = seatRepository;

    public async Task<int> PurchaseTicketAsync(TicketPurchaseRequestDTO ticketData)
    {
        Seat seat = await _seatRepository.GetSeatByIdAsync(ticketData.SeatId) ?? throw new EntityNotFoundException(nameof(Seat), ticketData.SeatId);
        AirlineUser user = await _airlineUserRepository.GetUserByIdAsync(ticketData.AirlineUserId) ?? throw new EntityNotFoundException(nameof(AirlineUser), ticketData.AirlineUserId);

        Ticket? ownerTicket = await _ticketRepository.GetByOwnerDocumentAndFlightAsync(ticketData.OwnerDocument, seat.FlightId);
        if(ownerTicket != null)
        {
            throw new TicketPurchaseException(user.Id, seat.FlightId);
        }

        seat.IsAvailable = false;
        await _seatRepository.UpdateAsync(seat);
        Ticket ticket = new(ticketData);
        return await _ticketRepository.AddTicketAsync(ticket);
    }
}