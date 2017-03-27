using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RagolRogueLike.PlayerClasses
{
    public class Player
    {

        #region Field Region

        string symbol;
        Color color;
        bool block;

        Vector2 position;
        SpriteFont spriteFont;

        #endregion
        
        #region Property Region

        
        
        #endregion
        
        #region Constructor Region
        
        public Player(string symbol, Color color, SpriteFont spriteFont, Vector2 position)
        {
            this.symbol = symbol;
            this.color = color;
            block = true;
            this.spriteFont = spriteFont;
            this.position = position;
        }

        #endregion

        #region Method Region

        public void Update(GameTime gameTime)
        {
            Vector2 motion = new Vector2();
            //Here the motion is set to 16 because the size (including spacing) for symbols is 16.
            if (InputHandler.KeyPressed(Keys.NumPad8))
            {
                motion.Y = -16;
            }
            else if (InputHandler.KeyPressed(Keys.NumPad2))
            {
                motion.Y = 16;
            }
            else if (InputHandler.KeyPressed(Keys.NumPad4))
            {
                motion.X = -16;
            }
            else if (InputHandler.KeyPressed(Keys.NumPad6))
            {
                motion.X = 16;
            }
            else if (InputHandler.KeyPressed(Keys.NumPad7))
            {
                motion.Y = -16;
                motion.X = -16;
            }
            else if (InputHandler.KeyPressed(Keys.NumPad9))
            {
                motion.Y = -16;
                motion.X = 16;
            }
            else if (InputHandler.KeyPressed(Keys.NumPad1))
            {
                motion.Y = 16;
                motion.X = -16;
            }
            else if (InputHandler.KeyPressed(Keys.NumPad3))
            {
                motion.Y = 16;
                motion.X = 16;
            }

            if (motion != Vector2.Zero)
            {
                position += motion;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.DrawString(spriteFont, symbol, position, color);
        }
        
        #endregion
        
    }
}
