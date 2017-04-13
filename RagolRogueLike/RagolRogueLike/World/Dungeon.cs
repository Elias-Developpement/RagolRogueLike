using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using RagolRogueLike.MapGenerator;
using RagolRogueLike.TileEngine;
using RagolRogueLike.PlayerClasses;
using RagolRogueLike.Entities;
using RagolRogueLike.GameObject;

namespace RagolRogueLike.World
{
    public class Dungeon
    {
        #region Field Region

        Player player;

        int floor = 0;
        List<Map> dungeon;
        List<EntityManager> entities;
        List<ItemManager> items;

        SpriteFont tileFont;

        #endregion
        
        #region Property Region
        
        #endregion
        
        #region Constructor Region
        
        //Class used to hold the multiple levels of the map and to deal with moving between levels.
        //Will also probably be used to deal with the scaling of the game later on.
        public Dungeon(SpriteFont tileFont, Player player)
        {
            this.player = player;
            this.tileFont = tileFont;

            dungeon = new List<Map>();
            entities = new List<EntityManager>();
            items = new List<ItemManager>();

            //Add the first level of the dungeon to the game.
            Map firstLevel = new Map(50, 50, tileFont);
            dungeon.Add(firstLevel);

            //Initialize the dungeon generator
            //Using the basic one that creates a pretty shitty dungeon.
            BasicDungeon level = new BasicDungeon(dungeon[floor].Tiles, player, tileFont);
            dungeon[floor].Tiles = level.CreateBasicDungeon();
            dungeon[floor].FindStairsUp();
            dungeon[floor].FindStairsDown();

            //Retrieve the entities and items and add it to the floor.
            entities.Add(level.GetEntities());
            items.Add(level.getItems());
        }

        #endregion
        
        #region Method Region

        public void Update(GameTime gameTime)
        {
            player.Update(gameTime, dungeon[floor], entities[floor]);
            ChangeLevel();
            PickUpItem();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            dungeon[floor].Draw(spriteBatch, player.Camera);
            items[floor].Draw(spriteBatch);
            entities[floor].Draw(spriteBatch);
        }
        
        private void ChangeLevel()
        {
            // > is stairs down
            if (InputHandler.KeyReleased(Keys.OemPeriod) && (InputHandler.KeyDown(Keys.RightShift) || InputHandler.KeyDown(Keys.LeftShift)) && dungeon[floor].Tiles[(int)player.Position.X / 16, (int)player.Position.Y / 16].Symbol == ">")
            {
                if (dungeon.Count <= floor + 1)
                {
                    Map newLevel = new Map(tileFont);
                    dungeon.Add(newLevel);
                    BasicDungeon newGen = new BasicDungeon(dungeon[floor + 1].Tiles, player, tileFont);
                    dungeon[floor + 1].Tiles = newGen.CreateBasicDungeon();
                    dungeon[floor + 1].FindStairsDown();
                    dungeon[floor + 1].FindStairsUp();
                    entities.Add(newGen.GetEntities());
                    items.Add(newGen.getItems());
                }
                floor++;
                player.Position = new Vector2(dungeon[floor].StairsUpX * 16, dungeon[floor].StairsUpY * 16);
                player.Camera.LockToPlayer(player);
            }
            // < is stairs up
            else if (InputHandler.KeyReleased(Keys.OemComma) && (InputHandler.KeyDown(Keys.RightShift) || InputHandler.KeyDown(Keys.LeftShift)) && dungeon[floor].Tiles[(int)player.Position.X / 16, (int)player.Position.Y / 16].Symbol == "<")
            {
                if (floor > 0)
                {
                    floor--;

                    player.Position = new Vector2(dungeon[floor].StairsDownX * 16, dungeon[floor].StairsDownY * 16);
                    player.Camera.LockToPlayer(player);
                }
            }
        }

        //TODO: Find a better place to put this code.
        private void PickUpItem()
        {
            if (InputHandler.KeyReleased(Keys.E))
            {
                foreach (Item item in items[floor].Items)
                {
                    if (item.Position == player.Position)
                    {
                        player.Inventory.AddItem(item);
                        items[floor].RemoveItem(item);
                        break;
                    }
                }
            }
        }

        #endregion
    }
}
