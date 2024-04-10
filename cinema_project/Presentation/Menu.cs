﻿static class Menu
{
    static public void Start()
    {
        Console.WriteLine("1. Login");
        Console.WriteLine("2. Create an Account");
        Console.WriteLine("3. Exit");

        string input = Console.ReadLine();
        if (input == "1")
        {
            User loggedInUser = UserLogin.Start();
            if (loggedInUser != null)
            {
                if (loggedInUser.Role == "admin")
                {
                    AdminMenu.Start();
                }
                else
                {
                    UserMenu.Start(ref loggedInUser);
                }
            }
            else
            {
                Start(); // Restart the menu if login failed
            }
        }
        else if (input == "2")
        {
            UserCreation.Start();
        }
        else if (input == "3")
        {
            Environment.Exit(0);
        }
        else
        {
            Console.WriteLine("Invalid input");
            Start();
        }
    }
}
