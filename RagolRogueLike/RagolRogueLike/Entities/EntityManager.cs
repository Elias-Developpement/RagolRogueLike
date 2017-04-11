using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        //TODO: Add in adding entities to this list and deleting entities
        //As well as adding the entity manager id to the entity
        //and changing it when an entity is deleted from the list.

        #endregion

    }
}
