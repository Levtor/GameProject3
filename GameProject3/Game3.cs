using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameProject3
{
    public class Game3 : Game
    {
        private GraphicsDeviceManager graphics;
        private ScreenManager screenManager;
        private SpriteBatch spriteBatch;

        public Game3()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            var screenFactory = new ScreenFactory();
            Services.AddService(typeof(IScreenFactory), screenFactory);

            screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            
        }
        private void AddInitialScreens()
        {
            screenManager.AddScreen(new WinScreen(), null);
            RainParticleSystem rainSystem = new RainParticleSystem(this, new Rectangle(-200, -30, 1200, 30));
            GameplayScreen gScreen = new GameplayScreen(this);
            gScreen.RainSystem = rainSystem;
            screenManager.AddScreen(gScreen, null);
            screenManager.AddScreen(new TitleScreen(), null);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            AddInitialScreens();
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
