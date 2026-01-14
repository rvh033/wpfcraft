using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace wpfcraft
{
    internal class Chunk : Canvas
    {

        public const int ChunkWidth = 16;
        public const int ChunkHeight = 128;

        public Chunk()
        {
            Width = ChunkWidth;
            Height = ChunkHeight;
        }
    }
}
