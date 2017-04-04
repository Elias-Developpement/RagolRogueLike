using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using RagolRogueLike.GameStates;
using RagolRogueLike.Controls;
using RagolRogueLike.TileEngine;
using RagolRogueLike.PlayerClasses;
using RagolRogueLike.Entities;

namespace RagolRogueLike.GameScreens
{
    public class GamePlayScreen : BaseGameState
    {
        #region Field Region

        Engine engine;
        Map map;
        Player player;
        Entity testEntity;

        SpriteFont EntityFont;

        Game1 gameRef;

        Viewport mapViewport;
        Viewport statsViewport;
        Viewport messageViewport;

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
            testEntity = new Entity("T", Color.Green, EntityFont, new Vector2(32, 32));
            map = new Map(100, 100, EntityFont, player);

            mapViewport = GameRef.GraphicsDevice.Viewport;
            mapViewport.Width = 3 * mapViewport.Width / 4;
            mapViewport.Height = 7 * mapViewport.Height / 8;

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            player.Update(gameTime, map, testEntity);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GameRef.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, player.Camera.Transformation);

            GraphicsDevice.Viewport = mapViewport;
            map.Draw(GameRef.spriteBatch, player.Camera);
            testEntity.Draw(GameRef.spriteBatch);
            player.Draw(GameRef.spriteBatch, gameTime);
            base.Draw(gameTime);

            GameRef.spriteBatch.End();
        }

        #endregion
    }
}
