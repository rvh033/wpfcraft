using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using wpfcraft.Items.Containers;

namespace wpfcraft.Blocks
{
    internal class BlockChest(int id) : Block(id)
    {
        public Chest Chest = new();
    }
}
