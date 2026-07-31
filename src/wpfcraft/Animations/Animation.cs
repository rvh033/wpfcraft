using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace wpfcraft.Animations
{
    public class Animation : DoubleAnimation
    {
        public Animation(double? from, double to, double duration)
        {
            From = from;
            To = to;
            Duration = new Duration(TimeSpan.FromSeconds(duration));
        }
    }
}
