using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfcraft.Animations
{
    public class DeceleratingAnimation : Animation
    {
        public DeceleratingAnimation(double from, double to, double duration) : base(from, to, duration)
        {
            DecelerationRatio = 1;
        }
    }
}
