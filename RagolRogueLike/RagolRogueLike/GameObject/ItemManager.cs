using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RagolRogueLike.Entities;
using RagolRogueLike.PlayerClasses;


namespace RagolRogueLike.GameObject
{
    public class ItemManager
    {
        #region Field Region

        List<Item> items;

        #endregion

        #region Property Region

        public List<Item> Items
        {
            get { return items; }
        }

        #endregion

        #region Constructor Region

        public ItemManager()
        {
            items = new List<Item>();
        }

        #endregion

        #region Method Region  

        public void Draw(SpriteBatch spriteBatch, EntityManager entities, Player player)
        {

            foreach (Item item in items)
            {
                bool spaceOccupied = false;

                foreach (Entity entity in entities.Entities)
                {
                    if (item.Position == entity.Position)
                    {
                        spaceOccupied = true;
                        break;
                    }
                }

                foreach (Entity entity in entities.DeadEntities)
                {
                    if (spaceOccupied)
                    {
                        break;
                    }

                    if (item.Position == entity.Position)
                    {
                        spaceOccupied = true;
                        break;
                    }
                }

                if (!spaceOccupied && player.Position == item.Position)
                {
                    spaceOccupied = true;
                }

                if (!spaceOccupied)
                {
                    item.Draw(spriteBatch);
                }
            }
        }

        public void AddItem(Item item)
        {
            items.Add(item);
            item.ManagerID = items.Count - 1;
        }

        public void RemoveItem(Item item)
        {
            items.RemoveAt(item.ManagerID);
            for (int i = item.ManagerID; i < items.Count; i++)
            {
                items[i].ManagerID -= 1;
            }
        }

        #endregion
    }
}
