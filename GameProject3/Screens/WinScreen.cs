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
        private SpriteFont tnr12;
        private SpriteFont tnr30;

        public override void Activate()
        {
            base.Activate();

            if (content == null) content = new ContentManager(ScreenManager.Game.Services, "Content");

            tnr12 = content.Load<SpriteFont>("TNR12");
            tnr30 = content.Load<SpriteFont>("TNR30");
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
            ScreenManager.SpriteBatch.DrawString(tnr30, title, new Vector2(400 - (int)tnr30.MeasureString(title).X / 2,
                120 - (int)tnr30.MeasureString(title).Y / 2), Color.Black);
            title = "YOU WIN!!!";
            ScreenManager.SpriteBatch.DrawString(tnr30, title, new Vector2(400 - (int)tnr30.MeasureString(title).X / 2,
                240 - (int)tnr30.MeasureString(title).Y / 2), Color.Black);
            string instructions = "Press ESC to quit";
            ScreenManager.SpriteBatch.DrawString(tnr12, instructions, new Vector2(400 - (int)tnr12.MeasureString(instructions).X / 2,
                360 - (int)tnr12.MeasureString(instructions).Y / 2), Color.Black);

            ScreenManager.SpriteBatch.End();
        }
    }
}
