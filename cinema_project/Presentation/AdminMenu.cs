using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
static class AdminMenu
{
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
                    MoviesInterface(); 
                    break;
                case "2":
                    AdminLogic.ViewAllUsers();
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
                    Console.WriteLine("Logging out...");
                    Console.WriteLine("You have been logged out.");
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
        Console.WriteLine("Choose search criteria:");
        Console.WriteLine("1. Search by film");
        Console.WriteLine("2. Search by year");
        Console.WriteLine("3. Search by genre");

        string searchOption = Console.ReadLine();

        switch (searchOption)
        {
            case "1":
                SearchLogic.SearchByFilm();
                break;
            case "2":
                SearchLogic.SearchByYear();
                break;
            case "3":
                SearchLogic.SearchByGenre();
                break;
            default:
                Console.WriteLine("Invalid option.");
                break;
        }
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
            Console.WriteLine("5. Add Time and Auditorium to a Movie");
            Console.WriteLine("6. Back to Main Menu\n");

            string input = Console.ReadLine()!;
            switch (input)
            {
                case "1":
                    AdminLogic.ViewMovies();
                    break;
                case "2":
                    AdminLogic.AddMovie();
                    break;
                case "3":
                    AdminLogic.EditMovie();
                    break;
                case "4":
                    AdminLogic.RemoveMovie();
                    break;
                case "5":
                    AdminLogic.AddTimeAndAuditoriumToMovie();
                    break;
                case "6":
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
                RulesLogic.ViewAllRules();
                break;
            case "2":
                AdminLogic.EditRules();
                break;
            case "3":
                AdminLogic.AddRule();
                break;
            case "4":
                AdminLogic.RemoveRule();
                break;
            default:
                Console.WriteLine("Invalid input");
                break;
        }
    }

    




    private static void cateringeditmenu()
    {
        Console.WriteLine("1. Add Items");
        Console.WriteLine("2. View Items");
        Console.WriteLine("3. Remove Items");
        Console.WriteLine("4. Sort Items");
        Console.WriteLine("5. Cancel");
        bool exitmenu = false;
        string? cateringchoice = Console.ReadLine();

        while(!exitmenu)
            switch ( cateringchoice )
            {
                case "1":
                    CateringLogic.AddCateringItem();
                    exitmenu = true;
                    break;
                case "2":
                    CateringLogic.ViewItems("all", CateringAccess.cateringmenu);
                    exitmenu = true;
                    break;
                case "3":
                    CateringLogic.RemoveItem();
                    exitmenu = true;
                    break;
                case "4":
                    CateringLogic.SortItems();
                    exitmenu = true;
                    break;
                case "5":
                    exitmenu =true;
                    break;
                default:
                    Console.WriteLine("Invalid Input");
                    break;
            }
    }
}