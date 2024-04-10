﻿static class UserLogin
{
    public static User Start()
    {
        Console.WriteLine("Welcome to the login page");
        Console.WriteLine("Please enter your username");
        string username = Console.ReadLine();
        Console.WriteLine("Please enter your password");
        string password = Console.ReadLine();


        User loggedInUser = UserManager.Login(username, password);
        if (loggedInUser != null)
        {
            Console.WriteLine($"Welcome {loggedInUser.Username}!");
            return loggedInUser; // Return the logged-in user
        }
        else
        {
            Console.WriteLine("Login failed. Invalid username or password.");
            return null; // Return null if login failed
        }
    }
}
