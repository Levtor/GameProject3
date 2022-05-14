using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GameProject3
{
    public class TitleScreen : GameScreen
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

            if (input.CurrentKeyboardStates[0].IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter)) ExitScreen();
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);

            ScreenManager.SpriteBatch.Begin();
            
            string title = "A DARK AND STORMY NIGHT";
            ScreenManager.SpriteBatch.DrawString(tnrbig, title, new Vector2(400 - (int)tnrbig.MeasureString(title).X / 2,
                100 - (int)tnrbig.MeasureString(title).Y / 2), Color.Black);
            string instructions = "Use the forward key to move forward.";
            ScreenManager.SpriteBatch.DrawString(tnrsmall, instructions, new Vector2(400 - (int)tnrsmall.MeasureString(instructions).X / 2,
                200 - (int)tnrsmall.MeasureString(instructions).Y / 2), Color.Black);
            instructions = "Use the left and right keys to rotate.";
            ScreenManager.SpriteBatch.DrawString(tnrsmall, instructions, new Vector2(400 - (int)tnrsmall.MeasureString(instructions).X / 2,
                230 - (int)tnrsmall.MeasureString(instructions).Y / 2), Color.Black);

            instructions = "BEWARE: the rain is a trickster";
            ScreenManager.SpriteBatch.DrawString(tnrsmall, instructions, new Vector2(400 - (int)tnrsmall.MeasureString(instructions).X / 2,
                290 - (int)tnrsmall.MeasureString(instructions).Y / 2), Color.Black);
            instructions = "and leads away from the EXITs";
            ScreenManager.SpriteBatch.DrawString(tnrsmall, instructions, new Vector2(400 - (int)tnrsmall.MeasureString(instructions).X / 2,
                320 - (int)tnrsmall.MeasureString(instructions).Y / 2), Color.Black);

            instructions = "(Press ENTER to continue)";
            ScreenManager.SpriteBatch.DrawString(tnrsmall, instructions, new Vector2(400 - (int)tnrsmall.MeasureString(instructions).X / 2,
                410 - (int)tnrsmall.MeasureString(instructions).Y / 2), Color.Black);

            ScreenManager.SpriteBatch.End();
        }
    }
}
