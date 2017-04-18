using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using RagolRogueLike.GameStates;
using RagolRogueLike.GameScreens;

namespace RagolRogueLike
{
    
    public class Game1 : Game
    {
        #region Monogame Field Region

        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        #endregion

        #region Game State Region

        GameStateManager stateManager;

        public TitleScreen titleScreen;
        public StartMenuScreen startMenuScreen;
        public GamePlayScreen gamePlayScreen;
        public EndGameScreen endGameScreen;

        #endregion

        #region Screen Field Region

        const int screenWidth = 1024;
        const int screenHeight = 768;

        public readonly Rectangle screenRectangle;

        #endregion

        #region Constructor Region

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;

            screenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);

            Content.RootDirectory = "Content";

            this.IsMouseVisible = true;

            Components.Add(new InputHandler(this));

            stateManager = new GameStateManager(this);
            Components.Add(stateManager);

            titleScreen = new TitleScreen(this, stateManager);
            startMenuScreen = new StartMenuScreen(this, stateManager);
            gamePlayScreen = new GamePlayScreen(this, stateManager);
            endGameScreen = new EndGameScreen(this, stateManager);

            stateManager.ChangeState(titleScreen);
        }

        #endregion

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }
    }
}
