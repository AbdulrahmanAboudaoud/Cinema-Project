using System.Data.SqlClient;
using System.Text.RegularExpressions;

public static class UserAccess
{
    public static string connectionString = "Data Source=localhost;Initial Catalog=cinema_db;User ID=sa;Password=123456;";

    public static SqlConnection OpenConnection()
    {
        SqlConnection connection = new SqlConnection(connectionString);
        connection.Open();
        return connection;
    }

    public static void CloseConnection(SqlConnection connection)
    {
        connection.Close();
    }

    
}
