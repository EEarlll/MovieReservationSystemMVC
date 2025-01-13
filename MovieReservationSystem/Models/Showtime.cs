namespace MovieReservationSystem.Models {
    public class Showtime {

        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int MovieId { get; set; }
        public Movies? Movie { get; set; }
        public int SeatCount { get; set; }
    }
}
