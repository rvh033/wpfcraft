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
    public class GraphicSky : Graphic
    {

        LinearGradientBrush Gradient;

        public GraphicSky()
        {
            Width = 800;
            Height = 450;
            Rectangle r = new();
            r.Width = Width;
            r.Height = Height;
            LinearGradientBrush brush = new();
            brush.StartPoint = new System.Windows.Point(0, 1);
            brush.EndPoint = new System.Windows.Point(0, 0);
            brush.GradientStops.Add(new GradientStop(Colors.Cyan, 0.90));
            brush.GradientStops.Add(new GradientStop(Colors.Black, 1));
            Gradient = brush;
            r.Fill = Gradient;
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
                        GradientStop s = Gradient.GradientStops[0];
                        s.Offset = World.WorldLight - World.WorldDarkness;
                    });
                });
            }
        }
    }
}
