using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieReservationSystem.Models;
using System.Text.Json;

namespace MovieReservationSystem.Data {
    public class ApplicationDbContext : IdentityDbContext<IdentityUser> {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) {
        }

        public DbSet<Movies> Movies { get; set; }
        public DbSet<Revenue> Revenue { get; set; }
        public DbSet<Seat> Seat { get; set; }
        public DbSet<Showtime> Showtime { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Showtime>()
                .HasOne(s => s.Movie)
                .WithMany()
                .HasForeignKey(s => s.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Seat>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            var movieFilePath = Path.Combine("Data", "movie.json");
            if (File.Exists(movieFilePath)) {
                var jsonString = File.ReadAllText(movieFilePath);
                var movies = JsonSerializer.Deserialize<List<Movies>>(jsonString);
                if (movies != null) {
                    int id = 1;
                    foreach (var movie in movies) {
                        movie.Id = id++;
                    }
                    modelBuilder.Entity<Movies>().HasData(movies);
                }
            }
        }
    }
}
