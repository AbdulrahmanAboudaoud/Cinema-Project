﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

public static class MovieScheduleAccess
{
    static public string MovieScheduleFilePath = "C:\\Users\\abdul\\OneDrive\\Documents\\GitHub\\Cinema-Project\\cinema_project\\DataSources\\MovieSchedule.json";
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

    public static void WriteToMovieSchedule(string filename, string movieTitle, DateTime displayTime, string auditorium, decimal lowPrice, decimal mediumPrice, decimal highPrice)
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
                HighPrice = highPrice
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
}