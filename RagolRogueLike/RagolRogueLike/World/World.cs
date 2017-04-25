using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using RagolRogueLike.TileEngine;
using RagolRogueLike.PlayerClasses;
using RagolRogueLike.Entities;

namespace RagolRogueLike.World
{
    public class World
    {
        #region Field Region

        Player player;

        SpriteFont tileFont;

        const int mapHeight = 10;
        const int mapWidth = 10;
        Vector2 currentChunk;
        Map[,] Chunks;


        #endregion
        
        #region Property Region
        
        #endregion
        
        #region Constructor Region
        
        public World(SpriteFont tileFont, Player player)
        {
            this.tileFont = tileFont;
            this.player = player;

            Chunks = new Map[mapWidth, mapHeight];

            currentChunk = new Vector2(10, 10);
        }

        #endregion
        
        #region Method Region

        public void Draw(SpriteBatch spriteBatch)
        {
            //Loop through some of the close maps and draw them.
            //TODO: Find out how to draw the position of the chunks relative to the maps around them.
            for (int x = (int)currentChunk.X - 1; x < (int)currentChunk.X + 1; x++)
            {
                for (int y = (int)currentChunk.Y - 1; y < (int)currentChunk.Y + 1; y++)
                {
                    //Chunks[x, y].Draw(spriteBatch, player.Camera, player);
                }
            }
        }
        
        #endregion
    }
}
