using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TweetApp.Service.TweetAppEntity;

namespace TweetApp.Service
{
    public interface ITweetCosmosService
    {
        Task<string> AddNewPost(Tweets tweet);
        Task<IEnumerable<Tweets>> GetUserTweets(string emailId);
        Task<IEnumerable<Tweets>> GetOtherAllUserTweets(string emailId);
    }
}
