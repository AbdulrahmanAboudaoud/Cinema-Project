using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class MovieManager
{
    private const string MoviesFilePath = "C:\\Users\\Gebruiker\\OneDrive - Hogeschool Rotterdam\\Github\\Cinema-Project\\cinema_project\\DataSources\\movies.csv";

    public List<Movie> GetAllMovies()
    {
        List<Movie> movies = new List<Movie>();
        try
        {
            using (var reader = new StreamReader(MoviesFilePath))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    movies.Add(new Movie(values[0], int.Parse(values[1]), values[2]));

                }
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Movies file not found.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading movies file: " + ex.Message);
        }

        return movies;
    }

    public void WriteMoviesToCSV(List<Movie> movies)
    {
        try
        {
            using (var writer = new StreamWriter(MoviesFilePath))
            {
                foreach (var movie in movies)
                {
                    writer.WriteLine($"{movie.Title},{movie.Year},{movie.Genre}");
                }
            }
            Console.WriteLine("Movies written to file successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error writing movies to file: " + ex.Message);
        }
    }

    public void AddMovie(Movie movie)
    {
        List<Movie> movies = GetAllMovies();
        movies.Add(movie);
        WriteMoviesToCSV(movies);
        Console.WriteLine("Movie added successfully.");
    }

    public void EditMovie(string titleToEdit, Movie updatedMovie)
    {
        List<Movie> movies = GetAllMovies();
        var movieToEdit = movies.FirstOrDefault(m => m.Title.Equals(titleToEdit, StringComparison.OrdinalIgnoreCase));
        if (movieToEdit != null)
        {
            movieToEdit.Title = updatedMovie.Title;
            movieToEdit.Year = updatedMovie.Year;
            movieToEdit.Genre = updatedMovie.Genre;
            WriteMoviesToCSV(movies);
            Console.WriteLine("Movie edited successfully.");
        }
        else
        {
            Console.WriteLine("Movie not found.");
        }
    }

    public bool RemoveMovie(string TitleToRemove)
    {
        List<Movie> movies = GetAllMovies();

        var movieToRemove = movies.FirstOrDefault(m => m.Title.Equals(TitleToRemove, StringComparison.OrdinalIgnoreCase));
        if (movieToRemove != null)
        {
            movies.Remove(movieToRemove);
            WriteMoviesToCSV(movies);
            Console.WriteLine("Movie removed successfully");
            return true;
        }
        else
        {
            Console.WriteLine("Movie not found.");
            return false;
        }

    }
}
