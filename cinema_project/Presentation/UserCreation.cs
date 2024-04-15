public static class UserCreation
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

        string email;
        do
        {
            Console.WriteLine("Enter email:");
            email = Console.ReadLine();
            if (!UserManager.IsValidEmail(email))
            {
                Console.WriteLine("Invalid email format. Please enter a valid email address.");
            }
        } while (!UserManager.IsValidEmail(email));

        string phoneNumber;
        do
        {
            Console.WriteLine("Enter phone number:");
            phoneNumber = Console.ReadLine();
            if (!UserManager.IsValidPhoneNumber(phoneNumber))
            {
                Console.WriteLine("Invalid phone number format. Please enter a valid phone number.");
            }
        } while (!UserManager.IsValidPhoneNumber(phoneNumber));

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
