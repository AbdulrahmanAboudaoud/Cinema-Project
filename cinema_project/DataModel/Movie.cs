public class Movie
{
	public string Title { get; set; }
	public int Year { get; set; }
	public string Genre { get; set; }

	public Movie(string title, int year, string genre)
	{
		Title = title;
		Year = year;
		Genre = genre;
	}
}
