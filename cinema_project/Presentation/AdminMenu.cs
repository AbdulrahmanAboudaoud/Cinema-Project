static class AdminMenu
{
    static private MovieManager movieManager = new MovieManager();

    static public void Start()
    {
        bool logoutRequested = false;

        while (!logoutRequested)
        {
            Console.WriteLine("1. View Movies");
            Console.WriteLine("2. Add Movie");
            Console.WriteLine("3. Edit Movie");
            Console.WriteLine("4. Remove Movie");
            Console.WriteLine("5. View All Users");
            Console.WriteLine("6. Logout");

            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    ViewMovies();
                    break;
                case "2":
                    AddMovie();
                    break;
                case "3":
                    EditMovie();
                    break;
                case "4":
                    RemoveMovie();
                    break;
                case "5":
                    ViewAllUsers();
                    break;
                case "6":
                    Logout();
                    logoutRequested = true;
                    break;
                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
        }
    }

    static private void ViewMovies()
    {
        MovieManager movieManager = new MovieManager();
        List<Movie> movies = movieManager.GetAllMovies();

        Console.WriteLine("\nAvailable Movies:");
        foreach (var movie in movies)
        {
            Console.WriteLine($"Title: {movie.Title}, Year: {movie.Year}, Genre: {movie.Genre}");
        }
        Console.WriteLine();
    }

    static private void EditMovie()
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

    static private void AddMovie()
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

    static private void ViewAllUsers()
    {
        //UserManager userManager = new UserManager();
        List<User> users = UserManager.GetAllUsers();

        Console.WriteLine("\nAll Users:");
        foreach (var user in users)
        {
            Console.WriteLine($"Username: {user.Username}, Role: {user.Role}");
        }
        Console.WriteLine();
    }

    static private void Logout()
    {
        Console.WriteLine("Logging out...");
        Console.WriteLine("You have been logged out.");
    }

    static private void RemoveMovie()
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
}
