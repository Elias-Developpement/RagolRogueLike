using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RagolRogueLike.PlayerClasses;
using RagolRogueLike.Controls;

namespace RagolRogueLike.GameUI
{
    public class GuiManager
    {
        #region Field Region

        Player player;

        SpriteFont guiFont;

        Viewport sideViewport;
        Viewport messageViewport;

        #endregion
        
        #region Property Region
        
        #endregion
        
        #region Constructor Region
        
        public GuiManager(Player player, SpriteFont font, Viewport sideViewport, Viewport messageViewport)
        {
            this.player = player;
            guiFont = font;

            this.sideViewport = sideViewport;
            this.messageViewport = messageViewport;
        }

        #endregion
        
        #region Method Region


        
        #endregion
    }
}
