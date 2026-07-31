using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;

namespace wpfcraft.Graphics
{
    public class GraphicSunMoon : Graphic
    {

        RotateTransform Rotation;
        RotateTransform MoonRotation;
        Image MoonTexture;
        Image MoonTextureB;

        public GraphicSunMoon()
        {
            Width = 800;
            Height = 50;
            Ellipse e = new();
            e.Width = 30;
            e.Height = 30;
            e.Fill = Brushes.Yellow;
            Ellipse eb = new();
            eb.Width = 30;
            eb.Height = 30;
            eb.Fill = Brushes.Yellow;
            BlurEffect blur = new();
            blur.KernelType = KernelType.Gaussian;
            blur.Radius = 30;
            BlurEffect blur2 = new();
            blur2.KernelType = KernelType.Gaussian;
            blur2.Radius = 30;
            eb.Effect = blur;
            SetTop(eb, 10);
            SetTop(e, 10);
            //Rectangle r = new();
            //r.Width = 800;
            //r.Height = 50;
            //r.Fill = Brushes.Purple;
            //Children.Add(r);
            Children.Add(e);
            Children.Add(eb);
            SetTop(this, 400);
            //SetLeft(this, 100);
            Image moon = new();
            moon.Width = 25;
            moon.Height = 25;
            Image moonB = new();
            moonB.Width = 25;
            moonB.Height = 25;
            SetLeft(moon, 775);
            SetTop(moon, 12.5);
            SetLeft(moonB, 775);
            SetTop(moonB, 12.5);
            MoonTexture = moon;
            MoonTextureB = moonB;
            MoonTextureB.Effect = blur;
            Children.Add(MoonTextureB);
            Children.Add(MoonTexture);
            Rotation = new RotateTransform(0, 400, 25);
            MoonRotation = new RotateTransform(0, 12.5, 12.5);
            RenderTransform = Rotation;
            UpdateLoop();
        }

        async Task UpdateLoop()
        {
            while (true)
            {
                await Task.Delay(10);
                await Task.Run(() =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        Rotation.Angle = ((float)World.WorldTimer.DayTime / (World.WorldTimer.Hours * 1000)) * 360;
                        if (World.PrevDayOfWeek != World.DayOfWeek)
                        {
                            int moonPhase = 0;
                            ImageSourceConverter conv = new();
                            switch (World.DayOfWeek)
                            {
                                case 0:
                                    {
                                        MoonRotation.Angle = 180;
                                        moonPhase = 0;
                                        break;
                                    }
                                case 1:
                                    {
                                        MoonRotation.Angle = 180;
                                        moonPhase = 1;
                                        break;
                                    }
                                case 2:
                                    {
                                        MoonRotation.Angle = 180;
                                        moonPhase = 2;
                                        break;
                                    }
                                case 3:
                                    {
                                        MoonRotation.Angle = 0;
                                        moonPhase = 3;
                                        break;
                                    }
                                case 4:
                                    {
                                        MoonRotation.Angle = 0;
                                        moonPhase = 2;
                                        break;
                                    }
                                case 5:
                                    {
                                        MoonRotation.Angle = 0;
                                        moonPhase = 1;
                                        break;
                                    }
                                case 6:
                                    {
                                        MoonRotation.Angle = 0;
                                        moonPhase = 0;
                                        break;
                                    }
                            }
                            if (moonPhase == 0)
                            {
                                MoonTextureB.Visibility = System.Windows.Visibility.Hidden;
                            }
                            else
                            {
                                MoonTextureB.Visibility = System.Windows.Visibility.Visible;
                            }
                            MoonTexture.RenderTransform = MoonRotation;
                            MoonTextureB.RenderTransform = MoonRotation;
                            MoonTexture.SetValue(Image.SourceProperty, conv.ConvertFromString($"pack://application:,,,/Texture/Graphics/moon{moonPhase}.png"));
                            MoonTextureB.SetValue(Image.SourceProperty, conv.ConvertFromString($"pack://application:,,,/Texture/Graphics/moon{moonPhase}.png"));
                            World.PrevDayOfWeek = World.DayOfWeek;
                        }
                    });
                });
            }
        }
    }
}
