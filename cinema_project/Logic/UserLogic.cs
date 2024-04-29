using System.Data.SqlClient;
using System.Text.RegularExpressions;

public static class UserLogic
{
    public static void Start()
    {
        Console.WriteLine("Create an Account");
        Console.WriteLine("Enter username:");
        string newUsername = Console.ReadLine();

        Console.WriteLine("Enter password:");
        string newPassword = Console.ReadLine();

        Console.WriteLine("Enter name:");
        string name = Console.ReadLine();

        string email;
        do
        {
            Console.WriteLine("Enter email:");
            email = Console.ReadLine();
            if (!IsValidEmail(email))
            {
                Console.WriteLine("Invalid email format. Please enter a valid email address.");
            }
        } while (!IsValidEmail(email));

        string phoneNumber;
        do
        {
            Console.WriteLine("Enter phone number:");
            phoneNumber = Console.ReadLine();
            if (!IsValidPhoneNumber(phoneNumber))
            {
                Console.WriteLine("Invalid phone number format. Please enter a valid phone number.");
            }
        } while (!IsValidPhoneNumber(phoneNumber));

        bool creationResult = CreateUser(newUsername, newPassword, name, email, phoneNumber);

        if (creationResult)
        {
            Console.WriteLine("Account created successfully!");
            // Navigate to log in menu.
            UserLogin.Start();
        }
        else
        {
            Console.WriteLine("Failed to create account.");
            // Start again.
            Start();
        }
    }

    public static User Login(string username, string password)
    {
        using (SqlConnection connection = UserAccess.OpenConnection())
        {
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
    }

    public static bool CreateUser(string username, string password, string name, string email, string phoneNumber)
    {
        using (SqlConnection connection = UserAccess.OpenConnection())
        {
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
    }

    public static bool ChangeAccount(string NewInfo, int choice, string UserName)
    {
        using (SqlConnection connection = UserAccess.OpenConnection())
        {
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
    }

    public static bool DeleteAccount(string username)
    {
        using (SqlConnection connection = UserAccess.OpenConnection())
        {
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
    }

    public static List<User> GetAllUsers()
    {
        List<User> users = new List<User>();

        using (SqlConnection connection = UserAccess.OpenConnection())
        {
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
