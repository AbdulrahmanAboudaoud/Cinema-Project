class Program
{
    static void Main(string[] args)
    {
        // Connection string for SQL Server
        string connectionString = "Data Source=ABDULRAHMAN;Initial Catalog=cinema_project;User ID=sa;Password=q1w2e3r4t5;";

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
    }
}
