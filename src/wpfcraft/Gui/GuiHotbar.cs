using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using wpfcraft.Blocks;
using wpfcraft.Gui.Guis;
using wpfcraft.Items;
using wpfcraft.Items.Containers;
using wpfcraft.PlayerData;

namespace wpfcraft.Gui
{
    internal class GuiHotbar : Canvas
    {
        public int Selection = 0;
        public int Range = 8;
        int PrevSelection;
        Canvas SelectionBox;
        InventorySlotVisual[] Slots;

        public GuiHotbar(GuiIngame Parent, Player player)
        {
            Slots = new InventorySlotVisual[9];
            SetLeft(this, 0);
            SetTop(this, 0);
            Width = 480;
            Height = 60;
            Rectangle hotbar = new();
            hotbar.Width = 492;
            hotbar.Height = 60;
            hotbar.Fill = Brushes.Black;
            hotbar.Opacity = 0.5;
            Canvas selection = new();
            selection.Width = 48;
            selection.Height = 48;
            Rectangle selectionBox = new();
            selectionBox.Width = 48;
            selectionBox.Height = 48;
            selectionBox.Fill = Brushes.Black;
            selectionBox.Opacity = 0.5;
            selection.Children.Add(selectionBox);
            SelectionBox = selection;
            SetLeft(SelectionBox, 6);
            SetTop(SelectionBox, 6);
            Children.Add(hotbar);
            Children.Add(SelectionBox);
            SetLeft(hotbar, -6);
            SetLeft(this, 400 - Width / 2);
            SetTop(this, 450 - Height * 1.7);
        }

        public void UpdateSelectionPosition(Player player)
        {
            if (PrevSelection != Selection)
            {
                UpdateItems(player);
            }
            if (Selection > 8 || Selection < 0)
            {
                Selection = 0;
            }
            switch (Selection)
            {
                case 0:
                    SetLeft(SelectionBox, 0);
                    break;
                case > 0:
                    SetLeft(SelectionBox, Selection * (SelectionBox.Width + 6));
                    break;
            }
            PrevSelection = Selection;
        }

        public void UpdateItems(Player player)
        {
            for (int i = 0; i < player.Inventory.HotbarEntries.Length; i++)
            {
                if (player.Inventory.HotbarEntries[i] != null)
                {
                    Item item = player.Inventory.HotbarEntries[i].Item;
                    if (Slots[i] == null || item.Uid != Slots[i].Uid)
                    {
                        switch (item)
                        {
                            case ItemBuildingBlock:
                                {
                                    ItemBuildingBlock obj = (ItemBuildingBlock)item;
                                    InventorySlotVisual visual = new InventorySlotVisual($"Texture/Block/{obj.Block.Id}", item.Uid);
                                    SetLeft(visual, i * (visual.Width + 14) + 4);
                                    SetTop(visual, 10);
                                    Children.Add(visual);
                                    break;
                                }
                            case ItemContainerBlock:
                                {
                                    if (item is Chest)
                                    {
                                        ItemContainerBlock obj = (ItemContainerBlock)item;
                                        InventorySlotVisual visual = new InventorySlotVisual($"Texture/Block/10000", item.Uid);
                                        SetLeft(visual, i * (visual.Width + 14) + 4);
                                        SetTop(visual, 10);
                                        Children.Add(visual);
                                    }
                                    break;
                                }
                            case ItemFood:
                                {
                                    break;
                                }
                            case ItemTool:
                                {
                                    break;
                                }
                            case ItemWeapon:
                                {
                                    break;
                                }
                        }
                    }
                }
            }
        }
    }
}
