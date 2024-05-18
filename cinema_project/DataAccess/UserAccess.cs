﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

public static class UserAccess
{
    private static string connectionString = "Data Source=localhost;Initial Catalog=cinema_db;User ID=sa;Password=123456;";

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

    public static bool CreateUser(string username, string password, string name, string email, string phoneNumber)
    {
        using (SqlConnection connection = OpenConnection())
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

    public static bool ChangeAccount(string newInfo, int choice, string userName)
    {
        using (SqlConnection connection = OpenConnection())
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
                command.Parameters.AddWithValue(parameterName, newInfo);
                command.Parameters.AddWithValue("@UserName", userName);

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
        using (SqlConnection connection = OpenConnection())
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

        using (SqlConnection connection = OpenConnection())
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

    public static User Login(string username, string password)
    {
        using (SqlConnection connection = OpenConnection())
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

    public static List<UserData> GetAllUserData()
    {
        List<UserData> users = new List<UserData>();

        using (SqlConnection connection = OpenConnection())
        {
            string query = "SELECT user_name, password, name, email, phone_number FROM users";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UserData user = new UserData
                        {
                            UserName = reader["user_name"].ToString(),
                            Password = reader["password"].ToString(),
                            Name = reader["name"].ToString(),
                            Email = reader["email"].ToString(),
                            PhoneNumber = reader["phone_number"].ToString()
                        };
                        users.Add(user);
                    }
                }
            }
        }

        return users;
    }



    public static bool RemoveUser(string username)
    {
        using (SqlConnection connection = OpenConnection())
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

    public static void UpdateUser(string currentUsername, string newUsername, string newPassword, string newName, string newEmail, string newPhoneNumber)
    {
        using (SqlConnection connection = OpenConnection())
        {
            string query = @"
            UPDATE users
            SET user_name = @newUsername,
                password = @newPassword,
                name = @newName,
                email = @newEmail,
                phone_number = @newPhoneNumber
            WHERE user_name = @currentUsername";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@currentUsername", currentUsername);
                command.Parameters.AddWithValue("@newUsername", newUsername);
                command.Parameters.AddWithValue("@newPassword", newPassword);
                command.Parameters.AddWithValue("@newName", newName);
                command.Parameters.AddWithValue("@newEmail", newEmail);
                command.Parameters.AddWithValue("@newPhoneNumber", newPhoneNumber);

                command.ExecuteNonQuery();
            }
        }
    }


}
