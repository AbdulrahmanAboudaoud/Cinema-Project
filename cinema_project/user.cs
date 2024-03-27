using System;
using System.Data.SqlClient;

public class User
{
    public string Username;
    public string Password;
    public string Role;

    public User(string username, string password, string role)
    {
        Username = username;
        Password = password;
        Role = role;
    }

    // Method to attempt login
    public static User Login(string username, string password, string connectionString)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                // Open the connection.
                connection.Open();

                // Execute SQL query to fetch data from users table.
                string query = "SELECT role FROM users WHERE user_name = @username AND password = @password";

                // Create a SqlCommand object.
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add parameters to the query to prevent SQL injection.
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);

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
                            return new User(username, password, role);
                        }
                        else
                        {
                            // Login failed.
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error executing SQL query: " + ex.Message);
                // Login failed due to error.
                return null;
            }
        }
    }

    public static bool CreateUser(string username, string password, string name, string email, string phoneNumber, string connectionString)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                // Open the connection.
                connection.Open();

                // Execute SQL query to insert new user data into users table.
                string query = "INSERT INTO users (user_name, password, name, email, phone_number, role) " +
                               "VALUES (@username, @password, @name, @email, @phoneNumber, 'user')";

                // Create a SqlCommand object.
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add parameters to the query to prevent SQL injection.
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@phoneNumber", phoneNumber);

                    // Execute the query.
                    int rowsAffected = command.ExecuteNonQuery();

                    // Check if the user was successfully created.
                    if (rowsAffected > 0)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error executing SQL query: " + ex.Message);
                // User creation failed due to error.
                return false;
            }
            finally
            {
                // Close the connection in the finally block.
                connection.Close();
            }
        }
    }

    public static bool ChangeAccount(string NewInfo, int choice, string UserName, string connectionString)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                string query = "";
                string parameterName = "";

                switch (choice)
                {
                    case 1:
                        query = "UPDATE users SET email = @NewInfo WHERE user_name = @UserName";
                        parameterName = "@NewInfo";
                        break;
                    case 2:
                        query = "UPDATE users SET phone_number = @NewInfo WHERE user_name = @UserName";
                        parameterName = "@NewInfo";
                        break;
                    case 3:
                        query = "UPDATE users SET name = @NewInfo WHERE user_name = @UserName";
                        parameterName = "@NewInfo";
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        return false;
                }

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue(parameterName, NewInfo);
                    command.Parameters.AddWithValue("@UserName", UserName);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error executing SQL query: " + ex.Message);
                return false;
            }
            finally
            {
                connection.Close();
            }
        }
    }


}
