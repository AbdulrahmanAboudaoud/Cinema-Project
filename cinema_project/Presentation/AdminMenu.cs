using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
static class AdminMenu
{
    static private MovieManager movieManager = new MovieManager();
    static public void Start()
    {
        bool logoutRequested = false;

        while (!logoutRequested)
        {
            Console.WriteLine();
            Console.WriteLine("1. Movies");
            Console.WriteLine("2. View All Users");
            Console.WriteLine("3. Rules");
            Console.WriteLine("4. Search And Filter Movies");
            Console.WriteLine("5. Edit Catering Menu");
            Console.WriteLine("6. Logout\n");
            string input = Console.ReadLine()!;
            switch (input)
            {
                case "1":
                    MoviesInterface(); // Redirect to the movies interface
                    break;
                case "2":
                    ViewAllUsers();
                    break;
                case "3":
                    Rules();
                    break;
                case "4":
                    SearchMovies();
                    break;
                case "5":
                    cateringeditmenu();
                    break;
                case "6":
                    Logout();
                    Menu.Start();
                    logoutRequested = true;
                    break;
                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
        }
    }

    private static void SearchMovies()
    {
        SearchManager.SearchMovies();
    }

    static private void MoviesInterface()
    {
        bool exitRequested = false;

        while (!exitRequested)
        {
            Console.WriteLine();
            Console.WriteLine("1. View All Movies");
            Console.WriteLine("2. Add Movie");
            Console.WriteLine("3. Edit Movie");
            Console.WriteLine("4. Remove Movie");
            Console.WriteLine("5. Back to Main Menu\n");

            string input = Console.ReadLine()!;
            switch (input)
            {
                case "1":
                    ViewMovies();
                    break;
                case "2":
                    AddMovie();
                    break;
                case "3":
                    EditMovie();
                    break;
                case "4":
                    RemoveMovie();
                    break;
                case "5":
                    //AdminMenu.Start();
                    exitRequested = true;
                    break;
                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
        }
    }

    static private void Rules()
    {
        Console.WriteLine("1. View rules");
        Console.WriteLine("2. Edit rules");
        Console.WriteLine("3. Add rule");
        Console.WriteLine("4. Remove rule");
        string choice = Console.ReadLine().Trim();
        switch (choice)
        {
            case "1":
                RulesManager.ViewAllRules();
                break;
            case "2":
                EditRules();
                break;
            case "3":
                AddRule();
                break;
            case "4":
                RemoveRule();
                break;
            default:
                Console.WriteLine("Invalid input");
                break;
        }
    }

    static private void EditRules()
    {
        Console.WriteLine("Which rule would you like to edit? (Insert rule number)");
        int RuleNumber = Convert.ToInt32(Console.ReadLine());
        RulesManager.EditRules(RuleNumber);
    }

    static private void AddRule()
    {
        Console.WriteLine("Enter new rule:");
        string NewRule = Console.ReadLine();
        RulesManager.AddRule(NewRule);
    }

    static private void RemoveRule()
    {
        Console.WriteLine("Which rule would you like to remove? (Insert rule number)");
        int RuleNumber = Convert.ToInt32(Console.ReadLine());
        RulesManager.RemoveRule(RuleNumber);    
    }

    static private void ViewMovies()
    {
        MovieManager movieManager = new MovieManager();
        List<Movie> movies = movieManager.GetAllMovies();

        Console.WriteLine("\nAvailable Movies:");
        foreach (var movie in movies)
        {
            Console.WriteLine($"Title: {movie.Title}, Year: {movie.Year}, Genre: {movie.Genre}");
        }
        Console.WriteLine();
    }

    static private void EditMovie()
    {
        Console.WriteLine("Enter the title of the movie you want to edit:");
        string titleToEdit = Console.ReadLine();

        Console.WriteLine("Enter the new title of the movie:");
        string newTitle = Console.ReadLine();

        Console.WriteLine("Enter the new year of release:");
        int newYear = int.Parse(Console.ReadLine());

        Console.WriteLine("Enter the new genre of the movie:");
        string newGenre = Console.ReadLine();

        Movie updatedMovie = new Movie(newTitle, newYear, newGenre);
        movieManager.EditMovie(titleToEdit, updatedMovie);
    }

    static private void AddMovie()
    {
        Console.WriteLine("Enter the title of the movie:");
        string title = Console.ReadLine();

        Console.WriteLine("Enter the year of release:");
        int year = int.Parse(Console.ReadLine());

        Console.WriteLine("Enter the genre of the movie:");
        string genre = Console.ReadLine();

        Movie newMovie = new Movie(title, year, genre);
        movieManager.AddMovie(newMovie);
        Console.WriteLine("Movie added successfully.");
    }

    static private void ViewAllUsers()
    {
        //UserManager userManager = new UserManager();
        List<User> users = UserManager.GetAllUsers();

        Console.WriteLine("\nAll Users:");
        foreach (var user in users)
        {
            Console.WriteLine($"Username: {user.Username}, Role: {user.Role}");
        }
        Console.WriteLine();
    }

    static private void Logout()
    {
        Console.WriteLine("Logging out...");
        Console.WriteLine("You have been logged out.");
    }

    static private void RemoveMovie()
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
    private static void cateringeditmenu()
    {
        Console.WriteLine("1. Add Items");
        Console.WriteLine("2. View Items");
        Console.WriteLine("3. Edit Items");
        Console.WriteLine("4. Remove Items");
        Console.WriteLine("5. Cancel");
        bool exitmenu = false;
        string? cateringchoice = Console.ReadLine();
        while(!exitmenu)
            switch ( cateringchoice )
            {
                case "1":
                string productcatstring = null;
                bool correctinput = false;
                Console.WriteLine("Product name?");
                string productname = Console.ReadLine();
                Console.WriteLine("Food Category? (F or D)");
                char productcat = Console.ReadKey().KeyChar;
                Console.WriteLine("Size?");
                string size = Console.ReadLine();
                Console.WriteLine("Price?");
                double price = Convert.ToDouble(Console.ReadLine());
                while (!correctinput)
                {
                    if (productcat == 'f' || productcat == 'F')
                    {
                        productcatstring = "Food";
                        correctinput = true;
                    }
                    else if (productcat == 'd' || productcat == 'D')
                    {
                        productcatstring = "Drink";
                        correctinput = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input");
                        productcat = Console.ReadKey().KeyChar;
                    }
                } 
                Dictionary<string, object> newItem = new Dictionary<string, object>
                {
                    { "product", productname },
                    { "category", productcatstring },
                    { "size", size },
                    { "price", price },
                };
                AdminCateringMenu.AddMenuItem(newItem);     
                break;
                case "2":
                string filePath = "C:/Users/wikto/OneDrive/Documents/GitHub/Cinema-Project/cinema_project/DataSources/cateringmenu.json";
                UserCateringMenu.ViewItems("all",filePath);
                exitmenu = true;
                break;
                case "3":
                break;
                case "4":
                break;
                case "5":
                exitmenu = true;
                break;
            }
    }
}