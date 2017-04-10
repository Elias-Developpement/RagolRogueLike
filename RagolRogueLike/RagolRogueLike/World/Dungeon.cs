using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using RagolRogueLike.MapGenerator;
using RagolRogueLike.TileEngine;
using RagolRogueLike.PlayerClasses;

namespace RagolRogueLike.World
{
    public class Dungeon
    {
        #region Field Region

        Player player;

        int floor = 0;
        List<Map> dungeon;
        BasicDungeon level;

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

            //Add the first level of the dungeon to the game.
            Map firstLevel = new Map(100, 100, tileFont);
            dungeon.Add(firstLevel);

            //Initialize the dungeon generator
            //Using the basic one that creates a pretty shitty dungeon.
            level = new BasicDungeon(dungeon[floor].Tiles, player);
            dungeon[floor].Tiles = level.CreateBasicDungeon();

        }

        #endregion
        
        #region Method Region

        public void Update(GameTime gameTime)
        {
            player.Update(gameTime, dungeon[floor]);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            dungeon[floor].Draw(spriteBatch, player.Camera);
        }
        
        public void ChangeLevel()
        {

        }

        #endregion
    }
}
