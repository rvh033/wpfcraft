using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace wpfcraft.PlayerData
{
    internal class InventorySlotVisual : Canvas
    {

        public long Uid;

        public InventorySlotVisual(string path, long uid)
        {
            Uid = uid;
            Width = 40;
            Height = 40;
            ImageSourceConverter imageSourceConverter = new ImageSourceConverter();
            Image image = new Image();
            image.Width = Width;
            image.Height = Height;
            try
            {
                image.SetValue(Image.SourceProperty, imageSourceConverter.ConvertFromString($"pack://application:,,,/{path}.png"));
            }
            catch (Exception ex)
            {
                image.SetValue(Image.SourceProperty, imageSourceConverter.ConvertFromString($"pack://application:,,,/Texture/MissingTex.png"));
            }
            Children.Add(image);
        }
    }
}
