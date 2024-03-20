using System;
using System.Data.SqlClient;

public class User
{
    public string Username;
    public string Password;
    public string Role;

    public User(string username, string password)
    {
        Username = username;
        Password = password;
    }

    // Method to attempt login
    public static (bool, string) Login(string username, string password, string connectionString)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                // Open the connection.
                connection.Open();

                // Execute SQL query to fetch data from users table.
                string query = $"SELECT role FROM users WHERE user_name = '{username}' AND password = '{password}'";

                // Create a SqlCommand object.
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Execute the query and obtain a SqlDataReader
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Check if there are rows in the result set.
                        if (reader.HasRows)
                        {
                            // Read the role from the result set.
                            reader.Read();
                            string role = reader.GetString(0);

                            // Login successful.
                            return (true, role);
                        }
                        else
                        {
                            // Login failed.
                            return (false, "");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error executing SQL query: " + ex.Message);
                // Login failed due to error.
                return (false, "");
            }
        }
    }
}
