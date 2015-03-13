using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace y86sim
{
    public class Clock
    {
        
        public event Action OnTick;
        private int _interval;
        private Thread thr;

        public int Interval
        {
            get { return _interval; }
            set { _interval = value; }
        }

        private bool _enabled = false;

        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        public Clock(int interval = 1000)
        {   _interval = interval;
        }

        public void Start() 
        {
            _enabled = true;
            thr = new Thread(timing);
            thr.Start();
        }

        public void Stop()
        {
            _enabled = false;
            //thr.Abort();
        }

        private void timing()
        {
            if (OnTick == null) return;
            while (_enabled)
            {
                Thread.Sleep(_interval);
                OnTick();
            }
        }

        public void Tick()
        {
            if (OnTick != null) OnTick();
        }
    }
}
