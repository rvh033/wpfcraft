using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using wpfcraft.Items;

namespace wpfcraft.PlayerData
{
    public class Player : Canvas
    {
        public Player(string name, ulong id)
        {
            Init(name, id);
        }

        public string Name;
        public double X;
        public double Y;
        public double CollisionX;
        public double StartJumpY;
        public double PrevX;
        public double PrevY;
        public double MovementY = 0;
        public int InitCalls = 0;
        public int Health = 100;
        public ulong Id;
        public bool IsFalling = false;
        public bool IsJumping = false;
        public Inventory Inventory;
        public Selection Selection = new();

        void Init(string name, ulong id)
        {
            ++InitCalls;
            Name = name;
            Id = id;
            Inventory = new Inventory();
            Label nameString = new Label();
            nameString.Content = name;
            nameString.FontSize = 20;
            nameString.FontWeight = FontWeights.Bold;
            Width = 0.90;
            Height = 1.80;
            SetLeft(this, 0);
            SetTop(this, 0);
            X = GetLeft(this);
            Y = GetTop(this);
            Rectangle playerDecor = new Rectangle();
            playerDecor.Width = Width;
            playerDecor.Height = Height;
            playerDecor.Fill = Brushes.White;
            Children.Add(playerDecor);
        }

        public void SetPos(double x, double y)
        {
            X = x;
            Y = y;
            SetLeft(this, X);
            SetTop(this, Y);
        }

        public void SetX(double x)
        {
            X = x;
            SetLeft(this, X);
        }

        public void SetY(double y)
        {
            Y = y;
            SetTop(this, Y);
        }

        public void AddToInventory(Item item, int position, int count)
        {
            Inventory.AddItemToHotbar(item, position, count);
        }
    }
}
