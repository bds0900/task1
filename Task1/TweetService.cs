using AutoMapper;
using DataSource;
using DataSource.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task1.Models;

namespace Task1
{
    public class TweetService : ITweetService
    {
        private readonly TweetContext _context;
        private readonly IMapper _mapper;
        public TweetService(TweetContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
            _mapper = mapper;

        }
        public async Task CreateAsync(Tweet tweet)
        {
            try
            {
                await _context.Tweets.InsertOneAsync(tweet);
            }
            catch(MongoCommandException ex)
            {
                throw new MyException(ex.Message, ex);
            }
            catch(Exception ex)
            {
                throw new MyException(ex.Message, ex);
            }
            
            //throw new NotImplementedException();
        }

        public Task<Tweet> GetTweetAsync(string id)
        {
            try
            {
                var filter = Builders<Tweet>.Filter.Eq(doc => doc.TweetId, id);
                return  _context.Tweets.Find(filter).SingleOrDefaultAsync();
            }
            catch(Exception ex)
            {
                throw new MyException(ex.Message, ex);
            }
            //throw new NotImplementedException();
        }

        public Task<Tweet> UpdateAsync(string id, Tweet tweet)
        {
            try
            {
                var filter = Builders<Tweet>.Filter.Eq(doc => doc.TweetId, id);
                return _context.Tweets.FindOneAndUpdateAsync(
                    Builders<Tweet>.Filter.Eq(doc => doc.TweetId, id),
                    Builders<Tweet>.Update.Set(rec => rec.Text, tweet.Text),
                    options: new FindOneAndUpdateOptions<Tweet>
                    {
                        // Do this to get the record AFTER the updates are applied
                        ReturnDocument = ReturnDocument.After
                    });
            }
            catch (Exception ex)
            {
                throw new MyException(ex.Message, ex);
            }

            //throw new NotImplementedException();
        }

        public Task<Tweet> DeleteAsync(string id)
        {
            try
            {
                var filter= Builders<Tweet>.Filter.Eq(doc => doc.TweetId, id);
                return _context.Tweets.FindOneAndDeleteAsync(filter);
            }
            catch (Exception ex)
            {
                throw new MyException(ex.Message, ex);
            }
            throw new NotImplementedException();
        }


    }
}
