using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public static class AuditoriumsDataAccess
{
    public static string jsonFolderPath = @"C:\Users\Gebruiker\OneDrive - Hogeschool Rotterdam\Github\Cinema-Project\cinema_project\DataSources";
    public static CinemaHalls GetAllAuditoriums()
    {
        string json = File.ReadAllText(Path.Combine(jsonFolderPath, "CinemaHalls.json"));
        return JsonConvert.DeserializeObject<CinemaHalls>(json);
    }

    public static JObject GetAuditoriumData(string fileName)
    {
        try
        {
            string jsonContent = File.ReadAllText(Path.Combine(jsonFolderPath, fileName));
            return JsonConvert.DeserializeObject<JObject>(jsonContent);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading auditorium data: {ex.Message}");
            return null;
        }
    }

    public static void UpdateAuditoriumLayoutFile(string movieTitle, DateTime displayDate, string auditorium, string seatNumber, bool reserved)
    {
        try
        {
            string fileName = $"{movieTitle}-{displayDate:yyyyMMdd-HHmm}-{auditorium}.json";
            string filePath = Path.Combine(jsonFolderPath, fileName);

            string jsonContent = File.ReadAllText(filePath);
            var auditoriumData = JsonConvert.DeserializeObject<JObject>(jsonContent);

            var seatToUpdate = auditoriumData["auditoriums"][0]["layout"]
                .SelectMany(row => row)
                .FirstOrDefault(seat => seat["seat"].ToString() == seatNumber);

            if (seatToUpdate != null)
            {
                seatToUpdate["reserved"] = reserved.ToString().ToLower();
                File.WriteAllText(filePath, JsonConvert.SerializeObject(auditoriumData));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating auditorium layout file: {ex.Message}");
        }
    }

    public static void UpdateAuditoriumLayoutFile(string movieTitle, DateTime displayDate, string auditoriumName, string oldSeatNumber, string newSeatNumber, bool reserved)
    {
        try
        {
            string fileName = $"{movieTitle}-{displayDate:yyyyMMdd-HHmm}-{auditoriumName}.json";
            string filePath = Path.Combine(jsonFolderPath, fileName);

            string jsonContent = File.ReadAllText(filePath);
            var auditoriumData = JsonConvert.DeserializeObject<JObject>(jsonContent);

            var oldSeatToUpdate = auditoriumData["auditoriums"][0]["layout"]
                .SelectMany(row => row)
                .FirstOrDefault(seat => seat["seat"].ToString() == oldSeatNumber);

            if (oldSeatToUpdate != null)
            {
                oldSeatToUpdate["reserved"] = "false"; 
            }

            var newSeatToUpdate = auditoriumData["auditoriums"][0]["layout"]
                .SelectMany(row => row)
                .FirstOrDefault(seat => seat["seat"].ToString() == newSeatNumber);

            if (newSeatToUpdate != null)
            {
                newSeatToUpdate["reserved"] = "true"; 
            }

            string updatedJson = JsonConvert.SerializeObject(auditoriumData, Formatting.Indented);

            File.WriteAllText(filePath, updatedJson);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating auditorium layout file: {ex.Message}");
        }
    }

    public static void ReserveSeatAndUpdateFile(JObject auditoriumData, string seatNumber, string fileName)
    {
        var auditorium = auditoriumData["auditoriums"][0];
        foreach (var row in auditorium["layout"])
        {
            foreach (var seat in row)
            {
                if (seat["seat"].ToString() == seatNumber)
                {
                    seat["reserved"] = "true";

                    string updatedJson = JsonConvert.SerializeObject(auditoriumData, Formatting.Indented);

                    try
                    {
                        File.WriteAllText(fileName, updatedJson);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error updating JSON file: {ex.Message}");
                    }

                    return;
                }
            }
        }
    }
}
