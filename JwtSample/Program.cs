namespace JwtSample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter user name:");
            Console.Write("> ");
            string userName = Console.ReadLine();
            Console.WriteLine("Enter password:");
            Console.Write("> ");
            string userPassword = Console.ReadLine();

            UserService userService = new UserService();
            Console.WriteLine(userService.Authenticate(userName, userPassword));

            Console.ReadKey();
        }
    }
}