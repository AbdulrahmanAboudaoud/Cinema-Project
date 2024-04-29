public static class CateringLogic
{


    public static void AddMenuItem(Dictionary<string, object> newItem)
    {
        List<Dictionary<string, object>> menu = CateringAccess.LoadMenuFromJson(CateringAccess.cateringmenu);
        menu.Add(newItem);
        CateringAccess.SaveMenuToJson(menu, CateringAccess.cateringmenu);
        Console.WriteLine("Menu item added successfully!");
    }
    public static void DeleteItemFromMenu(int item_id)
    {
        List<Dictionary<string, object>> menu = CateringAccess.LoadMenuFromJson(CateringAccess.cateringmenu);
        bool found = false;
        for (int i = 0; i < menu.Count; i++)
        {
            if (menu[i].ContainsKey("id") && Convert.ToInt64(menu[i]["id"]) == item_id)
            {
                menu.RemoveAt(i);
                found = true;
                break;
            }
        }

        if (found)
        {
            CateringAccess.SaveMenuToJson(menu, CateringAccess.cateringmenu);
            Console.WriteLine("Menu item deleted successfully!");
        }
        else
        {
            Console.WriteLine("Invalid ID");
        }
    }
    public static void SortItems()
    {
        int i = 1;
        List<Dictionary<string, object>> menu = CateringAccess.LoadMenuFromJson(CateringAccess.cateringmenu);
        foreach (var item in menu)
        {
            item["id"] = i;
            i++;
        }
        CateringAccess.SaveMenuToJson(menu, CateringAccess.cateringmenu);
    }

    public static void ViewItems(string choice, string filePath)
    {
        int num = 0;
        List<Dictionary<string, object>> items = CateringAccess.LoadMenuFromJson(filePath);
        SortItems();
        foreach (var item in items)
        {
            SortItems();
            CateringItem.Food_ID = Convert.ToInt32(item["id"]);
            CateringItem.Product = (string)item["product"];
            CateringItem.Category = (string)item["category"];
            CateringItem.Size = (string)item["size"];
            CateringItem.Price = Convert.ToDouble(item["price"]);

            if (CateringItem.Category == choice)
            {
                num++;
                Console.WriteLine($"{num}. Product: {CateringItem.Product} Size: {CateringItem.Size} Price: {CateringItem.Price} euro's");
            }
            else if (choice == "all")
            {
                num++;
                Console.WriteLine($"{num}. Product: {CateringItem.Product} Product ID: {CateringItem.Food_ID}, Size: {CateringItem.Size} Price: {CateringItem.Price} euro's");
            }
        }
        Console.WriteLine("");
    }

    public static void AddCateringItem()
    {
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
            {"id", 0 },
            { "product", productname },
            { "category", productcatstring },
            { "size", size },
            { "price", price },
        };
        AddMenuItem(newItem);
        SortItems();
    }


    public static void RemoveItem()
    {
        ViewItems("all", CateringAccess.cateringmenu);
        Console.WriteLine("Which Item would you like to remove? (by ID)");
        int idinput = Convert.ToInt32(Console.ReadLine());
        DeleteItemFromMenu(idinput);
    }
}