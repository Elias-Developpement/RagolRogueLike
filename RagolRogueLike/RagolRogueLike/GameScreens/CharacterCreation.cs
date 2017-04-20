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
    public class CharacterCreation : BaseGameState
    {

        #region Field Region

        Label name;
        Label genderLabel;
        Label race;
        Label pickClass;

        LinkLabel startGame;

        ScrollSelector raceScroll;
        string[] races = { "Human", "Elf", "Dwarf", "Goblin", "Orc" };

        ScrollSelector genderScroll;
        string[] gender = { "Male", "Female" };

        ScrollSelector classScroll;
        string[] classes = { "Warrior", "Mage", "Archer", "Rogue" };


        #endregion
        
        #region Property Region
       
        #endregion
        
        #region Constructor Region
        
        public CharacterCreation(Game game, GameStateManager manager) : base(game, manager)
        {

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

            CreateControls();
        }

        public override void Update(GameTime gameTime)
        {
            ControlManager.Update(gameTime, PlayerIndex.One);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GameRef.spriteBatch.Begin();

            base.Draw(gameTime);

            ControlManager.Draw(GameRef.spriteBatch);

            GameRef.spriteBatch.End();
        }

        private void CreateControls()
        {
            //TODO:Find a good position for everything here.
            name = new Label();
            name.Text = "Name: ";
            name.Size = name.SpriteFont.MeasureString(name.Text);
            ControlManager.Add(name);

            genderLabel = new Label();
            genderLabel.Text = "Gender: ";
            genderLabel.Size = genderLabel.SpriteFont.MeasureString(genderLabel.Text);
            genderLabel.Position = new Vector2(0, 40);
            ControlManager.Add(genderLabel);

            genderScroll = new ScrollSelector();
            genderScroll.SetItems(gender, 125);
            genderScroll.Position = new Vector2(120, 40);
            ControlManager.Add(genderScroll);

            race = new Label();
            race.Text = "Race: ";
            race.Size = race.SpriteFont.MeasureString(race.Text);
            race.Position = new Vector2(0, 80);
            ControlManager.Add(race);

            raceScroll = new ScrollSelector();
            raceScroll.SetItems(races, 125);
            raceScroll.Position = new Vector2(120, 80);
            ControlManager.Add(raceScroll);

            pickClass = new Label();
            pickClass.Text = "Class: ";
            pickClass.Size = pickClass.SpriteFont.MeasureString(pickClass.Text);
            pickClass.Position = new Vector2(0, 120);
            ControlManager.Add(pickClass);

            classScroll = new ScrollSelector();
            classScroll.SetItems(classes, 125);
            classScroll.Position = new Vector2(120, 120);
            ControlManager.Add(classScroll);

            startGame = new LinkLabel();
            startGame.Text = "Begin your journey";
            startGame.Size = startGame.SpriteFont.MeasureString(startGame.Text);
            startGame.Selected += linkLabelSelected;
            startGame.Position = new Vector2(0, 160);
            ControlManager.Add(startGame);

            ControlManager.NextControl();

        }

        private void linkLabelSelected(object sender, EventArgs e)
        {
            InputHandler.Flush();

            StateManager.PopState();
            StateManager.PushState(GameRef.gamePlayScreen);
        }

        #endregion
    }
}
