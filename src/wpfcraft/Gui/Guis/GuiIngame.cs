using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics;
using System.Data;
using wpfcraft.PlayerData;

namespace wpfcraft.Gui.Guis
{
    internal class GuiIngame : Canvas
    {

        public GuiHotbar Hotbar;

        public GuiIngame(MainWindow main)
        {
            SetLeft(this, 0);
            SetTop(this, 0);
            Hotbar = new GuiHotbar(this, main.Player);
            Children.Add(Hotbar);
        }

        public void Update(Player player)
        {
            Hotbar.UpdateSelectionPosition(player);
        }
    }
}
