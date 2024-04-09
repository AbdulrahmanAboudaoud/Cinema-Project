static class UserLogin
{
    public static void Start()
    {
        Console.WriteLine("Welcome to the login page");
        Console.WriteLine("Please enter your username");
        string username = Console.ReadLine();
        Console.WriteLine("Please enter your password");
        string password = Console.ReadLine();

        // Check login
        User loggedInUser = UserManager.Login(username, password);
        if (loggedInUser != null)
        {
            Console.WriteLine($"Welcome {loggedInUser.Username}!");
            // Redirect to appropriate page based on role
            UserMenu.Start(ref loggedInUser);
        }
        else
        {
            Console.WriteLine("Login failed. Invalid username or password.");
        }
    }
}
