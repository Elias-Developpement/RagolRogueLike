using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RagolRogueLike.Entities;
using RagolRogueLike.PlayerClasses;
using RagolRogueLike.TileEngine;

using Microsoft.Xna.Framework;

namespace RagolRogueLike.PathFinding
{
    public class DiamondFOV
    {

        #region Field Region
        
        int lightradius;

        #endregion
        
        #region Property Region
        
        #endregion
        
        #region Constructor Region
        
        public DiamondFOV(int lightradius)
        {
            this.lightradius = lightradius;
        }

        #endregion
        
        #region Method Region

        //Use this method in the update method of which ever entity is using this FOV.
        public void UpdateFOV(Map map, Vector2 position)
        {
            
        }
        
        #endregion
    }
}
