using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using RagolRogueLike.GameStates;
using RagolRogueLike.Controls;
using RagolRogueLike.TileEngine;
using RagolRogueLike.PlayerClasses;
using RagolRogueLike.Entities;
using RagolRogueLike.GameUI;
using RagolRogueLike.World;

namespace RagolRogueLike.GameScreens
{
    public class GamePlayScreen : BaseGameState
    {
        #region Field Region

        GuiManager gui;

        Engine engine;
        Dungeon dungeon;
        Player player;

        SpriteFont EntityFont;

        Game1 gameRef;

        Viewport defaultViewport;
        Viewport mapViewport;
        Viewport statsViewport;
        Viewport messageViewport;

        bool InventoryOpen = false;
        

        #endregion

        #region Property Region

        #endregion

        #region Constructor Region

        public GamePlayScreen(Game game, GameStateManager manager) : base(game, manager)
        {
            gameRef = (Game1)game;

            engine = new Engine(16, 16);

            
        }

        #endregion

        #region Method Region

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            ContentManager Content = Game.Content;
            EntityFont = Content.Load<SpriteFont>(@"Fonts\EntityFont");
            player = new Player("@", Color.White, EntityFont, new Vector2(16, 16), GameRef.screenRectangle);
            dungeon = new Dungeon(EntityFont, player);

            defaultViewport = GameRef.GraphicsDevice.Viewport;

            mapViewport = GameRef.GraphicsDevice.Viewport;
            mapViewport.Width = 3 * mapViewport.Width / 4;
            mapViewport.Height = 7 * mapViewport.Height / 8;

            statsViewport = GameRef.GraphicsDevice.Viewport;
            statsViewport.Width = statsViewport.Width / 4;
            statsViewport.X = mapViewport.Width;

            messageViewport = GameRef.GraphicsDevice.Viewport;
            messageViewport.Height = messageViewport.Height / 8;
            messageViewport.Width = 3 * messageViewport.Width / 4;
            messageViewport.Y = mapViewport.Height;

            gui = new GuiManager(player, statsViewport, messageViewport, GameRef);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            dungeon.Update(gameTime);
            base.Update(gameTime);

            if (InputHandler.KeyReleased(Keys.I))
            {
                InventoryOpen = !InventoryOpen;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            //Draw all of the play area stuff in this area.
            GameRef.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, player.Camera.Transformation);

            GraphicsDevice.Viewport = mapViewport;
            dungeon.Draw(GameRef.spriteBatch);
            player.Draw(GameRef.spriteBatch, gameTime);
            base.Draw(gameTime);

            GameRef.spriteBatch.End();

            //Draw all of the messages in this area.
            GameRef.spriteBatch.Begin();

            gui.DrawSideView(GameRef.spriteBatch);
            base.Draw(gameTime);
            
            GameRef.spriteBatch.End();

            //Draw all of the stats and other stuff that goes in the side bar in this area.
            GameRef.spriteBatch.Begin();

            gui.DrawMessageView(GameRef.spriteBatch);
            base.Draw(gameTime);

            GameRef.spriteBatch.End();

            //Area to draw on the entire screen for things such as character stats and inventory.
            GameRef.spriteBatch.Begin();

            GraphicsDevice.Viewport = defaultViewport;

            if (InventoryOpen)
            {
                gui.DrawInventory(GameRef.spriteBatch);
            }

            base.Draw(gameTime);

            GameRef.spriteBatch.End();


        }

        #endregion
    }
}
