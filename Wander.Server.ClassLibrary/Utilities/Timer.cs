﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Wander.Server.ClassLibrary.Utilities
{
    public class Timer
    {
        public delegate void TimerEvent();
        public static void Once(TimerEvent callback, int delaySecond)
        {
            new System.Threading.Timer((param) => callback(), null,
                              TimeSpan.FromSeconds(delaySecond), 
                              TimeSpan.FromMilliseconds(-1));
        }

        public static void Repeat(TimerEvent callback, int intervalSecond)
        {
            new System.Threading.Timer((param) => callback(), null,
                              TimeSpan.FromSeconds(0),
                              TimeSpan.FromSeconds(intervalSecond));
        }

        public static void RepeatAfter(TimerEvent callback,int delaySecond, int intervalSecond)
        {
            new System.Threading.Timer((param)=>callback(), null,
                              TimeSpan.FromSeconds(delaySecond),
                              TimeSpan.FromSeconds(intervalSecond));
        }

        public static void Random(TimerEvent callback, int minIntervalSecond, int maxIntervalSecond)
        {
            Random r = new System.Random();
            int interval = r.Next(minIntervalSecond * 1000, maxIntervalSecond * 1000);

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = interval;
            timer.Start();
            timer.Elapsed += (param, e) =>
            {
                timer.Interval = r.Next(minIntervalSecond * 1000, maxIntervalSecond * 1000);
                callback();
            };
        }

    }
}
