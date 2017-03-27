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
        SpriteFont EntityFont;

        Game1 gameRef;

        Tile[,] testMap;

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
            player = new Player("@", Color.White, EntityFont, new Vector2(16, 16));
            CreateTestMap();

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

            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    GameRef.spriteBatch.DrawString(EntityFont, testMap[x, y].Symbol, testMap[x, y].Position, testMap[x, y].Color);
                }
            }

            player.Draw(GameRef.spriteBatch, gameTime);
            base.Draw(gameTime);

            GameRef.spriteBatch.End();
        }

        private void CreateTestMap()
        {
            //TODO: Start on collision detection for the player.
            testMap = new Tile[50, 50];

            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    if (x == 0 || x == 49 || y == 0 || y == 49)
                    {
                        testMap[x, y] = new Tile("#", true, Color.White, new Vector2(x * 16, y * 16));
                    }
                    else
                    {
                        testMap[x, y] = new Tile(".", false, Color.White, new Vector2(x * 16, y * 16));
                    }
                }
            }
        }

        #endregion
    }
}
