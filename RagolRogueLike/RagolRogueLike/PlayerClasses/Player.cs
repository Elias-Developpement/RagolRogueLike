using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using RagolRogueLike.TileEngine;
using RagolRogueLike.Entities;

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

        Camera camera;

        int damage;

        #endregion
        
        #region Property Region

        public Vector2 Position
        {
            get { return position; }
        }
        
        public Camera Camera
        {
            get { return camera; }
        }

        #endregion
        
        #region Constructor Region
        
        public Player(string symbol, Color color, SpriteFont spriteFont, Vector2 position, Rectangle viewportRect)
        {
            this.symbol = symbol;
            this.color = color;
            block = true;
            this.spriteFont = spriteFont;
            this.position = position;

            camera = new Camera(viewportRect);

            damage = 10;
        }

        #endregion

        #region Method Region

        public void Update(GameTime gameTime, Map map, Entity entity)
        {
            camera.Update(gameTime);

            if (InputHandler.KeyReleased(Keys.PageUp))
            {
                camera.ZoomIn();
                if (camera.CameraMode == CameraMode.Follow)
                    camera.LockToPlayer(this);
            }
            else if (InputHandler.KeyReleased(Keys.PageDown))
            {
                camera.ZoomOut();
                if (camera.CameraMode == CameraMode.Follow)
                    camera.LockToPlayer(this);
            }

            Vector2 motion = new Vector2();
            //Here the motion is set to 16 because the size (including spacing) for symbols is 16.
            if (InputHandler.KeyPressed(Keys.NumPad8) || InputHandler.KeyPressed(Keys.Up))
            {
                motion.Y = -16;
            }
            else if (InputHandler.KeyPressed(Keys.NumPad2) || InputHandler.KeyPressed(Keys.Down))
            {
                motion.Y = 16;
            }
            else if (InputHandler.KeyPressed(Keys.NumPad4) || InputHandler.KeyPressed(Keys.Left))
            {
                motion.X = -16;
            }
            else if (InputHandler.KeyPressed(Keys.NumPad6) || InputHandler.KeyPressed(Keys.Right))
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
                int x = ((int)position.X + (int)motion.X) / 16;
                int y = ((int)position.Y + (int)motion.Y) / 16;
                
                if (map.GetBlocked(x, y))
                {
                    return;
                }
                else if (entity.Position == position + motion && entity.Block)
                {
                    DealDamage(entity);
                }
                else
                {
                    position += motion;
                }
                
                camera.LockToPlayer(this);
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.DrawString(spriteFont, symbol, position, color);
        }

        private void DealDamage(Entity target)
        {
            target.TakeDamage(damage);
        }
        
        #endregion
        
    }
}
