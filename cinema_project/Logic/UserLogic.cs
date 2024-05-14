using System;
using System.Collections.Generic;
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

        bool creationResult = UserAccess.CreateUser(newUsername, newPassword, name, email, phoneNumber);

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
        return UserAccess.Login(username, password);
    }

    public static bool ChangeAccount(string newInfo, int choice, string userName)
    {
        return UserAccess.ChangeAccount(newInfo, choice, userName);
    }

    public static bool DeleteAccount(string username)
    {
        return UserAccess.DeleteAccount(username);
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
