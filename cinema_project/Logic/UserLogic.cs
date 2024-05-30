using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class UserLogic
{

    public static void Start()
    {
        Console.WriteLine("Create an Account");

        string newUsername;
        do
        {
            Console.WriteLine("Enter username:");
            newUsername = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newUsername))
            {
                Console.WriteLine("Username cannot be empty. Please enter a valid username.");
            }
            else if (UserAccess.UsernameExists(newUsername))
            {
                Console.WriteLine("Username already exists. Please enter a different username.");
                newUsername = string.Empty;
            }
        } while (string.IsNullOrWhiteSpace(newUsername));

        string newPassword;
        do
        {
            Console.WriteLine("Enter password:");
            newPassword = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newPassword))
            {
                Console.WriteLine("Password cannot be empty. Please enter a valid password.");
            }
        } while (string.IsNullOrWhiteSpace(newPassword));

        string name;
        do
        {
            Console.WriteLine("Enter name:");
            name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Name cannot be empty. Please enter a valid name.");
            }
        } while (string.IsNullOrWhiteSpace(name));

        string email;
        do
        {
            Console.WriteLine("Enter email:");
            email = Console.ReadLine();
            if (!IsValidEmail(email))
            {
                Console.WriteLine("Invalid email format. Please enter a valid email address.");
            }
            else if (UserAccess.EmailExists(email))
            {
                Console.WriteLine("Email already exists. Please enter a different email.");
                email = string.Empty;
            }
        } while (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email));

        string phoneNumber;
        do
        {
            Console.WriteLine("Enter phone number:");
            phoneNumber = Console.ReadLine();
            if (!IsValidPhoneNumber(phoneNumber))
            {
                Console.WriteLine("Invalid phone number format. Please enter a valid phone number.");
            }
            else if (UserAccess.PhoneNumberExists(phoneNumber))
            {
                Console.WriteLine("Phone number already exists. Please enter a different phone number.");
                phoneNumber = string.Empty;
            }
        } while (string.IsNullOrWhiteSpace(phoneNumber) || !IsValidPhoneNumber(phoneNumber));

        bool creationResult = UserAccess.CreateUser(newUsername, newPassword, name, email, phoneNumber);

        if (creationResult)
        {
            Console.WriteLine("Account created successfully!");
            UserLogin.Start();
        }
        else
        {
            Console.WriteLine("Failed to create account.");
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
