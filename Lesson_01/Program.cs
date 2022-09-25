using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace Lesson_01
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string fileName = "result.txt";
            string blogAddress = "https://jsonplaceholder.typicode.com";

            Blog blog = new Blog(new Uri(blogAddress));

            Console.WriteLine($"Урок 1. Получить посты с 4 по 13 с сайта {blogAddress}");

            var response =  await blog.GetBlogPostAsync("posts/",4, 13);

            if (response == null || response.Count == 0)
            {
                Console.WriteLine("Результат не был получен.");
            }
            else
            {
                Console.WriteLine($"Записываем полученные посты в файл {fileName}.");

                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    foreach (var item in response) writer.WriteLine(item.ToString());
                }
            }

            Console.WriteLine("Нажмите любую клавишу для завершении работы ... ");
            Console.ReadKey();
        }
    }
}