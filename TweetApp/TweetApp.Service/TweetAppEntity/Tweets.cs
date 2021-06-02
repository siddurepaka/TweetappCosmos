using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TweetApp.Service.TweetAppEntity
{
    public class Tweets
    {
            [JsonProperty(PropertyName = "id")]
            public string TweetId { get; set; }
            [JsonProperty(PropertyName = "userId")]
            public string UserId { get; set; }
            [JsonProperty(PropertyName = "userName")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "Tweet is required")]
            [StringLength(144, ErrorMessage = "Tweet cannot be longer than 20 characters.")]
            [JsonProperty(PropertyName = "userTweets")]
            public string UserTweets { get; set; }
            [JsonProperty(PropertyName = "createdDate")]
            public DateTime? CreatedDate { get; set; }
            

        }
    }

