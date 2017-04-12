using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RagolRogueLike.TileEngine;

namespace RagolRogueLike.Entities
{
    public class EntityManager
    {
        #region Field Region

        List<Entity> entities;

        #endregion

        #region Property Region

        public List<Entity> Entities
        {
            get { return entities; }
        }

        #endregion

        #region Constructor Region

        //Constructor used to start with an empty set of entities.
        public EntityManager()
        {
            entities = new List<Entity>();
        }

        //Constructor used to start with a single entity
        public EntityManager(Entity entity)
        {
            entities = new List<Entity>();
            entities.Add(entity);
        }

        //Constructor used to start with a list of entities
        public EntityManager(List<Entity> entities)
        {
            this.entities = entities;
        }

        #endregion

        #region Method Region

        public void Update(GameTime gameTime, Map map)
        {
            foreach (Entity entity in entities)
            {
                entity.Update(gameTime, map);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Entity entity in entities)
            {
                entity.Draw(spriteBatch);
            }
        }

        public void AddEntity(Entity entity)
        {
            entities.Add(entity);
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
        }


        #endregion

    }
}
