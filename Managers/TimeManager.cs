using System;
using System.Collections.Generic;
using System.Text;

namespace PushBoxer
{
    public static class TimeManager
    {
        public static long Time()
        {
            DateTime dateTime = new DateTime();
            TimeSpan timeSpan = DateTime.Now - dateTime;
            return (long)timeSpan.TotalMilliseconds;
        }
    }
}
