public static class AdminLogic
{
    static private MoviesLogic movieManager = new MoviesLogic();

    public static void RemoveMovie()
    {
        Console.WriteLine("Enter the title of the movie you want to remove:");
        string titleToRemove = Console.ReadLine();

        bool removed = movieManager.RemoveMovie(titleToRemove);
        if (removed)
        {
            Console.WriteLine($"Movie '{titleToRemove}' removed successfully.");
        }
        else
        {
            Console.WriteLine($"Failed to remove movie '{titleToRemove}'. Movie not found.");
        }
    }

    public static void ViewMovies()
    {
        List<Movie> movies = MovieAccess.GetAllMovies();

        Console.WriteLine("\nAvailable Movies:");
        foreach (var movie in movies)
        {
            Console.WriteLine($"Title: {movie.Title}, Year: {movie.Year}, Genre: {movie.Genre}");
        }
        Console.WriteLine();
    }

    public static void EditMovie()
    {
        Console.WriteLine("Enter the title of the movie you want to edit:");
        string titleToEdit = Console.ReadLine();

        Console.WriteLine("Enter the new title of the movie:");
        string newTitle = Console.ReadLine();

        Console.WriteLine("Enter the new year of release:");
        int newYear = int.Parse(Console.ReadLine());

        Console.WriteLine("Enter the new genre of the movie:");
        string newGenre = Console.ReadLine();

        Movie updatedMovie = new Movie(newTitle, newYear, newGenre);
        movieManager.EditMovie(titleToEdit, updatedMovie);
    }

    public static void AddMovie()
    {
        Console.WriteLine("Enter the title of the movie:");
        string title = Console.ReadLine();

        Console.WriteLine("Enter the year of release:");
        int year = int.Parse(Console.ReadLine());

        Console.WriteLine("Enter the genre of the movie:");
        string genre = Console.ReadLine();

        Movie newMovie = new Movie(title, year, genre);
        movieManager.AddMovie(newMovie);
        Console.WriteLine("Movie added successfully.");
    }

    public static void EditRules()
    {
        Console.WriteLine("Which rule would you like to edit? (Insert rule number)");
        int RuleNumber = Convert.ToInt32(Console.ReadLine());
        RulesLogic.EditRules(RuleNumber);
    }

    public static void AddRule()
    {
        Console.WriteLine("Enter new rule:");
        string NewRule = Console.ReadLine();
        RulesLogic.AddRule(NewRule);
    }

    public static void RemoveRule()
    {
        Console.WriteLine("Which rule would you like to remove? (Insert rule number)");
        int RuleNumber = Convert.ToInt32(Console.ReadLine());
        RulesLogic.RemoveRule(RuleNumber);
    }



    public static void ViewAllUsers()
    {
        List<User> users = UserLogic.GetAllUsers();

        Console.WriteLine("\nAll Users:");
        foreach (var user in users)
        {
            Console.WriteLine($"Username: {user.Username}, Role: {user.Role}");
        }
        Console.WriteLine();
    }
}