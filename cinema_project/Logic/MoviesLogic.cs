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
        var movieToEdit = movies.FirstOrDefault(m => m.Title.Equals(titleToEdit, StringComparison.OrdinalIgnoreCase));
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
                        updatedMovie.Title = Console.ReadLine();
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

                movieToEdit.Title = updatedMovie.Title;
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

        var movieToRemove = movies.FirstOrDefault(m => m.Title.Equals(TitleToRemove, StringComparison.OrdinalIgnoreCase));
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
        var movie = movies.FirstOrDefault(m => m.Title.Equals(movieTitle, StringComparison.OrdinalIgnoreCase));
        if (movie != null)
        {
            movie.DisplayDate = displayDate;
            movie.Auditorium = auditorium;
            try
            {
                MovieAccess.WriteMoviesToCSV(movies);
                MovieAccess.CreateLayoutFile(movie.Title, displayDate, auditorium);
                
                string FileName = $"{movie.Title}-{displayDate:yyyyMMdd-HHmm}-{auditorium}.json";
                MovieScheduleAccess.WriteToMovieSchedule(FileName, movie.Title, displayDate, auditorium);

                Console.WriteLine("Time, date, and auditorium added successfully for the movie.");
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


