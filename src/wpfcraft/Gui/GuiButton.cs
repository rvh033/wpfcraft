using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace wpfcraft.Gui
{
    internal class GuiButton : Canvas
    {
        public GuiButton(double x, double y, double width, double height, string content, Action todo)
        {
            Width = width;
            Height = height;
            Rectangle background = new();
            background.Width = Width;
            background.Height = Height;
            background.Fill = Brushes.Black;
            background.Opacity = 0.5;
            Label text = new();
            text.Width = Width;
            text.Height = Height;
            text.Content = content;
            text.FontSize = 24;
            text.FontWeight = FontWeights.Bold;
            text.Foreground = Brushes.White;
            text.HorizontalContentAlignment = HorizontalAlignment.Center;
            text.VerticalContentAlignment = VerticalAlignment.Center;
            Rectangle surface = new();
            surface.Width = Width;
            surface.Height = Height;
            surface.Fill = Brushes.Black;
            surface.Opacity = 0;
            surface.MouseDown += (sender, e) => todo();
            SetLeft(this, x);
            SetTop(this, y);
            Children.Add(background);
            Children.Add(text);
            Children.Add(surface);
        }
    }
}
