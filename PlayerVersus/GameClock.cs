using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace PlayerVersus
{
    public class GameClock
    {
        private System.Timers.Timer timer;


        public GameClock(int interval)
        {
            timer = new System.Timers.Timer(interval);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Notification notification = new Notification("GameClockTick", this);
            NotificationCenter.Instance.PostNotification(notification);
        }
    }
}
