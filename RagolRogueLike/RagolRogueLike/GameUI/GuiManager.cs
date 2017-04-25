using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using RagolRogueLike.PlayerClasses;
using RagolRogueLike.Controls;
using RagolRogueLike.GameObject;

namespace RagolRogueLike.GameUI
{
    public class GuiManager
    {
        #region Field Region

        Game1 gameRef;

        Player player;

        SpriteFont guiFont;
        SpriteFont menuFont;

        //Dimensions of the viewport
        //256x768
        Viewport sideViewport;
        //768x96
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
            menuFont = Content.Load<SpriteFont>(@"Fonts\MenuFont");
        }

        //TODO: add in items that are being stood on display on the side view.
        //could just make this a part of messages.
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

        public void DrawInventory(SpriteBatch spriteBatch)
        {   
            //Bounds for menus that open when a key is pressed.
            //Kind of looks too big right now, may end up changing this.
            DrawMenuBorder(spriteBatch);

            spriteBatch.DrawString(menuFont, "Inventory: ", new Vector2(48, 38), Color.White);

            int x = 64;
            int y = 64;

            foreach (Item item in player.Inventory.Items)
            {
                spriteBatch.DrawString(menuFont, item.Name, new Vector2(x, y), Color.White);
                
                y += 24;
            }

            //TODO: Add in the equipped area of the inventory.
            spriteBatch.DrawString(menuFont, "Equipped: ", new Vector2(520, 38), Color.White);

        }

        //Used to create a white rectangle that can be colored when drawn.
        private Texture2D DrawRectangle()
        {
            var rect = new Texture2D(gameRef.GraphicsDevice, 1, 1);
            rect.SetData(new[] { Color.White });
            return rect;
        }

        #endregion

        #region Menu GUI Region

        private void DrawMenuBorder(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(RectTexture, new Rectangle(40, 30, 944, 708), Color.White);
            spriteBatch.Draw(RectTexture, new Rectangle(43, 33, 469, 702), Color.Black);
            spriteBatch.Draw(RectTexture, new Rectangle(515, 33, 466, 702), Color.Black);
        }

        #endregion

        #region Message GUI Region

        private void DrawMessageBorder(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(RectTexture, new Rectangle(0, 0, 768, 96), Color.White);
            spriteBatch.Draw(RectTexture, new Rectangle(3, 3, 765, 90), Color.Black);
        }

        #endregion

        #region Side GUI Region

        private void DrawHealth(SpriteBatch spriteBatch)
        {
            //Create a box that becomes smaller as health decreases.
            //Could pose a problem later since it currently only decrease with int values and not float
            //therefore it only decrease every couple of hit points but that shouldn't really be a problem for the most part.
            //Since could always make HP work with values that make the percents work out nicely such as using multiples of 10s.
            DrawDynamicBar(spriteBatch, 20, 20, 216, 20, player.HealthPercent, Color.Red);

            spriteBatch.DrawString(guiFont, "Health: " + player.CurrentHealth.ToString() + " / " + player.MaxHealth.ToString(), new Vector2(70, 23), Color.White);
        }

        private void DrawBorder(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(RectTexture, new Rectangle(0, 0, 256, 768), Color.White);
            spriteBatch.Draw(RectTexture, new Rectangle(3, 3, 250, 762), Color.Black);
            spriteBatch.Draw(RectTexture, new Rectangle(6, 6, 244, 756), Color.White);
            spriteBatch.Draw(RectTexture, new Rectangle(9, 9, 238, 750), Color.Black);
        }

        
        #endregion

        #region Drawing Methods Region

        //Used to draw stuff such as health bars, experience bars, etc..
        //Takes in the starting x, y position and the width and height of the bars
        // as well as the tracked stat and color of the bar.
        private void DrawDynamicBar(SpriteBatch spriteBatch, int x, int y, int w, int h, int trackedStat, Color color)
        {
            spriteBatch.Draw(RectTexture, new Rectangle(x, y, w, h), Color.White);
            spriteBatch.Draw(RectTexture, new Rectangle(x + 2, y + 2, w - 4, h - 4), Color.Black);
            spriteBatch.Draw(RectTexture, new Rectangle(x + 2, y + 2, (w - 4) * trackedStat / 100, h - 4), color);
        }

        #endregion
    }
}
