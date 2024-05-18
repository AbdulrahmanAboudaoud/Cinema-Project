using System.Diagnostics;

public static class AdminLogic
{
    static private MoviesLogic movieManager = new MoviesLogic();

    public static void DisplayAllReservations()
    {
        List<Reservation> userReservations = ReservationAccess.LoadAllReservations();
        ReservationHistory.DisplayReservationHistory("", userReservations);
    }


    public static void RemoveMovie()
    {
        ViewMovies();
        Console.WriteLine();
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
            Console.WriteLine($"Title: {movie.movieTitle}, Year: {movie.Year}, Genre: {movie.Genre}");
        }
        Console.WriteLine();
    }


   /* public static void EditMovie()
    {
        ViewMovies();
        Console.WriteLine();

        Console.WriteLine("Enter the title of the movie you want to edit:");
        string titleToEdit = Console.ReadLine();

        List<Movie> movies = MovieAccess.GetAllMovies();
        var movieToEdit = movies.FirstOrDefault(m => m.movieTitle.Equals(titleToEdit, StringComparison.OrdinalIgnoreCase));

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
                        movieToEdit.movieTitle = newTitle;
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
    }*/


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
        RulesLogic.ViewAllRules();
        Console.WriteLine();
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
        RulesLogic.ViewAllRules();
        Console.WriteLine();
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
        List<User> users = UserAccess.GetAllUsers();

        Console.WriteLine("\nAll Users:");
        foreach (var user in users)
        {
            Console.WriteLine($"Username: {user.Username}, Role: {user.Role}");
        }
        Console.WriteLine();
    }

    public static void AddTimeAndAuditoriumToMovie()
    {
        ViewMovies();
        Console.WriteLine();

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

    public static void RemoveUser()
    {
        Console.WriteLine("Enter the username of the user you want to remove:");
        string usernameToRemove = Console.ReadLine();

        bool removed = UserAccess.RemoveUser(usernameToRemove);
        if (removed)
        {
            Console.WriteLine($"User '{usernameToRemove}' removed successfully.");
        }
        else
        {
            Console.WriteLine($"Failed to remove user '{usernameToRemove}'. User not found.");
        }
    }

    public static void ViewAllUserData()
    {
        List<UserData> users = UserAccess.GetAllUserData();

        Console.WriteLine("All Users:");
        foreach (var user in users)
        {
            Console.WriteLine($"Username: {user.UserName}, Name: {user.Name}, Password: {user.Password}, Email: {user.Email}, Phone Number: {user.PhoneNumber}");
        }
    }


    public static void EditUserAccount()
    {
        Console.WriteLine("Enter the username of the user you want to edit:");
        string currentUsername = Console.ReadLine();

        UserData selectedUser = UserAccess.GetAllUserData().FirstOrDefault(u => u.UserName == currentUsername);

        if (selectedUser == null)
        {
            Console.WriteLine("User not found.");
            return;
        }

        Console.WriteLine($"Selected User: Username: {selectedUser.UserName}, Name: {selectedUser.Name}, Email: {selectedUser.Email}, Phone Number: {selectedUser.PhoneNumber}");
        Console.WriteLine("Which field do you want to edit?");
        Console.WriteLine("1. Username");
        Console.WriteLine("2. Password");
        Console.WriteLine("3. Name");
        Console.WriteLine("4. Email");
        Console.WriteLine("5. Phone Number");

        string choice = Console.ReadLine();
        string newValue = string.Empty;

        switch (choice)
        {
            case "1":
                Console.WriteLine("Enter new username:");
                newValue = Console.ReadLine();
                UserAccess.UpdateUser(currentUsername, newValue, selectedUser.Password, selectedUser.Name, selectedUser.Email, selectedUser.PhoneNumber);
                break;
            case "2":
                Console.WriteLine("Enter new password:");
                newValue = Console.ReadLine();
                UserAccess.UpdateUser(currentUsername, selectedUser.UserName, newValue, selectedUser.Name, selectedUser.Email, selectedUser.PhoneNumber);
                break;
            case "3":
                Console.WriteLine("Enter new name:");
                newValue = Console.ReadLine();
                UserAccess.UpdateUser(currentUsername, selectedUser.UserName, selectedUser.Password, newValue, selectedUser.Email, selectedUser.PhoneNumber);
                break;
            case "4":
                Console.WriteLine("Enter new email:");
                newValue = Console.ReadLine();
                UserAccess.UpdateUser(currentUsername, selectedUser.UserName, selectedUser.Password, selectedUser.Name, newValue, selectedUser.PhoneNumber);
                break;
            case "5":
                Console.WriteLine("Enter new phone number:");
                newValue = Console.ReadLine();
                UserAccess.UpdateUser(currentUsername, selectedUser.UserName, selectedUser.Password, selectedUser.Name, selectedUser.Email, newValue);
                break;
            default:
                Console.WriteLine("Invalid choice.");
                break;
        }

        Console.WriteLine("User updated successfully.");
    }


}