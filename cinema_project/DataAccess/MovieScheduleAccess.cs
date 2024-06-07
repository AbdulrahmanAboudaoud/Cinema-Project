﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

public static class MovieScheduleAccess
{
    //static public string MovieScheduleFilePath = "C:\\Users\\Gebruiker\\OneDrive - Hogeschool Rotterdam\\Github\\Cinema-Project\\cinema_project\\DataSources\\MovieSchedule.json";
    //static public string MovieScheduleFilePath = "C:\\Users\\Joseph\\Documents\\GitHub\\Cinema-Project\\cinema_project\\DataSources\\MovieSchedule.json";
    static public string MovieScheduleFilePath = "C:\\Users\\abdul\\OneDrive\\Documents\\GitHub\\Cinema-Project\\cinema_project\\DataSources\\MovieSchedule.json";

    public static void PrintMoviesWithAuditoriumAndDates()
    {

        string jsonData = File.ReadAllText(MovieScheduleFilePath);
        JArray movieSchedule = JArray.Parse(jsonData);

        Console.WriteLine("Available Movies with Auditorium and Dates:");

        foreach (JObject movie in movieSchedule)
        {
            string movieTitle = movie["movieTitle"].ToString();
            string displayDate = DateTime.Parse(movie["displayTime"].ToString()).ToString("yyyy-MM-dd HH:mm");
            string auditorium = movie["auditorium"].ToString();

            Console.WriteLine($"Movie: {movieTitle}, Date: {displayDate}, Auditorium: {auditorium}");
        }
    }

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

    public static void WriteToMovieSchedule(string filename, string movieTitle, DateTime displayTime, string auditorium, decimal lowPrice, decimal mediumPrice, decimal highPrice, decimal handiPrice)
    {
        try
        {
            // Read the existing data
            string json = File.ReadAllText(MovieScheduleFilePath);
            var movieSchedule = JsonConvert.DeserializeObject<List<Movie>>(json) ?? new List<Movie>();

            // Add the new movie entry
            movieSchedule.Add(new Movie(movieTitle)
            {
                filename = filename,
                displayTime = displayTime,
                auditorium = auditorium,
                LowPrice = lowPrice,
                MediumPrice = mediumPrice,
                HighPrice = highPrice,
                HandicapPrice = handiPrice
            });

            // Write back to the file
            string updatedJson = JsonConvert.SerializeObject(movieSchedule, Formatting.Indented);
            File.WriteAllText(MovieScheduleFilePath, updatedJson);

            Console.WriteLine("Movie schedule updated successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error writing to movie schedule: " + ex.Message);
        }
    }

    public static void CheckOutdatingMovieScreenings()
    {
        var movieschedule = GetMovieSchedule();
        bool isUpdated = false;

        for (int i = movieschedule.Count - 1; i >= 0; i--)
        {
            var movieScreening = movieschedule[i];
            if (DateTime.TryParseExact(movieScreening["displayTime"], "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime displayTime))
            {
                if (displayTime < DateTime.Now)
                {
                    string movieTitle = movieScreening["movieTitle"];
                    string auditorium = movieScreening["auditorium"];
                    AuditoriumsDataAccess.RemoveLayoutFile(movieTitle, displayTime, auditorium);

                    movieschedule.RemoveAt(i);
                    isUpdated = true;
                }
            }
        }

        if (isUpdated)
        {
            try
            {
                string updatedJson = JsonConvert.SerializeObject(movieschedule, Formatting.Indented);
                File.WriteAllText(MovieScheduleFilePath, updatedJson);
                Console.WriteLine("Outdated movie screenings removed and schedule updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating movie schedule: " + ex.Message);
            }
        }
        else
        {
            Console.WriteLine("No outdated movie screenings found.");
        }
    }

}