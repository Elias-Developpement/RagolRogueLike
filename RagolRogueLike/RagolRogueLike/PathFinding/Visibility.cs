using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using RagolRogueLike.TileEngine;

namespace RagolRogueLike.PathFinding
{
    abstract class Visibility
    {
        public abstract void Compute(Vector2 position, Map map);
    }
}
