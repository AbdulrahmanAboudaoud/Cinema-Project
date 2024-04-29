using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class MovieAccess
{
    private const string MoviesFilePath = "C:\\Users\\Joseph\\Documents\\GitHub\\Cinema-Project\\cinema_project\\DataSources\\movies.csv";

    public static List<Movie> GetAllMovies()
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

    public static void WriteMoviesToCSV(List<Movie> movies)
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


}