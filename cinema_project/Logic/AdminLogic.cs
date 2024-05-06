﻿using System.Diagnostics;

public static class AdminLogic
{
    static private MoviesLogic movieManager = new MoviesLogic();
    private const string ReservationHistoryFilePath = "C:\\Users\\Joseph\\Documents\\GitHub\\Cinema-Project\\cinema_project\\DataSources\\ReservationHistory.csv";



    public static void DisplayAllReservations()
    {
        try
        {
            using (var reader = new StreamReader(ReservationHistoryFilePath))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    string username = values[0];
                    string movieTitle = values[1];
                    DateTime date = DateTime.Parse(values[2]);
                    string auditorium = values[3];
                    string seatNumber = values[4];

                    Console.WriteLine($"Username: {username}, Movie: {movieTitle}, Date: {date}, Auditorium: {auditorium}, Seat: {seatNumber}");
                }
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Reservation history file not found.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading reservation history: {ex.Message}");
        }
    }


    public static void RemoveMovie()
    {
        Console.WriteLine("Enter the title of the movie you want to remove:");
        string titleToRemove = Console.ReadLine();

        bool removed = movieManager.RemoveMovie(titleToRemove);
        if (removed)
        {
            Console.WriteLine($"Movie '{titleToRemove}' removed successfully.");
        }
        else
        {
            Console.WriteLine($"Failed to remove movie '{titleToRemove}'. Movie not found.");
        }
    }

    public static void ViewMovies()
    {
        List<Movie> movies = MovieAccess.GetAllMovies();

        Console.WriteLine("\nAvailable Movies:");
        foreach (var movie in movies)
        {
            Console.WriteLine($"Title: {movie.Title}, Year: {movie.Year}, Genre: {movie.Genre}");
        }
        Console.WriteLine();
    }


    public static void EditMovie()
    {
        Console.WriteLine("Enter the title of the movie you want to edit:");
        string titleToEdit = Console.ReadLine();

        List<Movie> movies = MovieAccess.GetAllMovies();
        var movieToEdit = movies.FirstOrDefault(m => m.Title.Equals(titleToEdit, StringComparison.OrdinalIgnoreCase));

        if (movieToEdit != null)
        {
            Console.WriteLine("What aspect of the movie would you like to edit?");
            Console.WriteLine("1. Title");
            Console.WriteLine("2. Year");
            Console.WriteLine("3. Genre");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Enter the new title of the movie:");
                        string newTitle = Console.ReadLine();
                        movieToEdit.Title = newTitle;
                        break;
                    case 2:
                        Console.WriteLine("Enter the new year of release:");
                        if (int.TryParse(Console.ReadLine(), out int newYear))
                        {
                            movieToEdit.Year = newYear;
                        }
                        else
                        {
                            Console.WriteLine("Invalid year format. Please enter a valid year.");
                            return;
                        }
                        break;
                    case 3:
                        Console.WriteLine("Enter the new genre of the movie:");
                        string newGenre = Console.ReadLine();
                        movieToEdit.Genre = newGenre;
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        return;
                }

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


    public static void AddMovie()
    {
        Console.WriteLine("Enter the title of the movie:");
        string title = Console.ReadLine();

        Console.WriteLine("Enter the year of release:");
        if (!int.TryParse(Console.ReadLine(), out int year))
        {
            Console.WriteLine("Invalid input for year. Please enter a valid integer.");
            return;
        }

        Console.WriteLine("Enter the genre of the movie:");
        string genre = Console.ReadLine();

        Movie newMovie = new Movie(title, year, genre);
        movieManager.AddMovie(newMovie);
        Console.WriteLine("Movie added successfully.");
    }

    public static void EditRules()
    {
        Console.WriteLine("Which rule would you like to edit? (Insert rule number)");
        if (!int.TryParse(Console.ReadLine(), out int RuleNumber))
        {
            Console.WriteLine("Invalid input for rule number. Please enter a valid integer.");
            return;
        }
        RulesLogic.EditRules(RuleNumber);
    }

    public static void AddRule()
    {
        Console.WriteLine("Enter new rule:");
        string NewRule = Console.ReadLine();
        RulesLogic.AddRule(NewRule);
    }

    public static void RemoveRule()
    {
        Console.WriteLine("Which rule would you like to remove? (Insert rule number)");
        if (!int.TryParse(Console.ReadLine(), out int RuleNumber))
        {
            Console.WriteLine("Invalid input for rule number. Please enter a valid integer.");
            return;
        }
        RulesLogic.RemoveRule(RuleNumber);
    }



    public static void ViewAllUsers()
    {
        List<User> users = UserLogic.GetAllUsers();

        Console.WriteLine("\nAll Users:");
        foreach (var user in users)
        {
            Console.WriteLine($"Username: {user.Username}, Role: {user.Role}");
        }
        Console.WriteLine();
    }

    public static void AddTimeAndAuditoriumToMovie()
    {
        Console.WriteLine("Enter movie title:");
        string title = Console.ReadLine();

        Console.WriteLine("Enter display date (yyyy-MM-dd HH:mm):");
        DateTime displayDate;
        while (true)
        {
            string input = Console.ReadLine();
            if (!DateTime.TryParseExact(input, "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.None, out displayDate))
            {
                Console.WriteLine("Invalid date format. Please enter in the format: yyyy-MM-dd HH:mm");
                continue;
            }

            if (displayDate <= DateTime.Now)
            {
                Console.WriteLine("You cannot enter a date in the past or today. Please enter a future date.");
                continue;
            }

            break;
        }

        Console.WriteLine("Enter auditorium:");
        string auditorium = Console.ReadLine();

        MoviesLogic.AddTimeAndAuditorium(title, displayDate, auditorium);
    }


}