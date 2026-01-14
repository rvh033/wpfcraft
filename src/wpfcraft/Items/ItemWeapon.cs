using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfcraft.Items
{
    internal class ItemWeapon : Item
    {

        public int Type = (int)ItemType.WEAPON;

        public ItemWeapon()
        {
            Random rand = new();
            Uid = rand.NextInt64();
        }
    }
}
