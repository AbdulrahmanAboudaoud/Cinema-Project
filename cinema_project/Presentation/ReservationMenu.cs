public static class ReservationMenu
{
    public static void Start(ref User loggedInUser)
    {
        bool logoutRequested = false;

        while (!logoutRequested)
        {
            Console.WriteLine();
            Console.WriteLine("1. Make reservation");
            Console.WriteLine("2. View reservation history");
            Console.WriteLine("3. Cancel reservation");
            Console.WriteLine("4. Edit reservation");
            Console.WriteLine("5. Go back to user menu");
            string input = Console.ReadLine()!;
            switch (input)
            {
                case "1":
                    ReservationLogic.MakeReservation(loggedInUser.Username);
                    break;
                case "2":
                    ReservationHistory.ViewReservationHistory(loggedInUser.Username);
                    break;
                case "3":
                    ReservationLogic.CancelReservation(loggedInUser.Username);
                    break;
                case "4":
                    ReservationLogic.EditReservation(loggedInUser.Username);
                    break;
                case "5":
                    Console.Clear();
                    logoutRequested = true;
                    break;
                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
        }
    }
}