using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfcraft.ChunkProviders
{
    internal class ChunkProvider
    {

        WPFCraft Main;

        public ChunkProvider(WPFCraft main)
        {
            Main = main;
        }

        public Chunk ProvideChunk(int x)
        {
            return ChunkProviderGenerate.ProvideChunkOverworld(x);
        }
    }
}
