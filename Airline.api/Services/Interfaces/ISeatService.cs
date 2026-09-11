using Airline.DTO;
using Airline.DTO.SeatDTOs;
using Airline.Models;

namespace Airline.Services.Interfaces;

public interface ISeatService
{
    public Task CreateAsync(SeatCreateRequestDTO data);
    public Task<IReadOnlyList<SeatTicketListDTO>> ListAvailableSeatsForTicketAsync(SeatListFilterDTO filters);
}