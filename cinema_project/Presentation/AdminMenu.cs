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
            Console.WriteLine("1. View Movies");
            Console.WriteLine("2. Add Movie");
            Console.WriteLine("3. Edit Movie");
            Console.WriteLine("4. Remove Movie");
            Console.WriteLine("5. View All Users");
            Console.WriteLine("6. Rules");
            Console.WriteLine("7. Logout");

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
                    ViewAllUsers();
                    break;
                case "6":
                    Rules();
                    break;
                case "7":
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
////-------------------------------------------------------------------------------------------------------------------------> Nieuwe methods
    /// <summary>
    /// Moet not getest worden
    /// </summary>
    static private void ViewCateringItems()
    {
        string filePath = "C:\\Users\\Joseph\\Documents\\GitHub\\Cinema-Project\\cinema_project\\DataSources\\cateringmenu.json";
        List<dynamic> cateringmenu = null!;
        try
        {
            string json = File.ReadAllText(filePath);
            cateringmenu = JsonConvert.DeserializeObject<List<dynamic>>(json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while reading the menu from JSON file: {ex.Message}");
        }
        Console.WriteLine();
        int num = 0;
        foreach(dynamic item in cateringmenu)
        {
            JObject itemObject = item as JObject;
            if (itemObject != null)
            {
                string Product = itemObject["product"].ToString();
                string Category = itemObject["category"].ToString();
                string Size = itemObject["size"].ToString();
                double Price = Convert.ToDouble(itemObject["price"]);
                Console.WriteLine($"{num}. Product: {Product} Category: {Category} Size: {Size} Price (in euro's): {Price}"); 
            }       
        }
    }
    /// <summary>
    /// Moet nog getest worden
    /// </summary>
    static private void AddCateringItems() 
    {
        string product = Console.ReadLine();
        string category = Console.ReadLine();
        string size = Console.ReadLine();
        double price = Convert.ToDouble(Console.ReadLine());
        string filePath = "C:\\Users\\Joseph\\Documents\\GitHub\\Cinema-Project\\cinema_project\\DataSources\\cateringmenu.json";
        List<Dictionary<string, object>> productList = ReadJsonFile(filePath);
        Dictionary<string, object> newitem = new Dictionary<string, object> {
            {"product", product},
            {"category", category},
            {"size", size},
            {"price", price},
        };
        productList.Add(newitem);
        string json = JsonConvert.SerializeObject(productList, Formatting.Indented);
        WriteJsonToFile(json, filePath);
        Console.WriteLine("New product added successfully.");
    }

    static List<Dictionary<string, object>> ReadJsonFile(string filePath)
    {
        using (StreamReader file = File.OpenText(filePath))
        {
            JsonSerializer serializer = new JsonSerializer();
            List<Dictionary<string, object>> itemlist = (List<Dictionary<string, object>>)serializer.Deserialize(file, typeof(List<Dictionary<string, object>>));
            return itemlist;
        }
    }

    static void WriteJsonToFile(string json, string filePath)
    {
        using (StreamWriter file = File.CreateText(filePath))
        {
            file.Write(json);
        }
    }
}
