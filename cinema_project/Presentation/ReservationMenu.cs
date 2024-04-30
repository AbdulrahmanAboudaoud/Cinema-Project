public static class ReservationMenu
{
    public static void Start()
    {
        bool logoutRequested = false;

        while (!logoutRequested)
        {
            Console.WriteLine();
            Console.WriteLine("1. Make reservation");
            Console.WriteLine("2. View reservation history");
            Console.WriteLine("3. Go back to user menu");
            string input = Console.ReadLine()!;
            switch (input)
            {
                case "1":
                    ReservationLogic.MakeReservation();
                    break;
                case "2":
                    //ViewReservationHistory();
                    break;
                case "3":
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