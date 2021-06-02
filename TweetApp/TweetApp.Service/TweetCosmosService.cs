using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetApp.Service.TweetAppEntity;

namespace TweetApp.Service
{
    public class TweetCosmosService : ITweetCosmosService
    {
        private Container _container;
        public TweetCosmosService(
            CosmosClient cosmosDbClient,
            string databaseName,
            string containerName)
        {
            _container = cosmosDbClient.GetContainer(databaseName, containerName);
        }


        public async Task<string> AddNewPost(Tweets tweet)
        {

            if (tweet != null)
            {
                await _container.CreateItemAsync(tweet, new PartitionKey(tweet.TweetId));
                return Messages.UserRegister;
            }
            return Messages.UserExists;
        }

        public async Task<IEnumerable<Tweets>> GetUserTweets(string emailId)
        {
            var tweetList = new List<Tweets>();
            FeedIterator<TweetAppEntity.Tweets> setIterator = _container.GetItemLinqQueryable<TweetAppEntity.Tweets>()
                       .Where(b => b.UserId == emailId)
                       .ToFeedIterator<TweetAppEntity.Tweets>();
            {
                while (setIterator.HasMoreResults)
                {
                    var response = await setIterator.ReadNextAsync();
                    tweetList.AddRange(response.ToList());
                }
                return tweetList;
            }
           
        }

        public async Task<IEnumerable<Tweets>> GetOtherAllUserTweets(string emailId)
        {
            var tweetList = new List<Tweets>();
            FeedIterator<TweetAppEntity.Tweets> setIterator = _container.GetItemLinqQueryable<TweetAppEntity.Tweets>()
                       .Where(b => b.UserId != emailId)
                       .ToFeedIterator<TweetAppEntity.Tweets>();
            {
                while (setIterator.HasMoreResults)
                {
                    var response = await setIterator.ReadNextAsync();
                    tweetList.AddRange(response.ToList());
                }
                return tweetList;
            }
        }

    }
}
