using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
public static class AdminCateringMenu
{
    static private string cateringmenu = "C:\\Users\\Joseph\\Documents\\GitHub\\Cinema-Project\\cinema_project\\DataSources\\cateringmenu.json";

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

    public static void AddMenuItem(Dictionary<string, object> newItem)
    {
        List<Dictionary<string, object>> menu = LoadMenuFromJson(cateringmenu);
        menu.Add(newItem);
        SaveMenuToJson(menu, cateringmenu);
        Console.WriteLine("Menu item added successfully!");
    }
    public static void DeleteItemFromMenu(int item_id)
    {
        List<Dictionary<string, object>> menu = LoadMenuFromJson(cateringmenu);
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
            SaveMenuToJson(menu, cateringmenu);
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
        List<Dictionary<string, object>> menu = LoadMenuFromJson(cateringmenu);
        foreach (var item in menu)
        {
            item["id"] = i;
            i++;
        }
        SaveMenuToJson(menu, cateringmenu);
    }

    public static void SaveMenuToJson(List<Dictionary<string, object>> menu, string file)
    {
        try
        {
            string json = JsonConvert.SerializeObject(menu, Formatting.Indented);
            File.WriteAllText(file, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while saving the menu to JSON file: {ex.Message}");
        }
    }
}