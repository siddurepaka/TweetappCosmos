﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TweetApp.Service
{
    public class TweetException : Exception
    {
        public TweetException()
        {

        }

        public TweetException(string message) : base(message)
        {

        }

        public TweetException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}

