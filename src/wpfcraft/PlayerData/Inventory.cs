using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpfcraft.Items;
using wpfcraft.Items.Containers;

namespace wpfcraft.PlayerData
{
    public class Inventory
    {
        public ContainerEntry[] InventoryEntries;
        public ContainerEntry[] HotbarEntries;

        public Inventory()
        {
            InventoryEntries = new ContainerEntry[28];
            HotbarEntries = new ContainerEntry[9];
        }

        public void AddItemToInventory(Item item, int position, int count)
        {
            if (position < 27)
            {
                switch (item)
                {
                    case ItemBuildingBlock:
                        {
                            ItemBuildingBlock obj = (ItemBuildingBlock)item;
                            ContainerEntry entry = new(obj, count);
                            InventoryEntries[position] = entry;
                            break;
                        }
                    case ItemContainerBlock:
                        {
                            ItemContainerBlock obj = (ItemContainerBlock)item;
                            ContainerEntry entry = new(obj, count);
                            InventoryEntries[position] = entry;
                            break;
                        }
                    case ItemFood:
                        {
                            ItemFood obj = (ItemFood)item;
                            ContainerEntry entry = new(obj, count);
                            InventoryEntries[position] = entry;
                            break;
                        }
                    case ItemTool:
                        {
                            ItemTool obj = (ItemTool)item;
                            ContainerEntry entry = new(obj, count);
                            InventoryEntries[position] = entry;
                            break;
                        }
                    case ItemWeapon:
                        {
                            ItemWeapon obj = (ItemWeapon)item;
                            ContainerEntry entry = new(obj, count);
                            InventoryEntries[position] = entry;
                            break;
                        }
                }
            }
        }

        public void AddItemToHotbar(Item item, int position, int count)
        {
            if (position < 9)
            {
                switch (item)
                {
                    case ItemBuildingBlock:
                        {
                            ItemBuildingBlock obj = (ItemBuildingBlock)item;
                            ContainerEntry entry = new(obj, count);
                            HotbarEntries[position] = entry;
                            break;
                        }
                    case ItemContainerBlock:
                        {
                            ItemContainerBlock obj = (ItemContainerBlock)item;
                            ContainerEntry entry = new(obj, count);
                            HotbarEntries[position] = entry;
                            break;
                        }
                    case ItemFood:
                        {
                            ItemFood obj = (ItemFood)item;
                            ContainerEntry entry = new(obj, count);
                            HotbarEntries[position] = entry;
                            break;
                        }
                    case ItemTool:
                        {
                            ItemTool obj = (ItemTool)item;
                            ContainerEntry entry = new(obj, count);
                            HotbarEntries[position] = entry;
                            break;
                        }
                    case ItemWeapon:
                        {
                            ItemWeapon obj = (ItemWeapon)item;
                            ContainerEntry entry = new(obj, count);
                            HotbarEntries[position] = entry;
                            break;
                        }
                }
            }
        }

        public bool IsInventoryFull()
        {
            for (int i = 0; i < InventoryEntries.Length; i++)
            {
                if (InventoryEntries[i] == null)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsHotbarFull()
        {
            for (int i = 0; i < HotbarEntries.Length; i++)
            {
                if (HotbarEntries[i] == null)
                {
                    return false;
                }
            }
            return true;
        }

        public int GetFirstAvailableInventorySlot()
        {
            for(int i = 0; i < InventoryEntries.Length; i++)
            {
                if (InventoryEntries[i] == null)
                {
                    return i;
                }
            }
            return -1;
        }

        public int GetFirstAvailableHotbarSlot()
        {
            for (int i = 0; i < HotbarEntries.Length; i++)
            {
                if (HotbarEntries[i] == null)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
