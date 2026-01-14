using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace wpfcraft.ChunkProviders
{
    internal class ChunkProviderGenerate
    {

        public static Chunk ProvideChunkOverworld(int x)
        {
            Chunk chunk = new Chunk();
            Canvas.SetLeft(chunk, x);
            int goaledX = x;
            Random rand = new Random();
            chunk.Uid = $"{rand.NextInt64()}";
            int ct = rand.Next(0, 5);
            bool chunkHasTree = false;
            List<int[]> occupiedXYCoordsByTree = new List<int[]>();
            if (ct == 4)
            {
                chunkHasTree = true;
            }
            for (int i = 0; i < 16; i++)
            {
                int startHeight = rand.Next(60, 65);
                for (int j = 0; j < 128; j++)
                {
                    if (j < startHeight)
                    {
                        Block block = new Block(6);
                        bool canBlockBeAdded = true;
                        foreach (int[] XY in occupiedXYCoordsByTree)
                        {
                            if (XY[0] == i && XY[1] == j)
                            {
                                canBlockBeAdded = false;
                                //print($"Tried to place a block at {i} / {j}... but it is already occupied! Skipping");
                            }
                        }
                        if (canBlockBeAdded)
                        {
                            block.SetPos(i, j);
                            chunk.Children.Add(block);
                        }
                    }
                    if (j >= startHeight)
                    {
                        int id = 0;
                        int treeYPos = startHeight - 4;
                        if (chunkHasTree)
                        {
                            chunkHasTree = false;
                            int treeX = rand.Next(1, 15);
                            for (int k = 0; k < 3; k++)
                            {
                                Block log = new Block(4);
                                int[] occupiedCoords = new int[2];
                                occupiedCoords[0] = treeX;
                                occupiedCoords[1] = treeYPos + k;
                                occupiedXYCoordsByTree.Add(occupiedCoords);
                                log.SetPos(treeX, treeYPos + k);
                                chunk.Children.Add(log);
                            }
                            for (int l = 0; l < 3; l++)
                            {
                                Block leaf = new Block(5);
                                int[] occupiedCoords = new int[2];
                                switch (l)
                                {
                                    case 0:
                                        leaf.SetPos(treeX, treeYPos - 1);
                                        occupiedCoords[0] = treeX;
                                        occupiedCoords[1] = treeYPos - 1;
                                        break;
                                    case 1:
                                        leaf.SetPos(treeX - 1, treeYPos);
                                        occupiedCoords[0] = treeX - 1;
                                        occupiedCoords[1] = treeYPos;
                                        break;
                                    case 2:
                                        leaf.SetPos(treeX + 1, treeYPos);
                                        occupiedCoords[0] = treeX + 1;
                                        occupiedCoords[1] = treeYPos;
                                        break;
                                }
                                occupiedXYCoordsByTree.Add(occupiedCoords);
                                chunk.Children.Add(leaf);
                            }
                        }
                        int dirtPos = startHeight + 1;
                        int stonePos = dirtPos + rand.Next(4, 8);
                        if (j < dirtPos)
                        {
                            id = 1;
                        }
                        if (j >= dirtPos)
                        {
                            id = 2;
                        }
                        if (j >= stonePos)
                        {
                            id = 3;
                        }
                        if (j == 127)
                        {
                            id = 0;
                        }
                        Block block = new Block(id);
                        bool canBlockBeAdded = true;
                        foreach (int[] XY in occupiedXYCoordsByTree)
                        {
                            if (XY[0] == i && XY[1] == j)
                            {
                                canBlockBeAdded = false;
                                //print($"Tried to place a block at {i} / {j}... but it is already occupied! Skipping");
                            }
                        }
                        if (canBlockBeAdded)
                        {
                            block.SetPos(i, j);
                            chunk.Children.Add(block);
                        }
                    }
                }
            }
            return chunk;
        }
    }
}
