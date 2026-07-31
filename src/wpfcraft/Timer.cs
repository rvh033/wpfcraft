using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfcraft
{
    public class Timer
    {

        public long WorldTime;
        public int DayTime;
        //public int DayOfWeek;
        public int Hours;
        public float DayFraction;

        public Timer(int hours)
        {
            Hours = hours;
            TickLoop();
        }

        async Task TickLoop()
        {
            while(true)
            {
                await Task.Delay(50);
                await Task.Run(() =>
                {
                    ++WorldTime;
                    DayTime = (int)(WorldTime % (Hours * 1000));
                    if (DayTime == 23999)
                    {
                        ++World.DayOfWeek;
                    }
                    //DayOfWeek = (int)(WorldTime * (Hours * 0.0000015) % 7);
                    DayFraction = (float)DayTime / (Hours * 1000);
                });
            }
        }
    }
}
