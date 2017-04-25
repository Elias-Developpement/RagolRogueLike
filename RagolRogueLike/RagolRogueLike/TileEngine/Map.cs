using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RagolRogueLike.PlayerClasses;
using RagolRogueLike.Entities;
using RagolRogueLike.GameObject;


namespace RagolRogueLike.TileEngine
{
    public class Map
    {
        #region Field Region

        static int mapWidth;
        static int mapHeight;

        int stairsDownX;
        int stairsDownY;
        int stairsUpX;
        int stairsUpY;

        int chunkLocation;

        Tile[,] tiles;

        SpriteFont tileFont;

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

        public Tile[,] Tiles
        {
            get { return tiles; }
            set { tiles = value; }
        }

        public int StairsDownX
        {
            get { return stairsDownX; }
        }

        public int StairsDownY
        {
            get { return stairsDownY; }
        }

        public int StairsUpX
        {
            get { return stairsUpX; }
        }

        public int StairsUpY
        {
            get { return stairsUpY; }
        }

        //Use this if an overworld is ever added in. But currently don't bother with it.
        public int ChunkLocation
        {
            get { return chunkLocation; }
            set { chunkLocation = value; }
        }

        #endregion

        #region Constructor Region

        public Map(int mWidth, int mHeight, SpriteFont tileFont)
        {
            mapWidth = mWidth;
            mapHeight = mHeight;

            tiles = new Tile[mapWidth, mapHeight];

            this.tileFont = tileFont;

            chunkLocation = 1;

            fillMap();
            //fillBlocked();
        }

        //Used to create the next level of the map.
        //Still needs to be implemented by adding stairs and what not.
        //Also needs to find out how to save and store maps so that you can come back up the stairs
        //to go back to an old map.
        public Map(SpriteFont tileFont)
        {
            this.tileFont = tileFont;

            tiles = new Tile[mapWidth, mapHeight];

            chunkLocation = 1;

            fillMap();
        }


        #endregion

        #region Method Region

        //TODO: Add background color to tiles.
        public void Draw(SpriteBatch spriteBatch, Camera camera, Player player, EntityManager entities, ItemManager items)
        {
            Point cameraPoint = Engine.VectorToCell(camera.Position * (1 / camera.Zoom));
            Point viewPoint = Engine.VectorToCell(new Vector2((camera.Position.X + camera.ViewportRectangle.Width) * (1 / camera.Zoom), (camera.Position.Y + camera.ViewportRectangle.Height) * (1 / camera.Zoom)));

            Point min = new Point();
            Point max = new Point();

            min.X = Math.Max(0, cameraPoint.X - 1);
            min.Y = Math.Max(0, cameraPoint.Y - 1);

            max.X = Math.Min(viewPoint.X + 1, mapWidth);
            max.Y = Math.Min(viewPoint.Y + 1, mapHeight);

            for (int y = min.Y; y < max.Y; y++)
            {
                for (int x = min.X; x < max.X; x++)
                {
                    //Only draw tiles that are visible.
                    if (tiles[x, y] != null)
                    {
                        bool spaceOccupied = false;

                        foreach (Entity entity in entities.Entities)
                        {
                            if (entity.Position == tiles[x, y].Position)
                            {
                                spaceOccupied = true;
                                break;
                            }
                        }

                        foreach (Entity entity in entities.DeadEntities)
                        {
                            if (spaceOccupied)
                            {
                                break;
                            }
                            if (entity.Position == tiles[x, y].Position)
                            {
                                spaceOccupied = true;
                                break;
                            }
                        }

                        foreach (Item item in items.Items)
                        {
                            if (spaceOccupied)
                            {
                                break;
                            }
                            if (item.Position == tiles[x, y].Position)
                            {
                                spaceOccupied = true;
                                break;
                            }
                        }

                        if (player.Position == tiles[x, y].Position)
                        {
                            spaceOccupied = true;
                        }

                        if (!spaceOccupied)
                        {
                            if (tiles[x, y].IsVisible)
                            {
                                spriteBatch.DrawString(tileFont, tiles[x, y].Symbol, tiles[x, y].Position, tiles[x, y].Color);
                            }
                            //Or draw tiles that have been discovered but are no longer visible, but draw them gray instead.
                            else if (tiles[x, y].IsDiscovered)
                            {
                                spriteBatch.DrawString(tileFont, tiles[x, y].Symbol, tiles[x, y].Position, Color.Gray);
                            }
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
                    tiles[x, y] = new Tile("#", true, Color.White, Color.DarkGray, new Vector2(x * 16, y * 16));
                }
            }
        }
        
        public bool GetBlocked(int x, int y)
        {
            if (tiles[x, y].Block)
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
            tiles[x, y].IsVisible = !tiles[x, y].IsVisible;
        }

        public void FindStairsDown()
        {
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    if (tiles[x, y].Symbol == ">")
                    {
                        stairsDownX = x;
                        stairsDownY = y;
                    }
                }
            }
        }

        public void FindStairsUp()
        {
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    if (tiles[x, y].Symbol == "<")
                    {
                        stairsUpX = x;
                        stairsUpY = y;
                    }
                }
            }
        }

        #endregion

    }
}
