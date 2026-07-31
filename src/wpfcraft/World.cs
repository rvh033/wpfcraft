using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using wpfcraft.Graphics;

namespace wpfcraft
{
    public class World : Canvas
    {

        public static float WorldLight = 0;
        public static float WorldDarkness = 0;
        public static float PrevWorldDarkness = 0;
        public static int DayOfWeek;
        public static int PrevDayOfWeek = -1;
        public static Timer WorldTimer = new(24);
        public Canvas Chunks = new();
        public Canvas Entities = new();
        public Overlay ScaledOverlay = new();
        
        public World()
        {
            Width = long.MaxValue;
            Height = long.MaxValue;
            Children.Add(Chunks);
            Children.Add(Entities);
            Children.Add(ScaledOverlay);
            Loop();
        }

        async void Loop()
        {
            await Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(1000);
                    if (WorldDarkness != PrevWorldDarkness)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            foreach (Chunk chunk in Chunks.Children)
                            {
                                foreach (Block block in chunk.Children)
                                {
                                    block.SetLighting(WorldDarkness);
                                }
                            }
                        });
                    }
                    PrevWorldDarkness = WorldDarkness;
                    float celestialAngle = ((float)WorldTimer.DayTime / (WorldTimer.Hours * 1000) - 0.25F);
                    if (celestialAngle < 0.0)
                    {
                        celestialAngle += 1.0F;
                    }
                    if (celestialAngle > 1.0)
                    {
                        celestialAngle -= 1.0F;
                    }
                    float cosineFactor = (float)Math.Cos(celestialAngle * Math.PI * 2.0);
                    WorldDarkness = 1.0F - Math.Clamp(cosineFactor, 0.2F, 1);
                    WorldLight = Math.Clamp(cosineFactor, 0, 1);
                }
            });
        }
    }
}
