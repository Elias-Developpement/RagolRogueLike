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

namespace RagolRogueLike.GameScreens
{
    public class EndGameScreen : BaseGameState
    {
        #region Field Region

        Game1 gameRef;

        LinkLabel restartGame;
        LinkLabel mainMenu;
        LinkLabel endGame;

        SpriteFont endGameFont;

        float maxItemWidth = 0f;

        #endregion
        
        #region Property Region
        
        #endregion
        
        #region Constructor Region
        
        public EndGameScreen(Game game, GameStateManager manager) : base(game, manager)
        {
            gameRef = (Game1)game;
        }

        #endregion

        #region Method Region

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            ContentManager Content = Game.Content;

            endGameFont = Content.Load<SpriteFont>(@"Fonts\MenuFont");

            restartGame = new LinkLabel();
            restartGame.Text = "Restart Game";
            restartGame.Size = restartGame.SpriteFont.MeasureString(restartGame.Text);
            restartGame.Selected += new EventHandler(menuItem_Selected);

            ControlManager.Add(restartGame);

            mainMenu = new LinkLabel();
            mainMenu.Text = "Return to Main Menu";
            mainMenu.Size = mainMenu.SpriteFont.MeasureString(mainMenu.Text);
            mainMenu.Selected += menuItem_Selected;

            ControlManager.Add(mainMenu);

            endGame = new LinkLabel();
            endGame.Text = "Exit";
            endGame.Size = endGame.SpriteFont.MeasureString(endGame.Text);
            endGame.Selected += menuItem_Selected;

            ControlManager.Add(endGame);

            ControlManager.NextControl();

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

        private void DrawEndGame(SpriteBatch spriteBatch)
        {

        }

        private void menuItem_Selected(object sender, EventArgs e)
        {
            if (sender == restartGame)
            {
                StateManager.ChangeState(GameRef.characterCreation);

            }

            if (sender == mainMenu)
            {
                StateManager.ChangeState(GameRef.startMenuScreen);
            }

            if (sender == endGame)
            {
                GameRef.Exit();
            }
        }

        #endregion
    }
}
