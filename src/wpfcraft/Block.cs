using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace wpfcraft
{
    internal class Block : Canvas
    {
        public Block(int id)
        {
            Id = id;
            Init(IsFancy);
        }

        public double X;
        public double Y;
        public int Id;
        public bool IsFancy = true;

        public void Init(bool doInitFancy)
        {
            Width = 1;
            Height = 1;
            switch (doInitFancy)
            {
                case true:
                    DoFancy();
                    break;
                case false:
                    DoSimple();
                    break;
            }
        }

        void DoFancy()
        {
            Image img = new();
            img.Width = 1.01;
            img.Height = 1.01;
            SetLeft(img, -0.01);
            SetTop(img, -0.01);
            ImageSourceConverter conv = new();
            if (Id == 1 || Id == 2 || Id == 3 || Id == 10000)
            {
                img.SetValue(Image.SourceProperty, conv.ConvertFromString($"pack://application:,,,/Texture/Block/{Id}.png"));
                Children.Add(img);
            }
            else
            {
                DoSimple();
            }
        }

        void DoSimple()
        {
            // Create the block with just it's color, faster than loading the texture
            Rectangle blockFace = new Rectangle();
            blockFace.Width = (int)this.Width + 0.01;
            blockFace.Height = (int)this.Height + 0.01;
            SetLeft(blockFace, -0.01);
            SetTop(blockFace, -0.01);
            switch (Id)
            {
                case 0:
                    blockFace.Fill = Brushes.Gray;
                    break;
                case 1:
                    blockFace.Fill = Brushes.Green;
                    break;
                case 2:
                    blockFace.Fill = Brushes.SaddleBrown;
                    break;
                case 3:
                    blockFace.Fill = Brushes.DarkGray;
                    break;
                case 4:
                    blockFace.Fill = Brushes.Sienna;
                    break;
                case 5:
                    blockFace.Fill = Brushes.LimeGreen;
                    break;
                case 6:
                    blockFace.Fill = Brushes.Transparent;
                    break;
                case 100:
                    blockFace.Fill = Brushes.Gold;
                    break;
                case 10000:
                    blockFace.Fill = Brushes.BurlyWood;
                    break;
            }
            Children.Add(blockFace);
        }

        public void SetPos(double x, double y)
        {
            this.X = x;
            this.Y = y;
            SetLeft(this, x);
            SetTop(this, y);
        }
    }
}
