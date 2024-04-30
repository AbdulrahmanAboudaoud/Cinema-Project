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
            movieToEdit.Title = updatedMovie.Title;
            movieToEdit.Year = updatedMovie.Year;
            movieToEdit.Genre = updatedMovie.Genre;
            MovieAccess.WriteMoviesToCSV(movies);
            Console.WriteLine("Movie edited successfully.");
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
