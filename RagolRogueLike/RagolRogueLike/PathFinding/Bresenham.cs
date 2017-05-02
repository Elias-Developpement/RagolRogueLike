using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using RagolRogueLike.TileEngine;

namespace RagolRogueLike.PathFinding
{
    //My implementation of the Bresenham line algorithm
    //Pretty shit so far and not in a very good working condition.
    public class Bresenham
    {
        #region Field Region

        //An array of the walkable search nodes
        private SearchNode[,] searchNodes;

        //Size of the map
        private int mapWidth;
        private int mapHeight;
        private int numberNodesInTile = 16;

        private int lightRadius;
        

        #endregion
        
        #region Property Region
        
        #endregion
        
        #region Constructor Region
        
        public Bresenham(Tile[,] tiles, int lightRadius)
        {
            //Create the map into tiles of the proper size 16*16.
            mapWidth = Map.MapWidthInPixels / numberNodesInTile;
            mapHeight = Map.MapHeightInPixels / numberNodesInTile;

            this.lightRadius = lightRadius;

            //Initialize the search nodes.
            InitializeSearchNodes(tiles);
        }

        #endregion
        
        #region Method Region

        private void InitializeSearchNodes(Tile[,] tiles)
        {
            searchNodes = new SearchNode[mapWidth, mapHeight];

            //For each of these nodes we will create, from a tile, the proper
            //search node for it.
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    //Create the search node and set its position
                    SearchNode node = new SearchNode();

                    node.Position = new Point(x * numberNodesInTile, y * numberNodesInTile);

                    //Only store nodes that are walkable
                    //Check the tile if it blocks walking.
                    if (tiles[x, y].Block == true)
                    {
                        node.Walkable = false;
                    }

                    if (node.Walkable)
                    {
                        node.Walkable = true;
                        node.Neighbors = new SearchNode[8];
                        searchNodes[x, y] = node;
                    }
                }
            }

            //Now we connect each node to its neighbors
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    SearchNode node = searchNodes[x, y];

                    if (node == null || node.Walkable == false)
                    {
                        continue;
                    }

                    //Create an array of all possible neighbors
                    Point[] neighbors = new Point[]
                    {
                        new Point (x, y - 1),
                        new Point (x, y + 1),
                        new Point (x - 1, y),
                        new Point (x + 1, y),
                        new Point (x - 1, y - 1),
                        new Point (x - 1, y + 1),
                        new Point (x + 1, y - 1),
                        new Point (x + 1, y + 1)
                    };

                    //Loop through the neighbors and keep them if they are walkable, discard them otherwise
                    for (int i = 0; i < neighbors.Length; i++)
                    {
                        Point position = neighbors[i];

                        //Make sure this is a part of the map
                        if (position.X < 0 || position.X > mapWidth - 1 || position.Y < 0 || position.Y > mapHeight - 1)
                        {
                            continue;
                        }

                        SearchNode neighbor = searchNodes[position.X, position.Y];

                        if (neighbor == null || neighbor.Walkable == false)
                        {
                            continue;
                        }

                        //Finally store the reference to the neighbor
                        node.Neighbors[i] = neighbor;
                    }
                }
            }
        }

        //Used to swap two coordinates so that you always go from smaller number to larger number.
        private void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp;
            temp = lhs;
            lhs = rhs;
            rhs = temp;
        }

        public List<Vector2> Line(Point start, Point end)
        {
            if (start.X > end.X)
            {
                Swap<int>(ref start.X, ref end.X);
            }

            int dX = (end.X - start.X), dY = Math.Abs(end.Y - start.Y), err = (dX / 2), ystep = (start.Y < end.Y ? 1 : -1), y = start.Y;

            //No Line exists between the two points
            return new List<Vector2>();
        }

        #endregion
    }
}
