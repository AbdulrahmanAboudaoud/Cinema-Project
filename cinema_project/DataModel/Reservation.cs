public class Reservation
{
    public string MovieTitle { get; set; }
    public DateTime Date { get; set; }
    public string Auditorium { get; set; }
    public string SeatNumber { get; set; }

    public Reservation(string movieTitle, DateTime date, string auditorium, string seatNumber)
    {
        MovieTitle = movieTitle;
        Date = date;
        Auditorium = auditorium;
        SeatNumber = seatNumber;
    }
}
