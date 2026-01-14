using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfcraft.Items.Containers
{
    internal class Chest : ItemContainerBlock
    {

        public ContainerEntry[] Entries;

        public Chest()
        {
            Entries = new ContainerEntry[28];
        }

        public void AddItem(Item item, int position, int count)
        {
            if (position < 27)
            {
                switch (item)
                {
                    case ItemBuildingBlock:
                        {
                            ItemBuildingBlock obj = (ItemBuildingBlock)item;
                            ContainerEntry entry = new(obj, count);
                            Entries[position] = entry;
                            break;
                        }
                    case ItemContainerBlock:
                        {
                            ItemContainerBlock obj = (ItemContainerBlock)item;
                            ContainerEntry entry = new(obj, count);
                            Entries[position] = entry;
                            break;
                        }
                    case ItemFood:
                        {
                            ItemFood obj = (ItemFood)item;
                            ContainerEntry entry = new(obj, count);
                            Entries[position] = entry;
                            break;
                        }
                    case ItemTool:
                        {
                            ItemTool obj = (ItemTool)item;
                            ContainerEntry entry = new(obj, count);
                            Entries[position] = entry;
                            break;
                        }
                    case ItemWeapon:
                        {
                            ItemWeapon obj = (ItemWeapon)item;
                            ContainerEntry entry = new(obj, count);
                            Entries[position] = entry;
                            break;
                        }
                }
            }
        }

    }
}
