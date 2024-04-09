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