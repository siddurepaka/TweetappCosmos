﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TweetApp.Service
{
    public static class Messages
    {
        public const string
            UserRegister = "Registered succesfully",
            UserLogin = "Logged in succcessfully",
            LoginFailure = "Invalid UserName and Password",
            UserExists = "Username exists",
            UserDoesNotExists="Username Does not exists",
            EnterUserDetails = "Please enter the details",
            TweetAdded = "New tweet added successfully",
            TweetNotAdded = "tweet is not added",
            PasswordUpdated = "Password Updated",
            PasswordNotUpdated = "Password Not Updated, wrong userId and password",
            changePassword = "you can change the password",
            NoChangepassword = "you cannot change the password";



    }
}
