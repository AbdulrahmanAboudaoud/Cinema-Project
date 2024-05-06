static class SearchLogic
{


    public static void SearchByFilm()
    {
        Console.WriteLine("Enter the title of the movie:");
        string title = Console.ReadLine();


        List<Movie> movies = MovieAccess.GetAllMovies();

        var filteredMovies = movies.Where(movie => movie.movieTitle.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();

        if (filteredMovies.Count > 0)
        {
            Console.WriteLine("\nSearch Results:");
            foreach (var movie in filteredMovies)
            {
                Console.WriteLine($"Title: {movie.movieTitle}, Year: {movie.Year}, Genre: {movie.Genre}");
            }
        }
        else
        {
            Console.WriteLine("No movies found with the given title.");
        }
    }

    public static void SearchByYear()
    {
        Console.WriteLine("Enter the year:");
        if (int.TryParse(Console.ReadLine(), out int year))
        {
            List<Movie> movies = MovieAccess.GetAllMovies();

            var filteredMovies = movies.Where(movie => movie.Year == year).ToList();

            if (filteredMovies.Count > 0)
            {
                Console.WriteLine("\nSearch Results:");
                foreach (var movie in filteredMovies)
                {
                    Console.WriteLine($"Title: {movie.movieTitle}, Year: {movie.Year}, Genre: {movie.Genre}");
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

    public static void SearchByGenre()
    {
        Console.WriteLine("Enter the genre:");
        string genre = Console.ReadLine();


        List<Movie> movies = MovieAccess.GetAllMovies();

        var filteredMovies = movies.Where(movie => movie.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase)).ToList();

        if (filteredMovies.Count > 0)
        {
            Console.WriteLine("\nSearch Results:");
            foreach (var movie in filteredMovies)
            {
                Console.WriteLine($"Title: {movie.movieTitle}, Year: {movie.Year}, Genre: {movie.Genre}");
            }
        }
        else
        {
            Console.WriteLine("No movies found for the given genre.");
        }
    }

}