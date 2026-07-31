using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace wpfcraft.Animations
{
    internal class AnimationSelectionFade : DoubleAnimation
    {
        public AnimationSelectionFade()
        {
            From = 1;
            To = 0.25;
            AutoReverse = true;
            RepeatBehavior = RepeatBehavior.Forever;
            Duration = new Duration(TimeSpan.FromSeconds(0.5));
        }

        public void Stop(WPFCraft main, EventArgs e)
        {
            DoubleAnimation end = new();
            end.From = main.TheWorld.ScaledOverlay.SelectionBox.Opacity;
            end.To = 1;
            Duration = new Duration(TimeSpan.FromSeconds(0.5));
            main.TheWorld.ScaledOverlay.SelectionBox.BeginAnimation(System.Windows.UIElement.OpacityProperty, end);
        }
    }
}
