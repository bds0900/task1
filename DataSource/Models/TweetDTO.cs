using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSource.Models
{
    public class TweetDTO
    {
        public string UserId { get; set; }
        public string Id { get; set; }//tweet id
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
