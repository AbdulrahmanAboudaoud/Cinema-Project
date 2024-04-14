using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json.Linq;

public static class CateringMenu
{
    private static readonly string filePath = "C:\\Users\\Joseph\\Documents\\GitHub\\Cinema-Project\\cinema_project\\DataSources\\cateringmenu.json";
    public static string Product { get; set; } 
    public static string Category { get; set; }
    public static string Size { get; set; }
    public static double Price { get; set; }
    public static List<dynamic> LoadMenuFromJson(string file)
    {
        List<dynamic> menu = null!;

        try
        {
            string json = File.ReadAllText(file);
            menu = JsonConvert.DeserializeObject<List<dynamic>>(json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while reading the menu from JSON file: {ex.Message}");
        }
        return menu;
    }
    public static void StartMenu(ref User loggedInUser)
    {
        bool browsemenu = false;
        while (!browsemenu)
        {
            Console.WriteLine();
            Console.WriteLine("1. Browse Food");
            Console.WriteLine("2. Browse Drinks");
            Console.WriteLine("3. Go back to user menu\n");
            string userinput = Console.ReadLine();
            switch (userinput)
            {
                case "1":
                ViewFood();
                break;
                case "2":
                ViewDrinks();
                break;
                case "3":
                Console.Clear();
                browsemenu = true;
                UserMenu.Start(ref loggedInUser);
                break;
                default:
                Console.WriteLine("Invalid Input");
                break;
            }
        }
    }
    public static void ViewDrinks()
    {
        Console.WriteLine();
        int num = 0;
        List<dynamic> drinks = LoadMenuFromJson(filePath);
        foreach(dynamic item in drinks)
        {
            JObject itemObject = item as JObject;
            if (itemObject != null)
            {
                Product = itemObject["product"].ToString();
                Category = itemObject["category"].ToString();
                Size = itemObject["size"].ToString();
                Price = Convert.ToDouble(itemObject["price"]);

                if (Category == "Drink")
                {
                    num++;
                    Console.WriteLine($"{num}. Product: {Product} Size: {Size} Price: {Price} euro's");
                }
            }
        }
        Console.WriteLine("");
    }
    public static void ViewFood()
    {
        Console.WriteLine();
        int num = 0;
        List<dynamic> drinks = LoadMenuFromJson(filePath);
        foreach(dynamic item in drinks)
        {
            JObject itemObject = item as JObject;
            if (itemObject != null)
            {
                Product = itemObject["product"].ToString();
                Category = itemObject["category"].ToString();
                Size = itemObject["size"].ToString();
                Price = Convert.ToDouble(itemObject["price"]);

                if (Category == "Food")
                {
                    num++;
                    Console.WriteLine($"{num}. Product: {Product} Size: {Size} Price: {Price} euro's");
                }
            }
        }
        Console.WriteLine("");
    }
}