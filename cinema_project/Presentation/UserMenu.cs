public static class UserMenu
{
    public static void Start(ref User loggedInUser)
    {
        bool exitRequested = false;
        Console.Clear();
        while (!exitRequested)
        {
            Console.WriteLine();
            CenterText.printart(TextArt.welcomescreen());
            CenterText.print("=================================", "Cyan");
            CenterText.print("||                             ||", "Cyan");
            CenterText.print("|| 1. Change Account Info      ||", "Cyan");
            CenterText.print("|| 2. Delete Account           ||", "Cyan");
            CenterText.print("|| 3. View All Movies          ||", "Cyan");
            CenterText.print("|| 4. Catering Menu Info       ||", "Cyan");
            CenterText.print("|| 5. Display Auditoriums      ||", "Cyan");
            CenterText.print("|| 6. Search And Filter Movies ||", "Cyan");
            CenterText.print("|| 7. Reservation              ||", "Cyan");
            CenterText.print("|| 8. Logout                   ||", "Cyan");
            CenterText.print("||                             ||", "Cyan");
            CenterText.print("=================================", "Cyan");
            char option = Console.ReadKey().KeyChar;

            switch (option)
            {
                case '1':
                    Console.Clear();
                    UpdateAccount(loggedInUser);
                    break;
                case '2':
                    Console.Clear();
                    bool status = UserLogic.DeleteAccount(loggedInUser.Username);
                    if(status)
                    {
                        Console.WriteLine("Account deleted successfully");
                        Menu.Start();
                    }
                    break;
                case '3':
                    Console.Clear();
                    ViewMovies();
                    break;
                case '4':
                    Console.Clear();
                    CateringMenu.StartMenu(ref loggedInUser);
                    break;
                case '5':
                    Console.Clear();
                    AuditoriumsLogic.ShowAllAuditoriums();
                    break;
                case '6':
                    Console.Clear();
                    SearchMovies();
                    break;
                case '7':
                    Console.Clear();
                    ReservationMenu.Start(ref loggedInUser);
                    break;
                case '8':
                    Console.Clear();
                    exitRequested = true;
                    Logout(ref loggedInUser);
                    //exitRequested = true;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please select again.");
                    break;
            }
        }
    }


    private static void SearchMovies()
    {
        bool exitRequested = false;

        while (!exitRequested)
        {
            CenterText.print(" ===============================================", "Cyan");
            CenterText.print(" ||              Choose search criteria:      ||", "Cyan");
            CenterText.print(" ||                                           ||", "Cyan");
            CenterText.print(" || 1. Search by film                         ||", "Cyan");
            CenterText.print(" || 2. Search by year                         ||", "Cyan");
            CenterText.print(" || 3. Search by genre                        ||", "Cyan");
            CenterText.print(" || 4. Back to menu                           ||", "Cyan");
            CenterText.print(" ||                                           ||", "Cyan");
            CenterText.print(" ===============================================", "Cyan");

            char searchOption= Console.ReadKey().KeyChar;

            switch (searchOption)
            {
                case '1':
                    Console.Clear();    
                    SearchLogic.SearchByFilm();
                    break;
                case '2':
                    Console.Clear(); 
                    SearchLogic.SearchByYear();
                    break;
                case '3':
                    Console.Clear(); 
                    SearchLogic.SearchByGenre();
                    break;
                case '4':
                    Console.Clear(); 
                    exitRequested = true;
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    private static void UpdateAccount(User loggedInUser)
    {
        Console.Clear();
        CenterText.print(" ============================================", "Cyan");
        CenterText.print(" ||                                        ||", "Cyan");
        CenterText.print(" ||  Which info would you like to change?  ||", "Cyan");
        CenterText.print(" || 1. Email                               ||", "Cyan");
        CenterText.print(" || 2. Phone                               ||", "Cyan");
        CenterText.print(" || 3. Name                                ||", "Cyan");
        CenterText.print(" || 4. Password                            ||", "Cyan");
        CenterText.print(" ||                                        ||", "Cyan");
        CenterText.print(" ============================================", "Cyan");
        
        int choice;
        bool isValidChoice = false;
        do
        {
            Console.WriteLine("Enter your choice (1-4):");
            string choiceInput = Console.ReadLine();
            if (!int.TryParse(choiceInput, out choice) || choice < 1 || choice > 4)
            {
                Console.WriteLine("Invalid input. Please enter a number between 1 and 4.");
            }
            else
            {
                isValidChoice = true;
            }
        } while (!isValidChoice);

        Console.WriteLine("Enter the new information:");
        string newInfo = Console.ReadLine();

        bool result = UserLogic.ChangeAccount(newInfo, choice, loggedInUser.Username);
        if (result)
        {
            Console.WriteLine($"Your information has been updated successfully.\n");
            Console.WriteLine();
            Console.WriteLine("Press any key to contitnue..");
            Console.ReadKey();
            Console.Clear();
        }
        else
        {
            Console.WriteLine($"Failed to update information.\n");
        }
    }

    private static void ViewMovies()
    {
        List<Movie> movies = MovieAccess.GetAllMovies();

        Console.WriteLine("\nAvailable Movies:");
        foreach (var movie in movies)
        {
            Console.WriteLine($"Title: {movie.movieTitle}, Year: {movie.Year}, Genre: {movie.Genre}");
        }
        Console.WriteLine();
        Console.WriteLine("Press any key to continue..");
        Console.ReadKey();
        Console.Clear();
    }

    private static void Logout(ref User loggedInUser)
    {
        Console.WriteLine("Logging out...");
        loggedInUser = null;
        Console.WriteLine("You have been logged out.");
    }
}
