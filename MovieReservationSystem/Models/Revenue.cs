namespace MovieReservationSystem.Models {
    public class Revenue {
        public int Id { get; set; }
        public int RevenueAmount { get; set; }
        public DateTime Date { get; set; }
        public int MovieId { get; set; }
        public Movies? Movie { get; set; }
    }
}
    