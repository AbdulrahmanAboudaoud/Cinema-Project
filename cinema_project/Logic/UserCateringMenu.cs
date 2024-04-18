using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
public static class UserCateringMenu
{
    public static void ViewItems(string choice, string filePath)
    {
        int num = 0;
        List<Dictionary<string, object>> items = LoadMenuFromJson(filePath);
        foreach (var item in items)
        {
            CateringMenu.Food_ID = Convert.ToInt32(item["id"]);
            CateringMenu.Product = (string)item["product"];
            CateringMenu.Category = (string)item["category"];
            CateringMenu.Size = (string)item["size"];
            CateringMenu.Price = Convert.ToDouble(item["price"]);

            if (CateringMenu.Category == choice)
            {
                num++;
                Console.WriteLine($"{num}. Product: {CateringMenu.Product} Size: {CateringMenu.Size} Price: {CateringMenu.Price} euro's");
            }
            else if (choice == "all")
            {
                num++;
                Console.WriteLine($"{num}. Product: {CateringMenu.Product} Product ID: {CateringMenu.Food_ID}, Size: {CateringMenu.Size} Price: {CateringMenu.Price} euro's");
            }
        }
        Console.WriteLine("");
    }
    public static List<Dictionary<string, object>> LoadMenuFromJson(string file)
    {
        if (!File.Exists(file))
        {
            Console.WriteLine("Menu file does not exist.");
            return new List<Dictionary<string, object>>();
        }

        string json = File.ReadAllText(file);
        return JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json) ?? new List<Dictionary<string, object>>();
    }
}