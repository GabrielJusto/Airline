using Airline.Models;

namespace Airline.DTO;

public class SeatDetailDTO
{
    public int SeatId {get; set;}
    public string Row {get; set;}
    public int SeatNumber {get; set;}
    public decimal Price {get; set;}
    public bool IsAvailable {get; set;}
    public string SeatClass {get; set;} = string.Empty;

    public SeatDetailDTO(Seat seat)
    {
        SeatId = seat.SeatId;
        Row = seat.Row;
        SeatNumber = seat.SeatNumber;
        Price = seat.Price;
        IsAvailable = seat.IsAvailable;
        SeatClass = seat.SeatClass.ToString();
    }
}