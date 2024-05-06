﻿using Newtonsoft.Json.Linq;
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
            Console.WriteLine("6. View all reservations");
            Console.WriteLine("7. Logout\n");
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
                    AdminLogic.DisplayAllReservations();
                    break;
                case "7":
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
        bool exitRequested = false;

        while (!exitRequested)
        {
            Console.WriteLine("Choose search criteria:");
            Console.WriteLine("1. Search by film");
            Console.WriteLine("2. Search by year");
            Console.WriteLine("3. Search by genre");
            Console.WriteLine("4. Back to Main Menu\n");

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
                case "4":
                    exitRequested = true;
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
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
        bool exitRequested = false;

        while (!exitRequested)
        {
            Console.WriteLine("1. View rules");
            Console.WriteLine("2. Edit rules");
            Console.WriteLine("3. Add rule");
            Console.WriteLine("4. Remove rule");
            Console.WriteLine("5. Back to Main Menu\n");
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
                case "5":
                    exitRequested = true;
                    break;
                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
        }
    }

    private static void cateringeditmenu()
    {
        Console.WriteLine("1. Add Items");
        Console.WriteLine("2. View Items");
        Console.WriteLine("3. Remove Items");
        Console.WriteLine("4. Sort Items");
        Console.WriteLine("5. Edit Items");
        Console.WriteLine("6. Back to main menu\n");
        bool exitmenu = false;
        string? cateringchoice = Console.ReadLine();

        while(!exitmenu)
            switch (cateringchoice)
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
                    CateringLogic.ViewItems("all", CateringAccess.cateringmenu);
                    Console.WriteLine();
                    Console.WriteLine("Which Item would you like to edit? (by ID)");
                    int idinput = Convert.ToInt32(Console.ReadLine());
                    int? foundId = CateringLogic.IDFound(idinput);
                    if (foundId.HasValue)
                    {
                        Console.WriteLine("Which item would you like to edit?");
                        Console.WriteLine("Name (N)");
                        Console.WriteLine("Size (S)");
                        Console.WriteLine("Price (P)");
                        string cateringchoicen = Console.ReadLine().ToUpper();
                        string arg = "";
                        switch (cateringchoicen)
                        {
                            case "N":
                                arg = "product";
                                Console.WriteLine("What would you like to change the name to?");
                                string newname = Console.ReadLine();
                                CateringLogic.EditItem(idinput, arg, newname);
                                break;
                            case "S":
                                arg = "size";
                                Console.WriteLine("What would you like to change the size to?");
                                string newsize = Console.ReadLine();
                                CateringLogic.EditItem(idinput, arg, newsize);
                                break;
                            case "P":
                                arg = "price";
                                Console.WriteLine("What would you like to change the price to?");
                                double newprice = Convert.ToDouble(Console.ReadLine());
                                CateringLogic.EditItem(idinput, arg, newprice);
                                break;
                            default:
                                Console.WriteLine("Invalid Input");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Couldn't find the ID, please try again");
                    }
                    exitmenu = true;
                    break;
                case "6":
                    exitmenu = true;
                    break;
                default:
                    Console.WriteLine("Invalid Input");
                    break;
            }
    }
}