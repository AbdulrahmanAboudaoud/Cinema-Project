using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

public static class CateringMenu
{

    public static string Choice { get; set; }
    public static void StartMenu(ref User loggedInUser)
    
    {
        bool browsemenu = false;
        while (!browsemenu)
        {
            Console.WriteLine("1. Browse Food");
            Console.WriteLine("2. Browse Drinks");
            Console.WriteLine("3. Go back to user menu");
            string userinput = Console.ReadLine();
            switch (userinput)
            {
                case "1":
                    Choice = "Food";
                    CateringLogic.ViewItems(Choice, CateringAccess.cateringmenu);      
                    break;
                case "2":
                    Choice = "Drink";
                    CateringLogic.ViewItems(Choice, CateringAccess.cateringmenu);
                    break;
                case "3":
                    Console.Clear();
                    browsemenu = true;
                    break;
                default:
                    Console.WriteLine("Invalid Input");
                    break;
            }
        }
    }
}