using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

public static class MovieScheduleAccess
{
    static public string MovieScheduleFilePath = "C:\\Users\\Gebruiker\\OneDrive - Hogeschool Rotterdam\\Github\\Cinema-Project\\cinema_project\\DataSources\\MovieSchedule.json";
    public static List<Dictionary<string, string>> GetMovieSchedule()
    {
        List<Dictionary<string, string>> movieSchedule = new List<Dictionary<string, string>>();

        try
        {
            string json = File.ReadAllText(MovieScheduleFilePath);
            movieSchedule = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(json);
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Movie schedule file not found.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading movie schedule file: " + ex.Message);
        }

        return movieSchedule;
    }

    public static void WriteToMovieSchedule(string filename, string movieTitle, DateTime displayTime, string auditorium)
    {
        try
        {
            // Eerst de filmplanning ophalen uit het JSON-bestand
            List<Dictionary<string, string>> movieSchedule = GetMovieSchedule();

            // Een nieuwe dictionary maken voor de nieuwe film
            Dictionary<string, string> newMovie = new Dictionary<string, string>
            {
                { "filename", filename },
                { "movieTitle", movieTitle },
                { "displayTime", displayTime.ToString("yyyy-MM-ddTHH:mm:ss") }, // DisplayTime omzetten naar string formaat
                { "auditorium", auditorium }
            };

            // De nieuwe film toevoegen aan de filmplanning
            movieSchedule.Add(newMovie);

            // De bijgewerkte filmplanning opslaan in het JSON-bestand
            string updatedJson = JsonConvert.SerializeObject(movieSchedule, Formatting.Indented);
            File.WriteAllText(MovieScheduleFilePath, updatedJson);

            Console.WriteLine("Movie schedule updated successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error writing to movie schedule: " + ex.Message);
        }
    }

}