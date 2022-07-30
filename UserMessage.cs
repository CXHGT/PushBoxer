using System;
using System.Collections.Generic;
using System.Text;

namespace PushBoxer
{
    public class UserMessage
    {
        public string time;
        public string username;
        public string message;

        public UserMessage(string time,string username,string message)
        {
            this.time = time;
            this.username = username;
            this.message = message;
        }
        public string getString()
        {
            return time + " " + username + ": " + message;
        }
    }
}
