using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace RagolRogueLike.GameObject
{
    public class Item
    {
        #region Field Region

        string symbol;
        string name;
        Color color;

        //Gives the exact position of the item in the manager list.
        int managerID;

        Vector2 position;
        SpriteFont spriteFont;

        #endregion
        
        #region Property Region

        public int ManagerID
        {
            get { return managerID; }
            internal set { managerID = value; }
        }

        public Vector2 Position
        {
            get { return position; }
        }

        public string Name
        {
            get { return name; }
        }
        
        #endregion
        
        #region Constructor Region
        
        public Item(string name, string symbol, Color color, SpriteFont spriteFont, Vector2 position)
        {
            this.name = name;
            this.symbol = symbol;
            this.color = color;
            this.position = position;
            this.spriteFont = spriteFont;
        }

        #endregion
        
        #region Method Region

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(spriteFont, symbol, position, color);
        }
        
        #endregion
    }
}
