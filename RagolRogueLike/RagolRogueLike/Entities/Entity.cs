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
    public struct CombatStats
    {
        public int MaxHealth;
        public int currentHealth;

        public int damage;
        public int defense;
    }

    public class Entity
    {

        #region Field Region

        string symbol;
        Color color;
        bool block;

        //Gives the exact position of the entity in the manager list.
        int managerID;

        Vector2 position;
        SpriteFont spriteFont;

        int maxHealth;
        int currentHealth;

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

            maxHealth = 10;
            currentHealth = maxHealth;
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

        #region Combat Region

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

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            CheckHealth();
        }

        #endregion
    }
}
