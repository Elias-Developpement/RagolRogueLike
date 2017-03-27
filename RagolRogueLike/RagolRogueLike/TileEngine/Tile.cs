using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RagolRogueLike.TileEngine
{
    public class Tile
    {
        #region Fields and Properties

        int tileIndex;
        int tileset;

        bool block;

        Vector2 position;
        Rectangle positionRec;

        //An offset to make collisions look better
        int collisionRadius = 8;

        public int TileIndex
        {
            get { return tileIndex; }
        }

        public int Tileset
        {
            get { return tileset; }
        }

        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                setPositionRec();
            }
        }

        public Rectangle PositionRectangle
        {
            get { return positionRec; }
            set { positionRec = value; }
        }

        public bool Block
        {
            get { return block; }
        }

        #endregion

        #region Constructor Region

        public Tile(int tileindex, int tileSet, bool block)
        {
            tileIndex = tileindex;
            tileset = tileSet;
            this.block = block;
        }

        #endregion

        #region Method Region

        private void setPositionRec()
        {
            positionRec = new Rectangle((int)position.X * Engine.TileWidth + collisionRadius, (int)position.Y * Engine.TileHeight, Engine.TileWidth - collisionRadius, Engine.TileHeight - 2 * collisionRadius);
        }

        #endregion
    }
}
