using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfcraft.Items
{
    public class Item
    {

        public long Uid;

        public enum ItemType
        {
            BUILDING_BLOCK = 0,
            CONTAINER_BLOCK = 1,
            WEAPON = 2,
            TOOL = 3,
            FOOD = 4
        }

        public Item()
        {

        }
    }
}
