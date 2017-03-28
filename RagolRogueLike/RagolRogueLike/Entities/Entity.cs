using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using RagolRogueLike.TileEngine;

namespace RagolRogueLike.Entities
{
    public class Entity
    {

        #region Field Region

        string symbol;
        Color color;
        bool block;

        Vector2 position;
        SpriteFont spriteFont;

        #endregion

        #region Property Region

        public Vector2 Position
        {
            get { return position; }
        }

        public bool Block
        {
            get { return block; }
        }

        #endregion

        #region Constructor Region

        public Entity(string symbol, Color color, SpriteFont spriteFont, Vector2 position)
        {
            this.symbol = symbol;
            this.color = color;
            block = true;
            this.spriteFont = spriteFont;
            this.position = position;
        }

        #endregion

        #region Method Region

        public void Update(GameTime gameTime, Map map)
        {
            //TODO: Add collision detection for entities once they start to move.
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(spriteFont, symbol, position, color);
        }

        #endregion
    }
}
