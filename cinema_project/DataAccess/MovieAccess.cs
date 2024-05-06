using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class MovieAccess
{
    private const string MoviesFilePath = "C:\\Users\\abdul\\OneDrive\\Documents\\GitHub\\Cinema-Project\\cinema_project\\DataSources\\movies.csv";
    private const string CinemaHallsFilePath = "C:\\Users\\abdul\\OneDrive\\Documents\\GitHub\\Cinema-Project\\cinema_project\\DataSources\\CinemaHalls.json";
    private const string DataSourcesFolder = "C:\\Users\\abdul\\OneDrive\\Documents\\GitHub\\Cinema-Project\\cinema_project\\DataSources";


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
            using (var writer = new StreamWriter(MoviesFilePath, false))
            {
                foreach (var movie in movies)
                {
                    string displayDate = movie.displayTime != default(DateTime) ? movie.displayTime.ToString("yyyy-MM-dd HH:mm") : "";

                    writer.WriteLine($"{movie.movieTitle},{movie.Year},{movie.Genre},{displayDate},{movie.auditorium}");
                }
            }
            //Console.WriteLine("Movies written to file successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error writing movies to file: " + ex.Message);
        }
    }


    public static void CreateLayoutFile(string movieName, DateTime displayDate, string auditoriumName)
    {
        string fileName = Path.Combine(DataSourcesFolder, $"{movieName}-{displayDate:yyyyMMdd-HHmm}-{auditoriumName}.json");

        if (!File.Exists(fileName))
        {
            using (FileStream fs = File.Create(fileName)) { }
        }

        string cinemaHallsJson = File.ReadAllText(CinemaHallsFilePath);
        CinemaHalls cinemaHalls = JsonConvert.DeserializeObject<CinemaHalls>(cinemaHallsJson);

        var auditorium = cinemaHalls.auditoriums.FirstOrDefault(a => a.name.Equals(auditoriumName, StringComparison.OrdinalIgnoreCase));
        if (auditorium != null)
        {
            CinemaHalls selectedAuditorium = new CinemaHalls
            {
                auditoriums = new[] { auditorium }
            };

            string selectedAuditoriumJson = JsonConvert.SerializeObject(selectedAuditorium, Formatting.Indented);

            File.WriteAllText(fileName, selectedAuditoriumJson);

            //Console.WriteLine($"Layout for {auditoriumName} copied successfully to {fileName}.");
        }
        else
        {
            Console.WriteLine("Auditorium not found.");
        }
    }
}
