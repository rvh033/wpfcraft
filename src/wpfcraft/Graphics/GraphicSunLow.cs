using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace wpfcraft.Graphics
{
    public class GraphicSunLow : Graphic
    {
        public GraphicSunLow()
        {
            Width = 800;
            Height = 450;
            Rectangle r = new();
            r.Width = Width;
            r.Height = Height;
            r.Fill = Brushes.Orange;
            LinearGradientBrush brush = new();
            brush.StartPoint = new System.Windows.Point(0, 1);
            brush.EndPoint = new System.Windows.Point(0, 0);
            brush.GradientStops.Add(new GradientStop(Colors.Orange, 0));
            brush.GradientStops.Add(new GradientStop(Colors.Transparent, 0.90));
            r.OpacityMask = brush;
            Children.Add(r);
            UpdateLoop();
        }

        async Task UpdateLoop()
        {
            while (true)
            {
                await Task.Delay(50);
                await Task.Run(() =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        SetTop(this, Math.Abs(World.WorldDarkness - 0.5F) * 900F);
                    });
                });
            }
        }
    }
}
