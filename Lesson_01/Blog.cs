using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lesson_01.Models;

namespace Lesson_01
{
    public class Blog
    {
        public Uri BlogAdress { get; set; }

        public Blog(Uri blogAdress)
        {
            BlogAdress = blogAdress;
        }

        public async Task<BlogPost> GetBlogPostAsync(string apiRoute, UInt64 postId)
        {
            if (BlogAdress == null || BlogAdress.IsAbsoluteUri == false) return null;

            HttpClient client = new HttpClient();
            client.BaseAddress = BlogAdress;

            try
            {
                Console.WriteLine($"Trying get post {postId}");
                CancellationTokenSource cancellationToken = new CancellationTokenSource();
                cancellationToken.CancelAfter(5000);
                var response = await client.GetAsync($"{apiRoute}{postId}", cancellationToken.Token);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Got post {postId}");
                    return JsonConvert.DeserializeObject<BlogPost>(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Console.WriteLine($"Post {postId} was not got. Response result: {response.ReasonPhrase}");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine($"Post {postId} was not got. Message: {e.Message}");
            }

            return null;
        }

        public async Task<ICollection<BlogPost>> GetBlogPostAsync(string apiRoute, UInt64 fromPost, UInt64 toPost)
        {
            if (BlogAdress == null || BlogAdress.IsAbsoluteUri == false) return null;

            ICollection<Task<BlogPost>> jobs = new List<Task<BlogPost>>();
            ICollection<BlogPost> blogPosts = new List<BlogPost>();

            for (UInt64 i = fromPost; i <= toPost; i++)
            {
                try
                {
                    jobs.Add(GetBlogPostAsync(apiRoute, i));
                }
                catch (Exception)
                {

                }
            }

            await Task.WhenAll(jobs);
            return jobs.Select(x => x.Result).Where(x => x != null).ToList();

        }
    }
}
