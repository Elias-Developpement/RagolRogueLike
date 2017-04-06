using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using RagolRogueLike.PlayerClasses;
using RagolRogueLike.Controls;

namespace RagolRogueLike.GameUI
{
    public class GuiManager
    {
        #region Field Region

        Game1 gameRef;

        Player player;

        SpriteFont guiFont;

        Viewport sideViewport;
        Viewport messageViewport;

        Texture2D RectTexture;

        #endregion
        
        #region Property Region
        
        #endregion
        
        #region Constructor Region
        
        public GuiManager(Player player, Viewport sideViewport, Viewport messageViewport, Game game)
        {
            gameRef = (Game1)game;

            this.player = player;
            LoadFont();

            RectTexture = DrawRectangle();

            this.sideViewport = sideViewport;
            this.messageViewport = messageViewport;
        }

        #endregion
        
        #region Method Region

        private void LoadFont()
        {
            ContentManager Content = gameRef.Content;
            guiFont = Content.Load<SpriteFont>(@"Fonts\GuiFont");
        }

        public void DrawSideView(SpriteBatch spriteBatch)
        {
            gameRef.GraphicsDevice.Viewport = sideViewport;

            //Bound for the Side viewport to look nicer, probs will change this.
            DrawBorder(spriteBatch);


            DrawHealth(spriteBatch);
        }

        public void DrawMessageView(SpriteBatch spriteBatch)
        {
            gameRef.GraphicsDevice.Viewport = messageViewport;

            //Bound for the message Viewport, probs will change as well.
            DrawMessageBorder(spriteBatch);
            
            spriteBatch.DrawString(guiFont, "test", new Vector2(5, 5), Color.White);
        }

        //Used to create a white rectangle that can be colored when drawn.
        private Texture2D DrawRectangle()
        {
            var rect = new Texture2D(gameRef.GraphicsDevice, 1, 1);
            rect.SetData(new[] { Color.White });
            return rect;
        }

        #endregion

        #region Message Region

        private void DrawMessageBorder(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(RectTexture, new Rectangle(0, 0, 768, 96), Color.White);
            spriteBatch.Draw(RectTexture, new Rectangle(3, 3, 765, 90), Color.Black);
        }

        #endregion

        #region Side Region

        private void DrawHealth(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(guiFont, "Health: " + player.CurrentHealth.ToString() + " / " + player.MaxHealth.ToString(), new Vector2(10, 10), Color.Red);
        }

        private void DrawBorder(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(RectTexture, new Rectangle(0, 0, 256, 768), Color.White);
            spriteBatch.Draw(RectTexture, new Rectangle(3, 3, 250, 762), Color.Black);
            spriteBatch.Draw(RectTexture, new Rectangle(6, 6, 244, 756), Color.White);
            spriteBatch.Draw(RectTexture, new Rectangle(9, 9, 238, 750), Color.Black);
        }

        #endregion
    }
}
