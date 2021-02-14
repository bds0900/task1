using DataSource.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSource
{
    
    public class TweetContext
    {
        private readonly IMongoDatabase _database;
        public TweetContext(IMongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            if(client!=null)
            {
                _database=client.GetDatabase(settings.DatabaseName);
            }

        }
        public IMongoCollection<Tweet> Tweets 
        {
            get
            {
                return _database.GetCollection<Tweet>("tweet");
            }
        }

        /*public IMongoCollection<SomeModle> SomeModel
        {
            get
            {
                return _database.GetCollection<SomeModle>("SomeModel");
            }
        }*/



    }
}
