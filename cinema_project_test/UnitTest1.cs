using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace cinema_project_test;


[TestClass]
public class UnitTest1
{
    private const string ConnectionString = "Data Source=ABDULRAHMAN;Initial Catalog=cinema_project;User ID=sa;Password=q1w2e3r4t5;";
    private const string MovieScheduleFilePath = "C:\\Users\\abdul\\OneDrive\\Documents\\GitHub\\Cinema-Project\\cinema_project\\DataSources\\MovieSchedule.json";
    private const string MoviesFilePath = "C:\\Users\\abdul\\OneDrive\\Documents\\GitHub\\Cinema-Project\\cinema_project\\DataSources\\movies.csv";

    private SqlConnection OpenConnection()
    {
        return new SqlConnection(ConnectionString);
    }

    // Testing DataBase Connection.
    [TestMethod]
    public void TestDataBaseConnection()
    {
        {
            bool connectionEstablished = false;
            string connectionString = UserAccess.connectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    if (connection != null && connection.State == System.Data.ConnectionState.Open)
                    {
                        connectionEstablished = true;
                        Assert.IsTrue(connectionEstablished);
                    }
                else
                    {
                        Assert.Fail();
                    }
                }
                catch (Exception ex)
                {
                    Assert.Fail(ex.Message);
                }
            }
        }
    }

    // Testing Login With Right Credentials.
    [TestMethod]
    public void TestLogin_WithRightCredentials()
    {
        string username = "customer";
        string password = "customer";

        User result  = UserAccess.Login(username, password);

        Assert.IsNotNull(result);
        Assert.AreEqual(username, result.Username);
        Assert.AreEqual(password, result.Password);
        Assert.AreEqual("user", result.Role);
    }

    // Testing Login with wrong Credentials.
    [TestMethod]
    public void TestLogin_WithWrongCredentials()
    {
        string username = "wrongUsername";
        string password = "wrongPassword";

        User result = UserAccess.Login(username, password);

        Assert.IsNull(result);
    }

    // Testing User Creation And Check If User Added To DB. 
    [TestMethod]
    public void CreateUser_ShouldReturnTrue_WhenCredentialsAreValid()
    {
        string username = "testuser";
        string password = "testpassword";
        string name = "test User";
        string email = "testuser@example.com";
        string phoneNumber = "1234567890";

        bool result = UserAccess.CreateUser(username, password, name, email, phoneNumber);

        Assert.IsTrue(result);

        // Verify the user was actually created in the database.
        using (var connection = OpenConnection())
        {
            connection.Open();
            using (var command = new SqlCommand("SELECT COUNT(*) FROM users WHERE user_name = @username", connection))
            {
                command.Parameters.AddWithValue("@username", username);
                int userCount = (int)command.ExecuteScalar();
                Assert.AreEqual(1, userCount);
            }
        }
    }

    // Testing Delete User Account By Removing  The Created Test User.
    [TestMethod]
    public void DeleteAccount_ShouldReturnTrue_WhenUserExists()
    {
        string username = "testuser";
        string password = "testpassword";
        string name = "test User";
        string email = "testuser@example.com";
        string phoneNumber = "1234567890";

        UserAccess.CreateUser(username, password, name, email, phoneNumber);


        bool result = UserAccess.DeleteAccount(username);

        Assert.IsTrue(result);

        // Verify the user was actually deleted from the database.
        using (var connection = OpenConnection())
        {
            connection.Open();
            using (var command = new SqlCommand("SELECT COUNT(*) FROM users WHERE user_name = @username", connection))
            {
                command.Parameters.AddWithValue("@username", username);
                int userCount = (int)command.ExecuteScalar();
                Assert.AreEqual(0, userCount);
            }
        }
    }

    // Test Change Account Information.
    [TestMethod]
    public void ChangeAccount_ShouldReturnTrue_WhenInformationIsUpdated()
    {
        string username = "testuser";
        string password = "testpassword";
        string name = "test User";
        string email = "testuser@example.com";
        string phoneNumber = "1234567890";

        UserAccess.CreateUser(username, password, name, email, phoneNumber);

        try
        {
            string newEmail = "newemail@example.com";
            bool result = UserAccess.ChangeAccount(newEmail, 1, username); // Choice 1: Update email

            Assert.IsTrue(result);

            // Verify the user's email was updated in the database.
            using (var connection = OpenConnection())
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT email FROM users WHERE user_name = @username", connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    string updatedEmail = (string)command.ExecuteScalar();
                    Assert.AreEqual(newEmail, updatedEmail);
                }
            }
        }
        finally
        {
            // Ensure the user is deleted from the database.
            using (var connection = OpenConnection())
            {
                connection.Open();
                using (var command = new SqlCommand("DELETE FROM users WHERE user_name = @username", connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.ExecuteNonQuery();
                }
            }
        }
    }

    // Testing Add Movie To Schedule In Json File Methode.
    [TestMethod]
    public void WriteToMovieSchedule_ShouldAddMovieEntry_WhenDataIsValid()
    {
        string filename = "TestMovie.mp4";
        string movieTitle = "Test Movie";
        DateTime displayTime = DateTime.Now;
        string auditorium = "Auditorium 1";
        decimal lowPrice = 5.0m;
        decimal mediumPrice = 7.5m;
        decimal highPrice = 10.0m;
        decimal handiPrice = 4.0m;

        try
        {
            MovieScheduleAccess.WriteToMovieSchedule(filename, movieTitle, displayTime, auditorium, lowPrice, mediumPrice, highPrice, handiPrice);

            string json = File.ReadAllText(MovieScheduleFilePath);
            var movieSchedule = JsonConvert.DeserializeObject<List<Movie>>(json);

            Assert.IsNotNull(movieSchedule);
            Assert.IsTrue(movieSchedule.Exists(m => m.movieTitle == movieTitle && m.auditorium == auditorium && m.displayTime == displayTime));

            var addedMovie = movieSchedule.Find(m => m.movieTitle == movieTitle && m.auditorium == auditorium && m.displayTime == displayTime);
            Assert.AreEqual(filename, addedMovie.filename);
            Assert.AreEqual(lowPrice, addedMovie.LowPrice);
            Assert.AreEqual(mediumPrice, addedMovie.MediumPrice);
            Assert.AreEqual(highPrice, addedMovie.HighPrice);
            Assert.AreEqual(handiPrice, addedMovie.HandicapPrice);
        }
        finally
        {
            // Remove the movie entry added by the test.
            var movieSchedule = JsonConvert.DeserializeObject<List<Movie>>(File.ReadAllText(MovieScheduleFilePath));

            if (movieSchedule != null)
            {
                movieSchedule.RemoveAll(m => m.movieTitle == movieTitle && m.auditorium == auditorium && m.displayTime == displayTime);

                string updatedJson = JsonConvert.SerializeObject(movieSchedule, Formatting.Indented);
                File.WriteAllText(MovieScheduleFilePath, updatedJson);
            }
        }
    }

    // Testing Write Movies To CSV Methode.
    [TestMethod]
    public void WriteMoviesToCSV_ShouldAddMovieEntry_WhenDataIsValid()
    {
        var movies = new List<Movie>
            {
                new Movie("Test Movie")
                {
                    Year = 2024,
                    Genre = "Action",
                    displayTime = DateTime.Now,
                    auditorium = "Auditorium 1"
                }
            };

        var originalLines = File.ReadAllLines(MoviesFilePath).ToList();

        try
        {
            MovieAccess.WriteMoviesToCSV(movies);

            string[] lines = File.ReadAllLines(MoviesFilePath);
            string lastLine = lines.Last();

            Assert.AreEqual("Test Movie,2024,Action," + movies[0].displayTime.ToString("yyyy-MM-dd HH:mm") + ",Auditorium 1", lastLine);
        }
        finally
        {
            //Restore the original contents excluding the test movie
            File.WriteAllLines(MoviesFilePath, originalLines.Where(line => !line.Contains("Test Movie,2024,Action,")));
        }
    }
}