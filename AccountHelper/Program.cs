using EmployeeService.Utils;

namespace AccountHelper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PasswordUtils.CreatePasswordHash("pass1"));

            Console.ReadKey();
        }
    }
}