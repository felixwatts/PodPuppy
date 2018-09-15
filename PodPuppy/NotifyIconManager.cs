using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace PodPuppy
{
    public class NotifyIconManager
    {
        private enum NotifyIconMode
        {
            Paused,
            UpToDate,
            Downloading,
            Refreshing
        }

        private Timer _animationTimer;
        private int _animationFrameNumber;
        private Icon[] _downloadAnim;

        public NotifyIconManager()
        {
            _animationTimer = new Timer();
            _animationTimer.Interval = 200;
            _animationTimer.Tick += new EventHandler(_animationTimer_Tick);
            
            _downloadAnim = new Icon[8];
            _downloadAnim[0] = PodPuppy.Properties.Resources.DownloadAnim1;
            _downloadAnim[1] = PodPuppy.Properties.Resources.DownloadAnim2;
            _downloadAnim[2] = PodPuppy.Properties.Resources.DownloadAnim3;
            _downloadAnim[3] = PodPuppy.Properties.Resources.DownloadAnim4;
            _downloadAnim[4] = PodPuppy.Properties.Resources.DownloadAnim5;
            _downloadAnim[5] = PodPuppy.Properties.Resources.DownloadAnim6;
            _downloadAnim[6] = PodPuppy.Properties.Resources.DownloadAnim7;
            _downloadAnim[7] = PodPuppy.Properties.Resources.DownloadAnim8;
        }

        void _animationTimer_Tick(object sender, EventArgs e)
        {
            _animationFrameNumber = (_animationFrameNumber + 1) % 8;
            Statics.NotifyIcon.Icon = _downloadAnim[_animationFrameNumber];
        }

        public delegate void SimpleDelegate();

        public void RefreshMode()
        {
            if (Statics.FeedListView.InvokeRequired)
            {
                Statics.FeedListView.Invoke(new SimpleDelegate(RefreshMode));
            }
            else
            {
                if (Statics.Paused)
                {
                    SetMode(NotifyIconMode.Paused);
                    return;
                }
                else
                {
                    bool downloading = false;
                    foreach (Feed feed in Statics.FeedListView.Items)
                    {
                        if (feed.Status == FeedStatus.Refreshing)
                        {
                            SetMode(NotifyIconMode.Refreshing);
                            return;
                        }
                        else if (feed.Status == FeedStatus.Downloading)
                            downloading = true;
                    }

                    if (downloading)
                        SetMode(NotifyIconMode.Downloading);
                    else SetMode(NotifyIconMode.UpToDate);
                }
            }
        }

        private void SetMode(NotifyIconMode mode)
        {
            _animationTimer.Stop();
            switch (mode)
            {
                case NotifyIconMode.UpToDate:
                    Statics.NotifyIcon.Icon = PodPuppy.Properties.Resources.UpToDate;
                    Statics.NotifyIcon.Text = "PodPuppy: Up To Date";
                    break;
                case NotifyIconMode.Refreshing:
                    Statics.NotifyIcon.Icon = PodPuppy.Properties.Resources.Refeshing;
                    Statics.NotifyIcon.Text = "PodPuppy: Refreshing";
                    break;
                case NotifyIconMode.Paused:
                    Statics.NotifyIcon.Icon = PodPuppy.Properties.Resources.Paused;
                    Statics.NotifyIcon.Text = "PodPuppy: Paused";
                    break;
                case NotifyIconMode.Downloading:
                    _animationTimer.Start();
                    Statics.NotifyIcon.Text = "PodPuppy: Downloading";
                    break;
            }
        }
    }
}
