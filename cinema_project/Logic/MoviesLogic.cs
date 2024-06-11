public class MoviesLogic : IAdd
{

    public void AddItem<T>(T item)
    {
        if (item is Movie movie)
        {
            List<Movie> movies = MovieAccess.GetAllMovies();
            movies.Add(movie);
            MovieAccess.WriteMoviesToCSV(movies);
            Console.WriteLine("Movie added successfully.");
        }
        else
        {
            throw new ArgumentException("Item must be of type Movie.");
        }
    }

    public static void EditTitleMovie(string oldTitle, string newTitle)
    {
        Movie[] movies = MovieAccess.GetAllMovies().ToArray();
        var movieToEdit = movies.FirstOrDefault(m => m.movieTitle.Equals(oldTitle, StringComparison.OrdinalIgnoreCase));
        if (movieToEdit != null)
        {
            movieToEdit.movieTitle = newTitle;
            MovieAccess.WriteMoviesToCSV(movies.ToList());
            Console.WriteLine("Title edited successfully.");
        }
        else
        {
            Console.WriteLine("Movie not found.");
        }
    }

    public static void EditYearMovie(string title, int newYear)
    {
        Movie[] movies = MovieAccess.GetAllMovies().ToArray();
        var movieToEdit = movies.FirstOrDefault(m => m.movieTitle.Equals(title, StringComparison.OrdinalIgnoreCase));
        if (movieToEdit != null)
        {
            movieToEdit.Year = newYear;
            MovieAccess.WriteMoviesToCSV(movies.ToList());
            Console.WriteLine("Year edited successfully.");
        }
        else
        {
            Console.WriteLine("Movie not found.");
        }
    }

    public static void EditGenreMovie(string title, string newGenre)
    {
        Movie[] movies = MovieAccess.GetAllMovies().ToArray();
        var movieToEdit = movies.FirstOrDefault(m => m.movieTitle.Equals(title, StringComparison.OrdinalIgnoreCase));
        if (movieToEdit != null)
        {
            movieToEdit.Genre = newGenre;
            MovieAccess.WriteMoviesToCSV(movies.ToList());
            Console.WriteLine("Genre edited successfully.");
        }
        else
        {
            Console.WriteLine("Movie not found.");
        }
    }

    public bool RemoveMovie(string TitleToRemove)
    {
        List<Movie> movies = MovieAccess.GetAllMovies();

        var movieToRemove = movies.FirstOrDefault(m => m.movieTitle.Equals(TitleToRemove, StringComparison.OrdinalIgnoreCase));
        if (movieToRemove != null)
        {
            movies.Remove(movieToRemove);
            MovieAccess.WriteMoviesToCSV(movies);
            Console.WriteLine("Movie removed successfully");
            return true;
        }
        else
        {
            Console.WriteLine("Movie not found.");
            return false;
        }
    }

    public static void AddTimeAndAuditorium(string movieTitle, DateTime displayDate, string auditorium)
    {
        List<Movie> movies = MovieAccess.GetAllMovies();
        var movie = movies.FirstOrDefault(m => m.movieTitle.Equals(movieTitle, StringComparison.OrdinalIgnoreCase));
        if (movie != null)
        {
            movie.displayTime = displayDate;
            movie.auditorium = auditorium;
            movie.movieTitle = movieTitle;

            // Ask admin for seat prices
            Console.WriteLine("Enter low seat price:");
            movie.LowPrice = decimal.Parse(Console.ReadLine());

            Console.WriteLine("Enter medium seat price:");
            movie.MediumPrice = decimal.Parse(Console.ReadLine());

            Console.WriteLine("Enter high seat price:");
            movie.HighPrice = decimal.Parse(Console.ReadLine());

            Console.WriteLine("Enter handicap seat price:");
            movie.HandicapPrice = decimal.Parse(Console.ReadLine());

            try
            {
                MovieAccess.WriteMoviesToCSV(movies);
                AuditoriumsDataAccess.CreateLayoutFile(movie.movieTitle, displayDate, auditorium);

                string FileName = $"{movie.movieTitle}-{displayDate:yyyyMMdd-HHmm}-{auditorium}.json";
                MovieScheduleAccess.WriteToMovieSchedule(FileName, movie.movieTitle, displayDate, auditorium, movie.LowPrice, movie.MediumPrice, movie.HighPrice, movie.HandicapPrice);

                Console.WriteLine("Time, date, auditorium, and seat prices added successfully for the movie.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Movie not found.");
        }
    }
}


