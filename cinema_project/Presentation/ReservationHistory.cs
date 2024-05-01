using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

public static class ReservationHistory
{
    public static void ViewReservationHistory(string username)
    {
        // Load reservation history for the user from CSV file
        List<Reservation> userReservations = LoadUserReservationsFromCSV(username);

        // Display reservation history to the user
        if (userReservations.Count > 0)
        {
            Console.WriteLine($"Reservation History for {username}:");
            foreach (var reservation in userReservations)
            {
                Console.WriteLine($"Movie: {reservation.MovieTitle}, Date: {reservation.Date}, Auditorium: {reservation.Auditorium}, Seat: {reservation.SeatNumber}");
            }
        }
        else
        {
            Console.WriteLine("No reservations found for this user.");
        }
    }



    private static List<Reservation> LoadUserReservationsFromCSV(string username)
    {
        List<Reservation> userReservations = new List<Reservation>();

        try
        {
            // Read all lines from the CSV file
            string[] lines = File.ReadAllLines("C:\\Users\\Joseph\\Documents\\GitHub\\Cinema-Project\\cinema_project\\DataSources\\ReservationHistory.csv");

            foreach (string line in lines)
            {
                // Split the line by comma to extract reservation details
                string[] parts = line.Split(',');
                if (parts.Length == 5 && parts[0] == username)
                {
                    // Parse date with hours and minutes
                    DateTime date;
                    if (DateTime.TryParseExact(parts[2], "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                    {
                        // Create a reservation object and add it to the list
                        userReservations.Add(new Reservation(
                            username: parts[0],
                            movieTitle: parts[1],
                            date: date,
                            auditorium: parts[3],
                            seatNumber: parts[4]
                        ));
                    }
                    else
                    {
                        Console.WriteLine($"Error parsing date for reservation: {parts[1]}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading reservation history: {ex.Message}");
        }

        return userReservations;
    }

}
