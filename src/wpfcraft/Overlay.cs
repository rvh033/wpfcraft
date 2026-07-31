using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;

namespace wpfcraft
{
    public class Overlay : Canvas
    {

        public Rectangle SelectionBox;

        public Overlay()
        {
            Rectangle s = new();
            s.Width = 1;
            s.Height = 1;
            s.Stroke = Brushes.Black;
            s.StrokeThickness = 0.05;
            SelectionBox = s;
            Children.Add(SelectionBox);
        }
    }
}
