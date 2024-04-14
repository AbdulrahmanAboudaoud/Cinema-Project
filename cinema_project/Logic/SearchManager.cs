static class SearchManager
{
    public static void SearchMovies()
    {
        Console.WriteLine("Choose search criteria:");
        Console.WriteLine("1. Search by film");
        Console.WriteLine("2. Search by year");
        Console.WriteLine("3. Search by genre");

        string searchOption = Console.ReadLine();

        switch (searchOption)
        {
            case "1":
                SearchByFilm();
                break;
            case "2":
                SearchByYear();
                break;
            case "3":
                SearchByGenre();
                break;
            default:
                Console.WriteLine("Invalid option.");
                break;
        }
    }

    private static void SearchByFilm()
    {
        Console.WriteLine("Enter the title of the movie:");
        string title = Console.ReadLine();

        MovieManager movieManager = new MovieManager();
        List<Movie> movies = movieManager.GetAllMovies();

        var filteredMovies = movies.Where(movie => movie.Title.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();

        if (filteredMovies.Count > 0)
        {
            Console.WriteLine("\nSearch Results:");
            foreach (var movie in filteredMovies)
            {
                Console.WriteLine($"Title: {movie.Title}, Year: {movie.Year}, Genre: {movie.Genre}");
            }
        }
        else
        {
            Console.WriteLine("No movies found with the given title.");
        }
    }

    private static void SearchByYear()
    {
        Console.WriteLine("Enter the year:");
        if (int.TryParse(Console.ReadLine(), out int year))
        {
            MovieManager movieManager = new MovieManager();
            List<Movie> movies = movieManager.GetAllMovies();

            var filteredMovies = movies.Where(movie => movie.Year == year).ToList();

            if (filteredMovies.Count > 0)
            {
                Console.WriteLine("\nSearch Results:");
                foreach (var movie in filteredMovies)
                {
                    Console.WriteLine($"Title: {movie.Title}, Year: {movie.Year}, Genre: {movie.Genre}");
                }
            }
            else
            {
                Console.WriteLine("No movies found for the given year.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input for year.");
        }
    }

    private static void SearchByGenre()
    {
        Console.WriteLine("Enter the genre:");
        string genre = Console.ReadLine();

        MovieManager movieManager = new MovieManager();
        List<Movie> movies = movieManager.GetAllMovies();

        var filteredMovies = movies.Where(movie => movie.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase)).ToList();

        if (filteredMovies.Count > 0)
        {
            Console.WriteLine("\nSearch Results:");
            foreach (var movie in filteredMovies)
            {
                Console.WriteLine($"Title: {movie.Title}, Year: {movie.Year}, Genre: {movie.Genre}");
            }
        }
        else
        {
            Console.WriteLine("No movies found for the given genre.");
        }
    }

}