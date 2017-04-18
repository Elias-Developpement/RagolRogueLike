using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using RagolRogueLike.TileEngine;
using RagolRogueLike.PathFinding;
using RagolRogueLike.PlayerClasses;

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
        bool canAct = true;

        //Gives the exact position of the entity in the manager list.
        int managerID;
        EntityManager manager;

        Vector2 position;
        SpriteFont spriteFont;


        List<Vector2> path;

        int maxHealth;
        int currentHealth;
        int damage;

        #endregion

        #region Property Region

        internal EntityManager Manager
        {
            set { manager = value; }
        }

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
            damage = 3;
        }

        #endregion

        #region Method Region

        public void Update(GameTime gameTime, Map map, Player player)
        {
            if (canAct)
            {
                //TODO: Make collision detection better as well as pathfinding.
                path = manager.Pathfinder.FindPath(position.ToPoint(), player.Position.ToPoint());
                Vector2 playerPosition = player.Position;

                if (path.Count != 0)
                {

                    MoveEntity(path[0], player);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(spriteFont, symbol, position, color);
        }

        private void MoveEntity(Vector2 node, Player player)
        {
            bool notBlocked = true;

            Vector2 motion = node - position;
            //Entity is properly moving towards the player as of right now.
            //And the entity is not moving if it is dead.

            //Collision detection
            //Check the entities.
            foreach (Entity entity in manager.Entities)
            {
                if (entity.position == (position + motion) && entity.Block)
                {
                    notBlocked = false;
                    break;
                }
            }

            //Next check if the player is in the position.
            if (player.Position == (position + motion))
            {
                notBlocked = false;
                DealDamagePlayer(player);
            }


            if (notBlocked)
            {
                position += motion;
            }

        }

        #endregion

        #region Combat Region

        private void CheckHealth()
        {
            if (currentHealth <= 0)
            {
                color = Color.Red;
                block = false;
                canAct = false;
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

        private void DealDamagePlayer(Player player)
        {
            player.TakeDamage(damage);
        }

        private void DealDamageEntity(Entity entity)
        {

        }

        #endregion
    }
}
