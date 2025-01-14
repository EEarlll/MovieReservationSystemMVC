using Microsoft.AspNetCore.Identity;

namespace MovieReservationSystem.Models {
    public class Seat {
        public int Id { get; set; }
        public int SeatNumber { get; set; }
        public string? SeatType { get; set; }
        public int ShowtimeId { get; set; }
        public Showtime? Showtime { get; set; }
        public required string UserId { get; set; }
        public IdentityUser? User { get; set; }
    }
}
