//using System;
//using System.Data.SqlClient;

//class Program
//{
//    static void Main(string[] args)
//    {
//        string connectionString = "Data Source=localhost;Initial Catalog=cinema_db;User ID=sa;Password=123456;";
//        bool exitRequested = false;
//        bool isLoggedIn = false;
//        User loggedInUser = null;

//        while (!exitRequested)
//        {
//            if (!isLoggedIn)
//            {
//                Console.WriteLine("Welcome to the Cinema Application!");
//                Console.WriteLine("1. Login");
//                Console.WriteLine("2. Create an Account");
//                Console.WriteLine("3. Exit\n");
//            }
//            else
//            {
//                Console.WriteLine("1. Change account information");
//                Console.WriteLine("2. Delete account");
//                Console.WriteLine("3. Logout");
//                Console.WriteLine("4. Exit\n");
//            }



//            Console.Write("Select an option: ");
//            string option = Console.ReadLine();

//            switch (option)
//            {
//                case "1":
//                    if (!isLoggedIn)
//                    {
//                        Console.WriteLine("Login");
//                        Console.WriteLine("Enter username:");
//                        string username = Console.ReadLine();

//                        Console.WriteLine("Enter password:");
//                        string password = Console.ReadLine();

//                        loggedInUser = User.Login(username, password, connectionString);
//                        isLoggedIn = loggedInUser != null;

//                        if (isLoggedIn)
//                        {
//                            if (loggedInUser.Role == "admin")
//                            {
//                                Console.WriteLine("Login successful as ADMIN!");
//                            }
//                            else if (loggedInUser.Role == "user")
//                            {
//                                Console.WriteLine("Login successful as Customer!");
//                            }
//                            else
//                            {
//                                Console.WriteLine("Login successful!");
//                            }
//                            isLoggedIn = true;
//                        }
//                        else
//                        {
//                            Console.WriteLine("Login failed. Invalid username or password.");
//                        }
//                    }
//                    else
//                    {
//                        bool ChangeAccountResult;
//                        string NewInfo;
//                        string UserName = loggedInUser.Username;
//                        Console.WriteLine("\nWhich info would you like to change?");
//                        Console.WriteLine("1. Email\n2. Phone\n3. Name\n4. Password");
//                        int choice = Convert.ToInt32(Console.ReadLine());

//                        switch (choice)
//                        {
//                            case 1:
//                                Console.WriteLine("New Email:");
//                                NewInfo = Console.ReadLine();
//                                ChangeAccountResult = User.ChangeAccount(NewInfo, choice, UserName, connectionString);
//                                if (ChangeAccountResult)
//                                {
//                                    Console.WriteLine($"Your new email, {NewInfo}, is confirmed.\n");
//                                }
//                                break;
//                            case 2:
//                                Console.WriteLine("New Phone number:");
//                                NewInfo = Console.ReadLine();
//                                ChangeAccountResult = User.ChangeAccount(NewInfo, choice, UserName, connectionString);
//                                if (ChangeAccountResult)
//                                {
//                                    Console.WriteLine($"Your new phone number, {NewInfo}, is confirmed.\n");
//                                }
//                                break;
//                            case 3:
//                                Console.WriteLine("New Name:");
//                                NewInfo = Console.ReadLine();
//                                ChangeAccountResult = User.ChangeAccount(NewInfo, choice, UserName, connectionString);
//                                if (ChangeAccountResult)
//                                {
//                                    Console.WriteLine($"Your new name, {NewInfo}, is confirmed.\n");
//                                }
//                                break;
//                            case 4:
//                                Console.WriteLine("New Password:");
//                                NewInfo = Console.ReadLine();
//                                ChangeAccountResult = User.ChangeAccount(NewInfo, choice, UserName, connectionString);
//                                if (ChangeAccountResult)
//                                {
//                                    Console.WriteLine($"Your password has ben changed");
//                                }
//                                break;
//                            default:
//                                Console.WriteLine("Invalid option. Please select again.\n");
//                                break;
//                        }
//                        break;
//                    }
//                    break;

//                case "2":
//                    if (!isLoggedIn)
//                    {
//                        Console.WriteLine("Create an Account");
//                        Console.WriteLine("Enter username:");
//                        string newUsername = Console.ReadLine();

//                        Console.WriteLine("Enter password:");
//                        string newPassword = Console.ReadLine();

//                        Console.WriteLine("Enter name:");
//                        string name = Console.ReadLine();

//                        Console.WriteLine("Enter email:");
//                        string email = Console.ReadLine();

//                        Console.WriteLine("Enter phone number:");
//                        string phoneNumber = Console.ReadLine();

//                        bool creationResult = User.CreateUser(newUsername, newPassword, name, email, phoneNumber, connectionString);

//                        if (creationResult)
//                        {
//                            Console.WriteLine("Account created successfully!");
//                        }
//                        else
//                        {
//                            Console.WriteLine("Failed to create account.");
//                        }
//                    }
//                    else
//                    {
//                        bool deletionResult = User.DeleteAccount(loggedInUser.Username, connectionString);
//                        if (deletionResult)
//                        {
//                            Console.WriteLine("Account deleted successfully!");
//                            isLoggedIn = false;
//                        }
//                        else
//                        {
//                            Console.WriteLine("Failed to delete account.");
//                        }
//                    }
//                    break;

//                case "3":
//                    if (isLoggedIn)
//                    {
//                        isLoggedIn = false;
//                        Console.WriteLine("You have been logged out.");
//                    }
//                    else
//                    {
//                        exitRequested = true;
//                    }
//                    break;

//                case "4":
//                    exitRequested = true;
//                    break;

//                default:
//                    Console.WriteLine("Invalid option. Please select again.");
//                    break;
//            }
//        }
//    }
//}

using System;

class Program
{
    static void Main(string[] args)
    {
        bool exitRequested = false;

        while (!exitRequested)
        {
            Console.WriteLine("Welcome to the Cinema Application!");
            Menu.Start();
        }
    }
}
