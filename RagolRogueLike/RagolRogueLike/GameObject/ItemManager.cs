using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


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

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Item item in items)
            {
                item.Draw(spriteBatch);
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
