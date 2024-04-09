static class UserCreation
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

        Console.WriteLine("Enter email:");
        string email = Console.ReadLine();

        Console.WriteLine("Enter phone number:");
        string phoneNumber = Console.ReadLine();

        bool creationResult = UserManager.CreateUser(newUsername, newPassword, name, email, phoneNumber);

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
}
