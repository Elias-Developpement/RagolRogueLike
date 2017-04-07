using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RagolRogueLike.Entities;
using RagolRogueLike.PlayerClasses;
using RagolRogueLike.MapGenerator;



namespace RagolRogueLike.TileEngine
{
    public class Map
    {
        #region Field Region

        static int mapWidth;
        static int mapHeight;

        Tile[,] testMap;
        Player player;

        SpriteFont tileFont;

        BasicDungeon testdungeon;

        #endregion

        #region Property Region

        public static int MapWidthInPixels
        {
            get { return Engine.TileWidth * mapWidth; }
        }

        public static int MapHeightInPixels
        {
            get { return Engine.TileHeight * mapHeight; }
        }

        #endregion

        #region Constructor Region

        public Map(int mWidth, int mHeight, SpriteFont tileFont, Player player)
        {
            mapWidth = mWidth;
            mapHeight = mHeight;

            testMap = new Tile[mapWidth, mapHeight];
            this.player = player;

            this.tileFont = tileFont;

            testdungeon = new BasicDungeon(testMap, player);
            fillMap();
            testMap = testdungeon.CreateBasicDungeon();
            //fillBlocked();
        }

        //Used to create the next level of the map.
        //Still needs to be implemented by adding stairs and what not.
        //Also needs to find out how to save and store maps so that you can come back up the stairs
        //to go back to an old map.
        public Map(SpriteFont tileFont, Player player)
        {
            this.tileFont = tileFont;

            testMap = new Tile[mapWidth, mapHeight];
            this.player = player;

            fillMap();
        }


        #endregion

        #region Method Region

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            Point cameraPoint = Engine.VectorToCell(camera.Position * (1 / camera.Zoom));
            Point viewPoint = Engine.VectorToCell(new Vector2((camera.Position.X + camera.ViewportRectangle.Width) * (1 / camera.Zoom), (camera.Position.Y + camera.ViewportRectangle.Height) * (1 / camera.Zoom)));

            Point min = new Point();
            Point max = new Point();

            min.X = Math.Max(0, cameraPoint.X - 1);
            min.Y = Math.Max(0, cameraPoint.Y - 1);

            max.X = Math.Min(viewPoint.X + 1, mapWidth);
            max.Y = Math.Min(viewPoint.Y + 1, mapHeight);


            Rectangle destination = new Rectangle(0, 0, Engine.TileWidth, Engine.TileHeight);

            //TODO: Don't draw tiles that are being occupied.
            for (int y = min.Y; y < max.Y; y++)
            {
                destination.Y = y * Engine.TileHeight;

                for (int x = min.X; x < max.X; x++)
                {
                    destination.X = x * Engine.TileWidth;

                    //Only draw tiles that are visible.
                    if (testMap[x, y] != null)
                    {
                        if (testMap[x, y].IsVisible)
                        {
                            spriteBatch.DrawString(tileFont, testMap[x, y].Symbol, testMap[x, y].Position, testMap[x, y].Color);
                        }
                        //Or draw tiles that have been discovered but are no longer visible, but draw them gray instead.
                        else if (testMap[x, y].IsDiscovered)
                        {
                            spriteBatch.DrawString(tileFont, testMap[x, y].Symbol, testMap[x, y].Position, Color.Gray);
                        }
                    }
                }
            }

        }

        private void fillMap()
        {
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    testMap[x, y] = new Tile("#", true, Color.White, new Vector2(x * 16, y * 16));
                }
            }
        }
        
        public bool GetBlocked(int x, int y)
        {
            if (testMap[x, y].Block)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ChangeVisible(int x, int y)
        {
            testMap[x, y].IsVisible = !testMap[x, y].IsVisible;
        }

        #endregion

    }
}
