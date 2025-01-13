
namespace MovieReservationSystem.Models {
    public class Movies {

        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Plot { get; set; }
        public required string Genre { get; set; }
        public int Runtime { get; set; }
        public required string Poster { get; set; }
    }
}
