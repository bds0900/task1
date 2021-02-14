using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSource.Models
{
    [BsonCollection("tweet")]
    public class Tweet : Document
    {
        public string UserId { get; set; }
        public string TweetId { get; set; }
        public string Text { get; set; }

        /*[StringLength(30)]
        public string Tag { get; set; }*/
    }
}
