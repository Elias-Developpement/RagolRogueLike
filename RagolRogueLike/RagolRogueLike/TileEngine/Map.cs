using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace RagolRogueLike.TileEngine
{
    public class Map
    {
        #region Field Region

        List<TileSet> tilesets;
        List<MapLayer> mapLayers;
        static List<Tile> blockedTiles;

        static int mapWidth;
        static int mapHeight;
    
        public static int MapWidthInPixels
        {
            get { return mapWidth * Engine.TileWidth; }
        }

        public static int MapHeightInPixels
        {
            get { return mapHeight * Engine.TileHeight; }
        }

        public static List<Tile> BlockedTiles
        {
            get { return blockedTiles; }
        }

        #endregion

        #region Property Region

        #endregion

        #region Constructor Region

        public Map(TileSet tileset, MapLayer layer)
        {
            tilesets = new List<TileSet>();
            tilesets.Add(tileset);

            mapLayers = new List<MapLayer>();
            mapLayers.Add(layer);

            mapWidth = mapLayers[0].Width;
            mapHeight = mapLayers[0].Height;

            blockedTiles = new List<Tile>();

            fillBlocked();
        }

        public Map(List<TileSet> tilesets, List<MapLayer> mapLayers)
        {
            this.tilesets = tilesets;
            this.mapLayers = mapLayers;

            mapWidth = mapLayers[0].Width;
            mapHeight = mapLayers[0].Height;

            blockedTiles = new List<Tile>();

            fillBlocked();
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
            Tile tile;

            foreach (MapLayer layer in mapLayers)
            {
                for (int y = min.Y; y < max.Y; y++)
                {
                    destination.Y = y * Engine.TileHeight;

                    for (int x = min.X; x < max.X; x++)
                    {
                        tile = layer.GetTile(x, y);

                        destination.X = x * Engine.TileWidth;

                        //layer.GetTile(x, y).PositionRectangle = destination;

                        spriteBatch.Draw(tilesets[tile.Tileset].Texture, destination, tilesets[tile.Tileset].SourceRectangles[tile.TileIndex], Color.White);
                    }
                }
            }
            
        }

        private void fillBlocked()
        {
            Tile tile;

            foreach (MapLayer layer in mapLayers)
            {
                for (int y = 0; y < layer.Height; y++)
                {
                    for (int x = 0; x < layer.Width; x++)
                    {
                        tile = layer.GetTile(x, y);
                        if (tile.Block)
                        {
                            blockedTiles.Add(tile);
                        }
                    }
                }
            }
        }

        #endregion

    }
}
