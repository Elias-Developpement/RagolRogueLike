using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RagolRogueLike.TileEngine;
using RagolRogueLike.Entities;


namespace RagolRogueLike.PathFinding
{
    public class PathFinder
    {
        #region Field Region

        //An array of the walkable search nodes
        private SearchNode[,] searchNodes;

        //Size of the map
        private int mapWidth;
        private int mapHeight;
        private int numberNodesInTile = 16;

        private List<SearchNode> openList = new List<SearchNode>();
        private List<SearchNode> closedList = new List<SearchNode>();

        #endregion

        #region Property Region

        #endregion

        #region Constructor Region

        public PathFinder(Tile[,] tiles)
        {
            
            //This should create the map into tiles of the proper size 16*16.
            mapWidth = Map.MapWidthInPixels / numberNodesInTile;
            mapHeight = Map.MapHeightInPixels / numberNodesInTile;

            InitializeSearchNodes(tiles);

        }

        #endregion

        #region Method Region

        //TODO: Probably refactor all of this stuff into making non walkable nodes not null
        //So that entities can walk.
        
        private void MakeEntityLocationsBlocked(EntityManager manager)
        {
            //Loop through the entities
            foreach (Entity entity in manager.Entities)
            {
                //If this entity blocks a position.
                if (entity.Block)
                {
                    searchNodes[(int)entity.Position.X / 16, (int)entity.Position.Y / 16].Walkable = false;
                }
            }
        }

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

        //Finds the optimal path from one point to another
        public List<Vector2> FindPath(Point startPoint, Point endPoint, EntityManager manager)
        {
            //Only try to find a path if the start and end are different
            if (startPoint == endPoint)
            {
                return new List<Vector2>();
            }

            //Step 1: Clear everything from last time and set the positions of map entities.
            MakeEntityLocationsBlocked(manager);
            ResetSearchNodes();

            //TODO: Problem with making entities position null and then trying to use the start node.
            //Store reference to start and end nodes for convenience
            SearchNode startNode = searchNodes[startPoint.X / 16, startPoint.Y / 16];
            SearchNode endNode = searchNodes[endPoint.X / 16, endPoint.Y / 16];

            //Step 2: Set start node's G to 0 and its F value to H.
            startNode.InOpenList = true;

            startNode.DistanceToGoal = Heuristic(startPoint, endPoint);
            startNode.DistanceTraveled = 0;

            openList.Add(startNode);

            //Step 3: While there are still nodes to look at in the open list:
            while (openList.Count > 0)
            {
                //a: Loop through the open list and find the node that has the smallest F value
                SearchNode currentNode = FindBestNode();

                //b: If the open list is empty or no node can be found, no path can be found so algorithm terminates.
                if (currentNode == null)
                {
                    break;
                }

                //c: If the active node is the goal node, we will find and return the final path.
                if (currentNode == endNode)
                {
                    //Trace our path back to the start.
                    return FindFinalPath(startNode, endNode);
                }

                //d: Else, for each of the Active Node's neighbors :
                for (int i = 0; i < currentNode.Neighbors.Length; i++)
                {
                    SearchNode neighbor = currentNode.Neighbors[i];

                    //i: Make sure the neighbor node can be walked across.
                    if (neighbor == null || neighbor.Walkable == false)
                    {
                        continue;
                    }

                    //ii: Calculate a new G value for the neighboring node.
                    float distanceTraveled = currentNode.DistanceTraveled + 1;

                    //An estimate of the distance from this node to the end node.
                    float heuristic = Heuristic(neighbor.Position, endPoint);

                    //iii: If the neighboring node is not in either the open list or the closed list.
                    if (neighbor.InOpenList == false && neighbor.InClosedList == false)
                    {
                        //1: Set the neighboring G value to the one just calculated.
                        neighbor.DistanceTraveled = distanceTraveled;

                        //2: Set the neighboring F value to the new G value + the estimated value.
                        neighbor.DistanceToGoal = distanceTraveled + heuristic;

                        //3: Set the neighboring node's parent property to point at the active node.
                        neighbor.Parent = currentNode;

                        //4: Add the neighboring node to the open list.
                        neighbor.InOpenList = true;
                        openList.Add(neighbor);
                    }
                    //iv: if the neighboring node is in either the open or closed list
                    else if (neighbor.InOpenList || neighbor.InClosedList)
                    {
                        //1: If our new G value is less than the neighboring node's G value, we basically do exactly the same steps as if the node is not in the open and closed lists except we do not need to add this node to the open list again
                        if (neighbor.DistanceTraveled > distanceTraveled)
                        {
                            neighbor.DistanceTraveled = distanceTraveled;
                            neighbor.DistanceToGoal = distanceTraveled + heuristic;

                            neighbor.Parent = currentNode;
                        }
                    }
                }

                //e: Remove the active node from the open list and add it to the closed list
                openList.Remove(currentNode);
                currentNode.InClosedList = true;
            }

            //No path could be found.
            return new List<Vector2>();
        }

        //Provides an estimate between the two points. (H)
        private float Heuristic(Point point1, Point point2)
        {
            return Math.Abs(point1.X - point2.X) + Math.Abs(point1.Y - point2.Y);
        }

        //Reset the state of the seach nodes
        private void ResetSearchNodes()
        {
            openList.Clear();
            closedList.Clear();

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    SearchNode node = searchNodes[x, y];

                    if (node == null)
                    {
                        continue;
                    }

                    node.InClosedList = false;
                    node.InOpenList = false;

                    node.DistanceToGoal = float.MaxValue;
                    node.DistanceTraveled = float.MaxValue;
                }
            }
        }

        //Returns node with the smallest distance to goal.
        private SearchNode FindBestNode()
        {
            SearchNode currentTile = openList[0];

            float smallestDistanceToGoal = float.MaxValue;

            //Find closest node to the goal
            for (int i = 0; i < openList.Count; i++)
            {
                if (openList[i].DistanceToGoal < smallestDistanceToGoal)
                {
                    currentTile = openList[i];
                    smallestDistanceToGoal = currentTile.DistanceToGoal;
                }
            }
            return currentTile;
        }

        //Use the parent field of the search nodes to trace a path from the end node to the start node
        private List<Vector2> FindFinalPath(SearchNode startNode, SearchNode endNode)
        {
            closedList.Add(endNode);

            SearchNode parentTile = endNode.Parent;

            //Trace back through the nodes using the parent fields
            while (parentTile != startNode)
            {
                closedList.Add(parentTile);
                parentTile = parentTile.Parent;
            }

            List<Vector2> finalPath = new List<Vector2>();

            //Reverse the path and transform into world space.
            for (int i = closedList.Count - 1; i >= 0; i--)
            {
                finalPath.Add(new Vector2(closedList[i].Position.X, closedList[i].Position.Y));
            }

            return finalPath;
        }
        
        #endregion
    }
}
