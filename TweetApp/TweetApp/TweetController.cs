using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetApp.Service.TweetAppEntity;
using TweetApp.Service;

namespace TweetApp
{
    //[EnableCors("MODPolicy")]
    [ApiController]
    [Produces("application/json")]
    public class TweetController : ControllerBase
    {
        private readonly ITweetCosmosService _cosmosDbService;
        /// <summary>
        /// Create the instance of the tweetController.
        /// </summary>
        /// <param name="service">service.</param>
        public TweetController(ITweetCosmosService cosmoDbService)
        {
            _cosmosDbService = cosmoDbService ?? throw new ArgumentNullException(nameof(cosmoDbService));
        }

        /// <summary>
        /// Adds the new tweet
        /// </summary>
        /// <param name="tweet">Tweet.</param>
        /// <returns>returns the status message.</returns>

        [Route("AddNewTweet")]
        [HttpPost]
        public IActionResult AddNewTweet([FromBody]Tweets tweet)
        {
            try
            {
                if (tweet != null)
                {
                    tweet.TweetId = Guid.NewGuid().ToString();
                    this._cosmosDbService.AddNewPost(tweet);
                    return Ok(new { status = Messages.TweetAdded });
                }

                return Ok(new { status = Messages.TweetNotAdded });
            }
            catch (TweetException ex)
            {
                throw new TweetException("error in add new tweet" + ex.Message);
            }
        }

        /// <summary>
        /// Views all user tweets.
        /// </summary>
        /// <param name="userId">userId.</param>
        /// <returns>returns all the user tweets.</returns>
        [Route("ViewUserAllTweets/{userId}")]
        [HttpGet]
        public async Task<IActionResult> ViewUserAllTweets(string userId)
        {
            try
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    var tweets = await this._cosmosDbService.GetUserTweets(userId);
                    return Ok(tweets);
                }
                return null;
            }
            catch (TweetException ex)
            {
                throw new TweetException("error in add new tweet" + ex.Message);
            }


        }

        /// <summary>
        /// Views all user tweets.
        /// </summary>
        /// <param name="userId">userId.</param>
        /// <returns>returns all the user tweets.</returns>
        [Route("ViewOtherUsersAllTweets/{userId}")]
        [HttpGet]
        public async Task<IActionResult> ViewOtherUsersAllTweets(string userId)
        {
            try
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    var tweets = await this._cosmosDbService.GetOtherAllUserTweets(userId);
                    return Ok(tweets);
                }
                return null;
            }
            catch (TweetException ex)
            {
                throw new TweetException("error in add new tweet" + ex.Message);
            }


        }
    }
}
