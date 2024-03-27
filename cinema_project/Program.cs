using System;
using System.Data.SqlClient;

class Program
{
    static void Main(string[] args)
    {
        string connectionString = "Data Source=localhost;Initial Catalog=cinema_db;User ID=sa;Password=123456;";
        bool exitRequested = false;
        bool isLoggedIn = false; // New variable to check if user is logged in or not.
        User loggedInUser = null;

        while (!exitRequested)
        {
            // Main menu interface.

            if (!isLoggedIn) // Check if user is logged in.
            {
                Console.WriteLine("Welcome to the Cinema Application!");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Create an Account");
            }
            else
            {
                Console.WriteLine("1. Change account information");
                Console.WriteLine("2. Logout");
            }

            Console.WriteLine("3. Exit\n");

            Console.Write("Select an option: ");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    if (!isLoggedIn) //Only access to it if user is yet to log in.
                    {
                        Console.WriteLine("Login");
                        Console.WriteLine("Enter username:");
                        string username = Console.ReadLine();

                        Console.WriteLine("Enter password:");
                        string password = Console.ReadLine();

                        // Attempt to log in.
                        loggedInUser = User.Login(username, password, connectionString);
                        isLoggedIn = loggedInUser != null;

                        if (isLoggedIn)
                        {
                            // Check user role.
                            if (loggedInUser.Role == "admin")
                            {
                                Console.WriteLine("Login successful as ADMIN!");
                            }
                            else if (loggedInUser.Role == "user")
                            {
                                Console.WriteLine("Login successful as Customer!");
                            }
                            else
                            {
                                Console.WriteLine("Login successful!");
                            }
                            isLoggedIn = true; // User is now logged in.
                        }
                        else
                        {
                            Console.WriteLine("Login failed. Invalid username or password.");
                        }
                    }
                    else
                    {
                        //Here we have the code of changing account information
                        //Because the user is already logged in.
                        bool ChangeAccountResult;
                        string NewInfo;
                        string UserName = loggedInUser.Username;
                        Console.WriteLine("\nWhich info would you like to change?");
                        Console.WriteLine("1. Email\n2. Phone\n3. Name");
                        int choice = Convert.ToInt32(Console.ReadLine());

                        switch (choice)
                        {
                            case 1:
                                Console.WriteLine("New Email:");
                                NewInfo = Console.ReadLine();
                                ChangeAccountResult = User.ChangeAccount(NewInfo, choice, UserName, connectionString);
                                if (ChangeAccountResult)
                                {
                                    Console.WriteLine($"Your new email, {NewInfo}, is confirmed.\n");
                                }
                                break;
                            case 2:
                                Console.WriteLine("New Phone number:");
                                NewInfo = Console.ReadLine();
                                ChangeAccountResult = User.ChangeAccount(NewInfo, choice, UserName, connectionString);
                                if (ChangeAccountResult)
                                {
                                    Console.WriteLine($"Your new phone number, {NewInfo}, is confirmed.\n");
                                }
                                break;
                            case 3:
                                Console.WriteLine("New Name:");
                                NewInfo = Console.ReadLine();
                                ChangeAccountResult = User.ChangeAccount(NewInfo, choice, UserName, connectionString);
                                if (ChangeAccountResult)
                                {
                                    Console.WriteLine($"Your new name, {NewInfo}, is confirmed.\n");
                                }
                                break;
                            default:
                                Console.WriteLine("Invalid option. Please select again.\n");
                                break;
                        }
                        break;
                    }
                    break;

                case "2":
                    if (!isLoggedIn) //Only access to it if user is yet to log in.
                    {
                        Console.WriteLine("Create an Account");
                        Console.WriteLine("Enter username:");
                        string newUsername = Console.ReadLine();

                        Console.WriteLine("Enter password:");
                        string newPassword = Console.ReadLine();

                        Console.WriteLine("Enter name:");
                        string name = Console.ReadLine();

                        Console.WriteLine("Enter email:");
                        string email = Console.ReadLine();

                        Console.WriteLine("Enter phone number:");
                        string phoneNumber = Console.ReadLine();

                        // Attempt to create a new account.
                        bool creationResult = User.CreateUser(newUsername, newPassword, name, email, phoneNumber, connectionString);

                        if (creationResult)
                        {
                            Console.WriteLine("Account created successfully!");
                        }
                        else
                        {
                            Console.WriteLine("Failed to create account.");
                        }
                    }
                    else
                    {
                        //Logout code here
                        //It is not here yet

                        isLoggedIn = false;
                        Console.WriteLine("You have been logged out.");
                    }
                    break;

                case "3":
                    // Exit option
                    exitRequested = true;
                    break;

                default:
                    Console.WriteLine("Invalid option. Please select again.");
                    break;
            }
        }
    }
}

