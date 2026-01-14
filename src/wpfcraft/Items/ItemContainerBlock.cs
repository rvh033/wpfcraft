using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfcraft.Items
{
    internal class ItemContainerBlock : Item
    {

        public int Type = (int)ItemType.CONTAINER_BLOCK;

        public ItemContainerBlock()
        {
            Random rand = new();
            Uid = rand.NextInt64();
        }
    }
}
