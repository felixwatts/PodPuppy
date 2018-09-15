using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

namespace PodPuppy
{
    public enum SchduleModes
    {
        Download = 0,
        Pause = 1,
    }

    public class Scheduler
    {
        private Timer _timer;

        public Scheduler()
        {
            _timer = new Timer();
            _timer.Tick += new EventHandler(OnTimerTick);
        }
       
        public void Start()
        {
            OnTimerTick(null, null);
            _timer.Start();               
        }

        public void Stop()
        {
            _timer.Stop();
        }

        void OnTimerTick(object sender, EventArgs e)
        {
            _timer.Interval = ((3600 - (DateTime.Now.Minute * 60)) - DateTime.Now.Second) * 1000;

            int dayIndex = (((int)DateTime.Now.DayOfWeek) + 6) % 7;

            bool download = Statics.Config.Schedule[DateTime.Now.AddMinutes(1).Hour, dayIndex];

            if (download)
            {
                if (ResumeDownloads != null)
                    ResumeDownloads(this, null);
            }
            else
            {
                if (PauseDownloads != null)
                    PauseDownloads(this, null);
            }
        }

        public event EventHandler ResumeDownloads;
        public event EventHandler PauseDownloads; 
    }
}
