using Newtonsoft.Json;

public static class AuditoriumsDataAccess
{
    public static CinemaHalls GetAllAuditoriums()
    {
        //string json = File.ReadAllText("C:\\Users\\Joseph\\Documents\\GitHub\\Cinema-Project\\cinema_project\\DataSources\\CinemaHalls.json");
        string json = File.ReadAllText("C:\\Users\\abdul\\OneDrive\\Documents\\GitHub\\Cinema-Project\\cinema_project\\DataSources\\CinemaHalls.json");
        return JsonConvert.DeserializeObject<CinemaHalls>(json);
    }
}
