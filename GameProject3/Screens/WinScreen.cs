using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GameProject3
{
    public class WinScreen : GameScreen
    {
        ContentManager content;
        private SpriteFont tnrsmall;
        private SpriteFont tnrbig;

        public override void Activate()
        {
            base.Activate();

            if (content == null) content = new ContentManager(ScreenManager.Game.Services, "Content");

            tnrsmall = content.Load<SpriteFont>("TNRsmall");
            tnrbig = content.Load<SpriteFont>("TNRbig");
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            base.HandleInput(gameTime, input);
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.MediumPurple, 0, 0);

            ScreenManager.SpriteBatch.Begin();

            string title = "CONGRATULATIONS!";
            ScreenManager.SpriteBatch.DrawString(tnrbig, title, new Vector2(400 - (int)tnrbig.MeasureString(title).X / 2,
                120 - (int)tnrbig.MeasureString(title).Y / 2), Color.Black);
            title = "YOU WIN!!!";
            ScreenManager.SpriteBatch.DrawString(tnrbig, title, new Vector2(400 - (int)tnrbig.MeasureString(title).X / 2,
                240 - (int)tnrbig.MeasureString(title).Y / 2), Color.Black);
            string instructions = "Press ESC to quit";
            ScreenManager.SpriteBatch.DrawString(tnrsmall, instructions, new Vector2(400 - (int)tnrsmall.MeasureString(instructions).X / 2,
                360 - (int)tnrsmall.MeasureString(instructions).Y / 2), Color.Black);

            ScreenManager.SpriteBatch.End();
        }
    }
}
