class Program
{
    static void Main(string[] args)
    {
        // Connection string for SQL Server
        string connectionString = "Data Source=localhost;Initial Catalog=cinema_db;User ID=sa;Password=123456;";

        // Main menu interface
        Console.WriteLine("Welcome to the Cinema Application!");
        Console.WriteLine("1. Login");
        Console.WriteLine("2. Create an Account");
        Console.WriteLine("3. Exit");

        Console.Write("Select an option: ");
        string option = Console.ReadLine();

        switch (option)
        {
            case "1":
                // Login option
                Console.WriteLine("Login");
                Console.WriteLine("Enter username:");
                string username = Console.ReadLine();

                Console.WriteLine("Enter password:");
                string password = Console.ReadLine();

                // Attempt to log in.
                (bool loginResult, string role) = User.Login(username, password, connectionString);

                if (loginResult)
                {
                    // Check user role.
                    if (role == "admin")
                    {
                        Console.WriteLine("Login successful as ADMIN!");
                    }
                    else if (role == "user")
                    {
                        Console.WriteLine("Login successful as Customer!");
                    }
                    else
                    {
                        Console.WriteLine("Login successful!");
                    }
                }
                else
                {
                    Console.WriteLine("Login failed. Invalid username or password.");
                }
                break;

            case "2":
                // Account creation option
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
                break;

            case "3":
                // Exit option
                Environment.Exit(0);
                break;

            default:
                Console.WriteLine("Invalid option. Please select again.");
                break;
        }
    }

}
    /*static void Main(string[] args)
    {
        // Connection string for SQL Server
        string connectionString = "Data Source=localhost;Initial Catalog=cinema_db;User ID=sa;Password=123456;";

        // Asking user for username and password.
        Console.WriteLine("Enter username:");
        string username = Console.ReadLine();

        Console.WriteLine("Enter password:");
        string password = Console.ReadLine();

        // Attempt to log in.
        (bool loginResult, string role) = User.Login(username, password, connectionString);

        if (loginResult)
        {
            // Check user role.
            if (role == "admin")
            {
                Console.WriteLine("Login successful as ADMIN!");
            }
            else if (role == "user")
            {
                Console.WriteLine("Login successful as Customer!");
            }
            else
            {
                Console.WriteLine("Login successful!");
            }
        }
        else
        {
            Console.WriteLine("Login failed. Invalid username or password.");
        }
    }*/

