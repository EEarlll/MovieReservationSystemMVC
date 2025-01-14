// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function Fill(element) {
    const seatId = element.getAttribute("data-id");
    let SeatNumber = document.getElementById("SeatNumber");
    let SelectedSeat = document.getElementById("Selected-Seat");
    SeatNumber.value = seatId;
    SelectedSeat.innerHTML = seatId;

    // Remove the blue color from all seats
    let seats = document.querySelectorAll("[data-id]");
    seats.forEach(seat => {
        seat.style.backgroundColor = "";
    });

    // Turn the clicked seat blue
    element.style.backgroundColor = "blue";
}