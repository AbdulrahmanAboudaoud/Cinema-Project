using System.Globalization;

static class SearchLogic
{
    public static void SearchByFilm(ref User loggedInUser)
    {
        Console.WriteLine("Enter the title of the movie:");
        string title = Console.ReadLine();

        List<Movie> movies = MovieAccess.GetAllMovies();
        var movieSchedule = MovieScheduleAccess.GetMovieSchedule();

        var filteredMovies = movies.Where(movie => movie.movieTitle.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();

        if (filteredMovies.Count > 0)
        {
            Console.WriteLine("\nSearch Results:");
            foreach (var movie in filteredMovies)
            {
                Console.WriteLine($"Title: {movie.movieTitle}, Year: {movie.Year}, Genre: {movie.Genre}");
            }

            Console.WriteLine("\nAvailable screenings for this movie:");
            int ScreeningNumber = 1;
            foreach (var screening in movieSchedule)
            {
                if (screening["movieTitle"] == title)
                {
                    if (DateTime.TryParseExact(screening["displayTime"], "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime displayTime))
                    {
                        Console.WriteLine($"{ScreeningNumber} - {screening["movieTitle"]} at {displayTime:yyyy-MM-dd HH:mm} in {screening["auditorium"]}");
                        ScreeningNumber++;
                    }
                    else
                    {
                        Console.WriteLine($"Error parsing display time for movie: {screening["movieTitle"]}");
                    }
                }
                else
                {
                    Console.WriteLine("No screenings found for this movie.");
                }
            }
            Console.WriteLine("\nWould like to make a reservation? (Y/N)");
            string choice = Console.ReadLine();
            if (choice == "Y" || choice == "y")
            {
                Console.Write("\nEnter the screening number you want to reserve: ");
                if (int.TryParse(Console.ReadLine(), out int selectedScreeningNumber))
                {
                    selectedScreeningNumber--;

                    if (selectedScreeningNumber >= 0 && selectedScreeningNumber < ScreeningNumber - 1)
                    {
                        var selectedScreening = movieSchedule
                            .Where(screening => screening["movieTitle"] == title)
                            .ElementAt(selectedScreeningNumber);

                        ReservationLogic.MakeReservationFromSearch(loggedInUser.Username, selectedScreening);
                        ReservationMenu.Start(ref loggedInUser);
                    }
                    else
                    {
                        Console.WriteLine("Invalid screening number.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid screening number.");
                }
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