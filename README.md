# Movie Reservation System

## Overview
The Movie Reservation System is a web application designed to allow users to browse, search, and reserve movie tickets. It provides a comprehensive list of movies, including details such as genre, plot, runtime, and poster images.

## Features
- Browse movies by genre, title, and other attributes.
- View detailed information about each movie.
- Reserve tickets for available showtimes.
- User authentication and profile management.

## Controllers
The application includes several controllers to manage different aspects of the system:

### MoviesController
Handles the management of movies, including:
- Listing all movies.
- Viewing details of a specific movie.
- Creating, editing, and deleting movies (Admin only).

### ShowtimesController
Manages showtimes for movies, including:
- Listing all showtimes.
- Viewing details of a specific showtime.
- Creating, editing, and deleting showtimes (Admin only).
- Reserving seats for a showtime (User and Admin).

### SeatsController
Handles seat reservations, including:
- Listing all reserved seats.
- Viewing details of a specific seat (Admin only).
- Creating and deleting seat reservations (User and Admin).

### RolesController
Manages user roles, including:
- Listing all users and their roles (Admin only).
- Promoting users to Admin role (Admin only).

## Models
The application uses several models to represent data:

### Movies
Represents a movie with properties such as:
- Id
- Title
- Plot
- Genre
- Runtime
- Poster

### Showtime
Represents a showtime with properties such as:
- Id
- StartTime
- EndTime
- MovieId
- Movie
- SeatCount

### Seat
Represents a seat reservation with properties such as:
- Id
- SeatNumber
- SeatType
- ShowtimeId
- Showtime
- UserId
- User

### ViewRole
Represents a user role view with properties such as:
- UserId
- Username
- Role


## Getting Started
### Prerequisites
- .NET 6.0 SDK or later
- SQL Server

### Installation
1. Clone the repository:
    ```sh
    git clone https://github.com/yourusername/MovieReservationSystem.git
    ```
2. Navigate to the project directory:
    ```sh
    cd MovieReservationSystem
    ```
3. Restore the dependencies:
    ```sh
    dotnet restore
    ```
4. Update the database:
    ```sh
    dotnet ef database update
    ```
5. Run the application:
    ```sh
    dotnet run
    ```

## License
This project is licensed under the MIT License. See the [LICENSE](./LICENSE) file for details.

## Acknowledgements
- .NET Foundation
- Bootstrap Authors
- OpenJS Foundation

## Contact
For any inquiries or feedback, please contact [yourname@example.com](mailto:yourname@example.com).
