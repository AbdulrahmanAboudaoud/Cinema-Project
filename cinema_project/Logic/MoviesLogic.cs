public class MoviesLogic
{

    public void AddMovie(Movie movie)
    {
        List<Movie> movies = MovieAccess.GetAllMovies();
        movies.Add(movie);
        MovieAccess.WriteMoviesToCSV(movies);
        Console.WriteLine("Movie added successfully.");
    }

    public void EditMovie(string titleToEdit, Movie updatedMovie)
    {
        List<Movie> movies = MovieAccess.GetAllMovies();
        var movieToEdit = movies.FirstOrDefault(m => m.movieTitle.Equals(titleToEdit, StringComparison.OrdinalIgnoreCase));
        if (movieToEdit != null)
        {
            Console.WriteLine("What would you like to edit?");
            Console.WriteLine("1. Title");
            Console.WriteLine("2. Year");
            Console.WriteLine("3. Genre");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Enter the new title of the movie:");
                        updatedMovie.movieTitle = Console.ReadLine();
                        break;
                    case 2:
                        Console.WriteLine("Enter the new year of release:");
                        if (int.TryParse(Console.ReadLine(), out int newYear))
                        {
                            updatedMovie.Year = newYear;
                        }
                        else
                        {
                            Console.WriteLine("Invalid year format. Please enter a valid year.");
                            return;
                        }
                        break;
                    case 3:
                        Console.WriteLine("Enter the new genre of the movie:");
                        updatedMovie.Genre = Console.ReadLine();
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        return;
                }

                movieToEdit.movieTitle = updatedMovie.movieTitle;
                movieToEdit.Year = updatedMovie.Year;
                movieToEdit.Genre = updatedMovie.Genre;
                MovieAccess.WriteMoviesToCSV(movies);
                Console.WriteLine("Movie edited successfully.");
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }
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

            try
            {
                MovieAccess.WriteMoviesToCSV(movies);
                MovieAccess.CreateLayoutFile(movie.movieTitle, displayDate, auditorium);

                string FileName = $"{movie.movieTitle}-{displayDate:yyyyMMdd-HHmm}-{auditorium}.json";
                MovieScheduleAccess.WriteToMovieSchedule(FileName, movie.movieTitle, displayDate, auditorium, movie.LowPrice, movie.MediumPrice, movie.HighPrice);

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


