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

        string symbol;
        Color color;
        Color background;
        bool block;
        bool visible;
        bool discovered;

        Vector2 position;
        Rectangle positionRec;

        public string Symbol
        {
            get { return symbol; }
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

        public Color Color
        {
            get { return color; }
        }

        public Color Background
        {
            get { return background; }
        }

        public bool IsVisible
        {
            get { return visible; }
            set { visible = value; }
        }

        public bool IsDiscovered
        {
            get { return discovered; }
            set { discovered = value; }
        }

        #endregion

        #region Constructor Region

        public Tile(string symbol, bool block, Color color, Color background, Vector2 position)
        {
            this.symbol = symbol;
            this.block = block;
            this.color = color;
            this.background = background;
            this.position = position;
            visible = true;
            discovered = false;
        }

        #endregion

        #region Method Region

        private void setPositionRec()
        {
            positionRec = new Rectangle((int)position.X * Engine.TileWidth, (int)position.Y * Engine.TileHeight, Engine.TileWidth, Engine.TileHeight);
        }

        #endregion
    }
}
