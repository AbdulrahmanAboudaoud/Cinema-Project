﻿// UserMenu.cs

static class UserMenu
{
    public static void Start(ref User loggedInUser)
    {
        bool exitRequested = false;

        while (!exitRequested)
        {
            Console.WriteLine("1. Change account information");
            Console.WriteLine("2. Delete account");
            Console.WriteLine("3. Logout\n");

            Console.Write("Select an option: ");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    UpdateAccount(loggedInUser);
                    break;
                case "2":
                    bool status = UserManager.DeleteAccount(loggedInUser.Username);
                    if(status)
                    {
                        Console.WriteLine("Account deleted successfully");
                        Menu.Start();
                    }
                    break;
                case "3":
                    Logout(ref loggedInUser);
                    exitRequested = true;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please select again.");
                    break;
            }
        }
    }

    private static void UpdateAccount(User loggedInUser)
    {
        Console.WriteLine("\nWhich info would you like to change?");
        Console.WriteLine("1. Email\n2. Phone\n3. Name\n4. Password");
        int choice = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Enter the new information:");
        string newInfo = Console.ReadLine();

        bool result = UserManager.ChangeAccount(newInfo, choice, loggedInUser.Username);
        if (result)
        {
            Console.WriteLine($"Your information has been updated successfully.\n");
        }
        else
        {
            Console.WriteLine($"Failed to update information.\n");
        }
    }

    private static void Logout(ref User loggedInUser)
    {
        Console.WriteLine("Logging out...");
        loggedInUser = null;
        Console.WriteLine("You have been logged out.");
    }
}