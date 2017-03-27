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

namespace RagolRogueLike.GameScreens
{
    public class GamePlayScreen : BaseGameState
    {
        #region Field Region

        Engine engine;
        Player player;

        Game1 gameRef;

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
            SpriteFont EntityFont = Content.Load<SpriteFont>(@"Fonts\EntityFont");
            player = new Player("@", Color.White, EntityFont, new Vector2(0, 0));

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            player.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //Put this in begin after camera is added to the game
            //SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null ,null, null, player.Camera.Transformation
            GameRef.spriteBatch.Begin();

            player.Draw(GameRef.spriteBatch, gameTime);
            base.Draw(gameTime);

            GameRef.spriteBatch.End();
        }

        #endregion
    }
}
