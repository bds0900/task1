using DataSource.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task1.Models;

namespace Task1
{
    public interface ITweetService
    {
        //Task<IEnumerable<Tweet>> GetTweetsAsync(GetTodoQuery filter = null, PaginationFilter paginationFilter = null); 
        Task<Tweet> GetTweetAsync(string id);
        Task CreateAsync(Tweet tweet);
        Task<Tweet> UpdateAsync(string id, Tweet tweet);
        Task<Tweet> DeleteAsync(string id);
    }
}
