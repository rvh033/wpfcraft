using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfcraft.Items
{
    internal class ItemFood : Item
    {

        public int Type = (int)ItemType.FOOD;

        public ItemFood()
        {
            Random rand = new();
            Uid = rand.NextInt64();
        }
    }
}
