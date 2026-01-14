using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfcraft.Items
{
    internal class ItemTool : Item
    {

        public int Type = (int)ItemType.TOOL;

        public ItemTool()
        {
            Random rand = new();
            Uid = rand.NextInt64();
        }
    }
}
