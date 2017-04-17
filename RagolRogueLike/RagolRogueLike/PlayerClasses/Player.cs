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
using RagolRogueLike.PathFinding;
using RagolRogueLike.GameObject;

namespace RagolRogueLike.PlayerClasses
{
    public class Player
    {
        #region Field Region

        string symbol;
        Color color;
        bool block;
        bool hasActed = false;

        ItemManager inventory;
        bool menuOpen;

        Vector2 position;
        SpriteFont spriteFont;

        Camera camera;

        int lightradius = 1;

        int damage;
        int maxHealth;
        int currentHealth;

        #endregion
        
        #region Property Region

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        
        public Camera Camera
        {
            get { return camera; }
        }

        public int Damage
        {
            get { return damage; }
        }

        public int MaxHealth
        {
            get { return maxHealth; }
        }

        public int CurrentHealth
        {
            get { return currentHealth; }
        }

        public int HealthPercent
        {
            get { return currentHealth * 100 / maxHealth; }
        }

        public ItemManager Inventory
        {
            get { return inventory; }
        }

        public bool MenuOpen
        {
            get { return menuOpen; }
            set { menuOpen = value; }
        }

        public bool HasActed
        {
            get { return hasActed; }
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

            menuOpen = false;

            inventory = new ItemManager();

            camera = new Camera(viewportRect);

            damage = 10;
            maxHealth = 20;
            currentHealth = maxHealth;
        }

        #endregion

        #region Monogame Method Region

        public void Update(GameTime gameTime, Map map, EntityManager entities)
        {
            hasActed = false;

            camera.Update(gameTime);

            //Debugging code
            if (InputHandler.KeyPressed(Keys.K))
            {
                currentHealth--;
            }
            else if (InputHandler.KeyPressed(Keys.L))
            {
                currentHealth++;
            }
            //End of Debugging code

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
            
            if (InputHandler.KeyReleased(Keys.Space))
            {
                camera.ToggleCameraMode();
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

            if (!menuOpen)
            {
                if (motion != Vector2.Zero)
                {
                    int x = ((int)position.X + (int)motion.X) / 16;
                    int y = ((int)position.Y + (int)motion.Y) / 16;
                    bool blocked = false;

                    if (map.GetBlocked(x, y))
                    {
                        blocked = true;
                    }

                    foreach (Entity entity in entities.Entities)
                    {
                        if ((position + motion) == entity.Position && entity.Block)
                        {
                            blocked = true;
                            DealDamage(entity);
                            break;
                        }
                    }

                    if (blocked == false)
                    {
                        position += motion;
                        hasActed = true;
                    }

                    camera.LockToPlayer(this);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.DrawString(spriteFont, symbol, position, color);
        }

        #endregion

        #region Combat Method Region

        private void DealDamage(Entity target)
        {
            target.TakeDamage(damage);
        }
        
        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            CheckHealth();
        }

        private void CheckHealth()
        {
            if (currentHealth <= 0)
            {
                color = Color.Red;
                block = false;
            }
            else if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }

        #endregion

        #region Inventory Region



        #endregion
    }
}
