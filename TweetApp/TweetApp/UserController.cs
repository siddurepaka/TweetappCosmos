using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetApp.Service.TweetAppEntity;
using TweetApp.Service;

namespace TweetApp
{
    [ApiController]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly ITweetCosmoDBService _cosmosDbService;
       
        /// <summary>
        /// create the object of usercontroller.
        /// </summary>
        /// <param name="service">service dependenc injection.</param>
        public UserController(ITweetCosmoDBService cosmoDbService)
        {
            _cosmosDbService = cosmoDbService ?? throw new ArgumentNullException(nameof(cosmoDbService));
        }

        /// <summary>
        /// user login
        /// </summary>
        /// <param name="userId">based on userId.</param>
        /// <param name="password">based on password</param>
        /// <returns>returns the status message.</returns>

        [Route("UserLogin/{userId}/{password}")]
        [HttpGet]
        public Service.TweetAppEntity.User UserLogin(string userId, string password)
        {
            try
            {
               if(userId!=null && password != null)
                {
                    var result = this._cosmosDbService.UserLogin(userId, password);
                    if(result != null)
                    {
                        return result.Result;
                    }

                }
            }

            catch (TweetException ex)
            {
                throw new TweetException(BadRequest("error in userLogin") + ex.Message);
            }
            return null;


        }

        /// <summary>
        /// new user registration.
        /// </summary>
        /// <param name="userRegistration">userRegiastration.</param>
        /// <returns>returns the status message of user register.</returns>
        [Route("UserRegister")]
        [HttpPost]
        public IActionResult UserRegister([FromBody] Service.TweetAppEntity.User userRegistration)
        {
            try
            {
                if (userRegistration != null)
                {
                    userRegistration.Id = Guid.NewGuid().ToString();
                     _cosmosDbService.UserRegister(userRegistration);
                    return Ok(" inserted ");
                }
            }
            catch (TweetException ex)
            {
                throw new TweetException("error in user register" + ex.Message);
            }
            return Ok(new { status = Messages.EnterUserDetails });


        }

        [Route("UserExists/{emailId}")]
        [HttpGet]
        public string UserExists(String emailId)
        {
            try
            {               
                 if(emailId != null)
                {
                    var result = this._cosmosDbService.UserExists(emailId);
                    if(result.Result != null)
                    {
                        return Messages.UserExists;
                    }
                }

                return Messages.UserDoesNotExists;
            }
            catch (TweetException ex)
            {
                throw new TweetException("error in user register" + ex.Message);
            }


        }

        /// <summary>
        /// Updates the user password.
        /// </summary>
        /// <param name="userId">based on userId.</param>
        /// <param name="newPassword">updates the new password.</param>
        /// <returns>returns the status message whether the password is updated or not.</returns>
        [Route("resetpassword/{userId}/{oldpassword}/{newPassword}")]
        [HttpPut]
        public IActionResult PasswordUpdate(string userId, string oldpassword, string newPassword)
        {
            try
            {
                if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(newPassword))
                {

                    var result = this._cosmosDbService.PasswordUpdate(userId, oldpassword, newPassword);
                    return Ok(new
                    {
                        status = Messages.PasswordUpdated
                    });;

                }
            }
            catch (TweetException ex)
            {
                throw new TweetException("error in password update" + ex.Message);
            }

            return Ok(new { status = Messages.PasswordNotUpdated });
        }

        /// <summary>
        /// Password update emailid
        /// </summary>
        /// <param name="emailid">emailid.</param>
        /// <returns>return the status message</returns>
        [Route("ForgotPasswordEmailId/{emailId}")]
        [HttpGet]
        public IActionResult ForgotPasswordEmailId(string emailId)
        {
            try
            {
                if (!string.IsNullOrEmpty(emailId))
                {
                    var result = this._cosmosDbService.UserExists(emailId);
                    if (result.Result != null)
                    {
                        return Ok(new { status = Messages.UserExists });
                    }
                    

                }
            }
            catch (TweetException ex)
            {
                throw new TweetException("error in forgot email id " + ex.Message);
            }

            return Ok(new { status = Messages.NoChangepassword });
        }

        /// <summary>
        /// Pass
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        [Route("ForgotPassword/{userId}/{newPassword}")]
        [HttpPut]
        public IActionResult ForgotPassword(string userId, string newPassword)
        {
            try
            {
                if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(newPassword))
                {
                    var result = this._cosmosDbService.ForgotPassword(userId, newPassword);
                    

                    return Ok(new { status = Messages.PasswordUpdated });

                }
            }
            catch (TweetException ex)
            {
                throw new TweetException("error in password update" + ex.Message);
            }

            return Ok(new { status = Messages.PasswordNotUpdated });
        }
    }
}
