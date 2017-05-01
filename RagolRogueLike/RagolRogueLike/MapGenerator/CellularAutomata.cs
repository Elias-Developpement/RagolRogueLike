using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RagolRogueLike.TileEngine;

namespace RagolRogueLike.MapGenerator
{
    public class CellularAutomata
    {
        #region Field region

        Random rand = new Random(DateTime.Now.Millisecond);

        int[,] map;
        int mapWidth;
        int mapHeight;
        int percentWalls;

        

        #endregion

        #region Property Region

        #endregion

        #region Constructor Region

        //No idea if this class is every going to be used in the game but it was fun to make.
        //It generates a cave system thats pretty terrible as of right now using the 4 - 5 method.
        public CellularAutomata(int mapWidth, int mapHeight, int percentWalls)
        {
            this.mapWidth = mapWidth;
            this.mapHeight = mapHeight;
            this.percentWalls = percentWalls;

            map = new int[mapWidth, mapHeight];

            RandomFillMap();
            MakeCaverns();
        }

        #endregion

        #region Method Region

        public Tile[,] GenerateMap()
        {
            Tile[,] tiles = new Tile[mapWidth, mapHeight];

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    if (map[x, y] == 1)
                    {
                        tiles[x, y] = new Tile("#", true, Color.White, Color.Black, new Vector2(x * 16, y * 16));
                    }
                    else
                    {
                        tiles[x, y] = new Tile(".", false, Color.White, Color.Black, new Vector2(x * 16, y * 16));
                    }
                }
            }

            return tiles;
        }

        private void MakeCaverns()
        {
            for (int y = 0, x = 0; y < mapHeight; y++)
            {
                for (x = 0; x < mapWidth; x++)
                {
                    map[x, y] = PlaceWallLogic(x, y);
                }
            }
        }

        private int PlaceWallLogic(int x, int y)
        {
            int numWalls = GetAdjacentWalls(x, y);

            if (map[x, y] == 1)
            {
                if (numWalls >= 4)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                if (numWalls >= 5)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }

        private void RandomFillMap()
        {

            int mapMiddle = mapHeight / 2;
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    if (y == 0 || x == 0 || y == mapHeight - 1 || x == mapWidth - 1)
                    {
                        map[x, y] = 1;
                    }
                    else if (y == mapMiddle)
                    {
                        map[x, y] = 0;
                    }
                    else
                    {
                        map[x, y] = RandomPercent();
                    }
                }
            }
        }

        private int RandomPercent()
        {
            if (percentWalls >= rand.Next(1, 101))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        private int GetAdjacentWalls(int x, int y)
        {
            int wallCounter = 0;

            for (int startX = x - 1; startX <= x + 1; startX++)
            {
                for (int startY = y - 1; startY <= y + 1; startY++)
                {
                    
                    if (IsWall(startX, startY))
                    {
                        wallCounter++;
                    }
                }
            }

            return wallCounter;
        }

        private bool IsWall(int x, int y)
        {
            //Check the boundary cases and consider them as walls.
            if (x < 0 || y < 0 || x >= mapWidth || y >= mapHeight)
            {
                return true;
            }

            if (map[x, y] == 1)
            {
                return true;
            }
            else if (map[x, y] == 0)
            {
                return false;
            }

            return false;
        }

        #endregion
    }
}
