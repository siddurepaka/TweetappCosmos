using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TweetApp.Service.TweetAppEntity;

namespace TweetApp.Service
{
    public interface ITweetCosmoDBService
    {
        Task<string> UserRegister(TweetAppEntity.User user);
        Task<TweetAppEntity.User> UserLogin(string Username, string password);
        Task<TweetAppEntity.User> UserExists(string emailId);
        Task PasswordUpdate(string userId, string oldpassword, string newPassword);
        Task ForgotPassword(string userId, string newPassword);
        
    }
}
