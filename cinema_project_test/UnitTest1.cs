using System.Data.SqlClient;
namespace cinema_project_test;


[TestClass]
public class UnitTest1
{
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
    public void AddMovieTest()
    {
        string titleInput = "Test Movie";
        string yearInput = "2022";
        string genreInput = "Action";
        // Redirect console input
        StringReader stringReader = new StringReader($"{titleInput}\n{yearInput}\n{genreInput}\n");
        Console.SetIn(stringReader);
        // Capture console output
        StringWriter stringWriter = new StringWriter();
        Console.SetOut(stringWriter);
        // Act
        AdminLogic.AddMovie();
        // Assert
        string expectedOutput = "Enter the title of the movie:\r\nEnter the year of release:\r\nEnter the genre of the movie:\r\nMovie added successfully.\r\n";
        Assert.AreEqual(expectedOutput, stringWriter.ToString());
        // Cleanup
        stringReader.Dispose();
        stringWriter.Dispose();
    }
}