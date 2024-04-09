static class Menu
{
    static public void Start()
    {
        Console.WriteLine("1. Login");
        Console.WriteLine("2. Create an Account");
        Console.WriteLine("3. Exit");

        string input = Console.ReadLine();
        if (input == "1")
        {
            UserLogin.Start();
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
