using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using TweetApp.Service.TweetAppEntity;

namespace TweetApp.Service
{
    public class TweetCosmoDbService : ITweetCosmoDBService
    {
        private Container _container;
        public TweetCosmoDbService(
            CosmosClient cosmosDbClient,
            string databaseName,
            string containerName)
        {
            _container = cosmosDbClient.GetContainer(databaseName, containerName);
        }
        public async Task<string> UserRegister(TweetAppEntity.User user)
        {
            var result = this.UserExists(user.EmailId);
            if (result.Result == null)
            {
                await _container.CreateItemAsync(user, new PartitionKey(user.Id));
                return Messages.UserRegister;
            }
            return Messages.UserExists;
        }

        public async Task<TweetAppEntity.User> UserLogin(string Username, string password)
        {
            FeedIterator<TweetAppEntity.User> setIterator = _container.GetItemLinqQueryable<TweetAppEntity.User>()
                      .Where(b => b.EmailId == Username && b.Password==password)
                      .ToFeedIterator<TweetAppEntity.User>();
            {
                while (setIterator.HasMoreResults)
                {
                    foreach (var item in await setIterator.ReadNextAsync())
                    {
                        {
                            return item;
                        }
                    }
                }
            }
            return null;
        }

        public async Task<TweetAppEntity.User> UserExists(string emailId)
        {
            FeedIterator<TweetAppEntity.User> setIterator = _container.GetItemLinqQueryable<TweetAppEntity.User>()
                      .Where(b => b.EmailId == emailId)
                      .ToFeedIterator<TweetAppEntity.User>();
            {
                while (setIterator.HasMoreResults)
                {
                    foreach (var item in await setIterator.ReadNextAsync())
                    {
                        {
                            return item;
                        }
                    }
                }
            }
            return null;
        }

        public async Task PasswordUpdate(string userId, string oldpassword, string newPassword)
        {
            Task<TweetAppEntity.User> result = this.UserLogin(userId, oldpassword);
            if(result.Result != null)
            {
                result.Result.Password = newPassword;
                await _container.UpsertItemAsync(result.Result, new PartitionKey(result.Result.Id));
            }
            
        }

        public async Task ForgotPassword(string userId, string newPassword)
        {
            Task<TweetAppEntity.User> result = this.UserExists(userId);
            if (result.Result != null)
            {
                result.Result.Password = newPassword;
                await _container.UpsertItemAsync(result.Result, new PartitionKey(result.Result.Id));
            }

        }


    }
}
