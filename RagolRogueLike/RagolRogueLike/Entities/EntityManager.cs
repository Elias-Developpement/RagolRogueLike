using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RagolRogueLike.TileEngine;
using RagolRogueLike.PlayerClasses;
using RagolRogueLike.PathFinding;

namespace RagolRogueLike.Entities
{
    public class EntityManager
    {
        #region Field Region

        List<Entity> entities;
        List<Entity> deadEntities;

        PathFinder pathFinder;

        #endregion

        #region Property Region

        public List<Entity> Entities
        {
            get { return entities; }
        }

        public List<Entity> DeadEntities
        {
            get { return deadEntities; }
        }

        internal PathFinder Pathfinder
        {
            get { return pathFinder; }
        }

        #endregion

        #region Constructor Region

        //Constructor used to start with an empty set of entities.
        public EntityManager()
        {
            entities = new List<Entity>();
            deadEntities = new List<Entity>();
        }

        //Constructor used to start with a single entity
        public EntityManager(Entity entity)
        {
            entities = new List<Entity>();
            entities.Add(entity);
            deadEntities = new List<Entity>();
        }

        //Constructor used to start with a list of entities
        public EntityManager(List<Entity> entities)
        {
            this.entities = entities;
            deadEntities = new List<Entity>();
        }

        #endregion
        
        #region Method Region

        public void Update(GameTime gameTime, Map map, Player player)
        {
            foreach (Entity entity in entities)
            {
                entity.Update(gameTime, map, player);
            }
        }

        
        public void Draw(SpriteBatch spriteBatch, Player player)
        {
            foreach (Entity deadentity in deadEntities)
            {
                bool spaceOccupied = false;

                foreach (Entity entity in entities)
                {
                    if (entity.Position == deadentity.Position)
                    {
                        spaceOccupied = true;
                        break;
                    }
                }

                if (player.Position == deadentity.Position)
                {
                    spaceOccupied = true;
                }

                if (!spaceOccupied)
                {
                    deadentity.Draw(spriteBatch);
                }
            }

            foreach (Entity entity in entities)
            {
                entity.Draw(spriteBatch);
            }
        }

        public void AddEntity(Entity entity)
        {
            entities.Add(entity);
            entity.Manager = this;
            //Subtract one from the count so that you know which spot in the list the particular entity is.
            //It would either be subtracting one here or subtracting one every place this is referenced.
            entity.ManagerID = entities.Count - 1;
        }

        public void RemoveEntity(Entity entity)
        {
            entities.RemoveAt(entity.ManagerID);
            for (int i = entity.ManagerID; i < entities.Count; i++)
            {
                //decrease the entities id by 1 to adjust for removing one of the entities.
                entities[i].ManagerID -= 1;
            }
            //Add the entity to the list of dead entities so that it gets drawn on the bottom.
            deadEntities.Add(entity);
        }

        public void StartPathfinder(Tile[,] tiles)
        {
            pathFinder = new PathFinder(tiles);
        }


        #endregion

    }
}
