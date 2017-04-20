using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using RagolRogueLike.Controls;
using RagolRogueLike.GameStates;

namespace RagolRogueLike.GameScreens
{
    public class StartMenuScreen : BaseGameState
    {
        #region Field region

        LinkLabel startGame;
        LinkLabel loadGame;
        LinkLabel exitGame;

        float maxItemWidth = 0f;

        #endregion

        #region Property Region

        #endregion

        #region Constructor Region

        public StartMenuScreen(Game game, GameStateManager manager) : base(game, manager)
        {
        }

        #endregion

        #region XNA Method Region

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            ContentManager Content = Game.Content;

            startGame = new LinkLabel();
            startGame.Text = "The story begins";
            startGame.Size = startGame.SpriteFont.MeasureString(startGame.Text);
            startGame.Selected += new EventHandler(menuItem_Selected);

            ControlManager.Add(startGame);

            loadGame = new LinkLabel();
            loadGame.Text = "The story continues";
            loadGame.Size = loadGame.SpriteFont.MeasureString(loadGame.Text);
            loadGame.Selected += menuItem_Selected;

            ControlManager.Add(loadGame);

            exitGame = new LinkLabel();
            exitGame.Text = "The story ends";
            exitGame.Size = exitGame.SpriteFont.MeasureString(exitGame.Text);
            exitGame.Selected += menuItem_Selected;

            ControlManager.Add(exitGame);

            ControlManager.NextControl();

            //Set the position for the link labels above
            Vector2 position = new Vector2(350, 500);
            foreach (Control c in ControlManager)
            {
                if (c is LinkLabel)
                {
                    if (c.Size.X > maxItemWidth)
                        maxItemWidth = c.Size.X;
                    c.Position = position;
                    position.Y += c.Size.Y + 5f;
                }
            }

        }

        public override void Update(GameTime gameTime)
        {
            ControlManager.Update(gameTime, playerIndexInControl);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GameRef.spriteBatch.Begin();

            base.Draw(gameTime);

            ControlManager.Draw(GameRef.spriteBatch);

            GameRef.spriteBatch.End();
        }

        #endregion

        #region Game State Method Region

        private void menuItem_Selected(object sender, EventArgs e)
        {
            if (sender == startGame)
            {
                StateManager.PushState(GameRef.characterCreation);
            }

            if (sender == loadGame)
            {
                StateManager.PushState(GameRef.gamePlayScreen);
            }

            if (sender == exitGame)
            {
                GameRef.Exit();
            }

        }

        #endregion
    }
}
