using System.Data.SqlClient;
using System.Text.RegularExpressions;

public static class UserManager
{
    public static string connectionString = "Data Source=localhost;Initial Catalog=cinema_db;User ID=sa;Password=123456;";

    public static User Login(string username, string password)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                string query = "SELECT role FROM users WHERE user_name = @username AND password = @password";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
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

    public static bool CreateUser(string username, string password, string name, string email, string phoneNumber)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                string query = "INSERT INTO users (user_name, password, name, email, phone_number, role) " +
                               "VALUES (@username, @password, @name, @email, @phoneNumber, 'user')";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@phoneNumber", phoneNumber);

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

    public static bool ChangeAccount(string NewInfo, int choice, string UserName)
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
                    case 4:
                        query = "UPDATE users SET password = @NewInfo WHERE user_name = @UserName";
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

    public static bool DeleteAccount(string username)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                string query = "DELETE FROM users WHERE user_name = @username";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);

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

    public static List<User> GetAllUsers()
    {
        List<User> users = new List<User>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                string query = "SELECT user_name, role FROM users";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string username = reader.GetString(0);
                            string role = reader.GetString(1);

                            // Create user object and add it to the list
                            users.Add(new User(username, "", role));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error executing SQL query: " + ex.Message);
            }
        }

        return users;
    }

    // Method to validate email format.
    public static bool IsValidEmail(string email)
    {
        string pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
        return Regex.IsMatch(email, pattern);
    }

    // Method to validate phone number.
    public static bool IsValidPhoneNumber(string phoneNumber)
    {
        string pattern = @"^\d{10}$";
        return Regex.IsMatch(phoneNumber, pattern);
    }
}
