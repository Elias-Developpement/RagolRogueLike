using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RagolRogueLike.PathFinding
{
    internal class SearchNode
    {
        #region Field and Property Region

        //The position of this particular search node
        //Probably divide tiles up into multiple nodes so that movement looks better.
        Point position;

        bool walkable;

        //The neighbor nodes that surround the node (including diagonals).
        SearchNode[] neighbors;

        SearchNode parent;

        bool inOpenList;
        bool inClosedList;

        //The approximate distance from start node to the goal if the path goes through this node. (F)
        float distanceToGoal;

        //Distance traveled from the spawn point. (G)
        float distanceTraveled;

        public Point Position
        {
            get { return position; }
            set { position = value; }
        }

        public bool Walkable
        {
            get { return walkable; }
            set { walkable = value; }
        }

        public SearchNode[] Neighbors
        {
            get { return neighbors; }
            set { neighbors = value; }
        }

        public SearchNode Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public bool InOpenList
        {
            get { return inOpenList; }
            set { inOpenList = value; }
        }

        public bool InClosedList
        {
            get { return inClosedList; }
            set { inClosedList = value; }
        }

        public float DistanceToGoal
        {
            get { return distanceToGoal; }
            set { distanceToGoal = value; }
        }

        public float DistanceTraveled
        {
            get { return distanceTraveled; }
            set { distanceTraveled = value; }
        }

        #endregion

        #region Constructor Region

        internal SearchNode()
        {
            walkable = true;
        }

        #endregion

        #region Method Region

        #endregion
    }
}
