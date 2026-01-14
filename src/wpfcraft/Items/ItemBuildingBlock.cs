using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpfcraft.Blocks;

namespace wpfcraft.Items
{
    internal class ItemBuildingBlock : Item
    {

        public int Type = (int)ItemType.BUILDING_BLOCK;
        public Block Block;

        public ItemBuildingBlock(int id)
        {
            Random rand = new();
            Uid = rand.NextInt64();
            switch(id)
            {
                case < 10000:
                    {
                        Block = new(id);
                        break;
                    }
                case 10000:
                    {
                        Block = new BlockChest(id);
                        break;
                    }
            }
        }
    }
}
