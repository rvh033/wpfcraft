using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfcraft.Items.Containers
{
    public class ContainerEntry
    {

        public Item Item;
        public int Count;

        public ContainerEntry(Item item, int count)
        {
            Item = item;
            Count = count;
        }
    }
}
