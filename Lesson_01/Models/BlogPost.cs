using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_01.Models
{
    public class BlogPost
    {
        [JsonProperty("userId")]
        public long UserId { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("title")]
        public string? Title { get; set; }

        [JsonProperty("body")]
        public string? Body { get; set; }

        public override string ToString()
        {
            return $"{UserId}\n{Id}\n{Title}\n{Body}\n";
        }
    }
}
