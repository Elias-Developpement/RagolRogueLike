using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RagolRogueLike.TileEngine
{
    public class TileSet
    {
        #region Fields and Properties

        Texture2D image;

        int tileWidthInPixels;
        int tileHeightInPixels;
        int tilesWide;
        int tilesHigh;

        Rectangle[] sourceRectangles;

        public Texture2D Texture
        {
            get { return image; }
        }

        public Rectangle[] SourceRectangles
        {
            get { return (Rectangle[])sourceRectangles.Clone(); }
        }

        #endregion

        #region Constructor Region

        public TileSet(Texture2D Image, int TilesWide, int TilesHigh, int TileWidth, int TileHeight)
        {
            image = Image;

            tileHeightInPixels = TileHeight;
            tileWidthInPixels = TileWidth;

            tilesWide = TilesWide;
            tilesHigh = TilesHigh;

            int tiles = TilesHigh * TilesWide;

            sourceRectangles = new Rectangle[tiles];

            int tile = 0;
            
            for (int y = 0; y < tilesHigh; y++)
            {
                for (int x = 0; x < tilesWide; x++)
                {
                    sourceRectangles[tile] = new Rectangle(x * tileWidthInPixels, y * tileHeightInPixels, TileWidth, TileHeight);

                    tile++;
                }
            } 

        }

        #endregion

        #region Method Region

        #endregion
    }
}
