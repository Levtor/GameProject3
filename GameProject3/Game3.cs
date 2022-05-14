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
            RainParticleSystem rainSystem = new RainParticleSystem(this, new Rectangle(-200, -30, 1200, 30));
            GameplayScreen gScreen1 = new GameplayScreen(this, rainSystem, "Maze1.txt");
            GameplayScreen gScreen2 = new GameplayScreen(this, rainSystem, "Maze2.txt");
            GameplayScreen gScreen3 = new GameplayScreen(this, rainSystem, "Maze3.txt");
            GameplayScreen gScreen4 = new GameplayScreen(this, rainSystem, "Maze4.txt");
            GameplayScreen gScreen5 = new GameplayScreen(this, rainSystem, "Maze5.txt");

            screenManager.AddScreen(new WinScreen(), null);
            screenManager.AddScreen(gScreen5, null);
            screenManager.AddScreen(gScreen4, null);
            screenManager.AddScreen(gScreen3, null);
            screenManager.AddScreen(gScreen2, null);
            screenManager.AddScreen(gScreen1, null);
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
