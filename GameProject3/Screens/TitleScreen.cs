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

            if (input.CurrentKeyboardStates[0].IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter)) ExitScreen();
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);

            ScreenManager.SpriteBatch.Begin();
            
            string title = "A DARK AND STORMY NIGHT";
            ScreenManager.SpriteBatch.DrawString(tnr30, title, new Vector2(400 - (int)tnr30.MeasureString(title).X / 2,
                120 - (int)tnr30.MeasureString(title).Y / 2), Color.Black);
            string instructions = "Use the forward key to move forward.";
            ScreenManager.SpriteBatch.DrawString(tnr12, instructions, new Vector2(400 - (int)tnr12.MeasureString(instructions).X / 2,
                200 - (int)tnr12.MeasureString(instructions).Y / 2), Color.Black);
            instructions = "Use the left and right keys to rotate.";
            ScreenManager.SpriteBatch.DrawString(tnr12, instructions, new Vector2(400 - (int)tnr12.MeasureString(instructions).X / 2,
                220 - (int)tnr12.MeasureString(instructions).Y / 2), Color.Black);
            instructions = "Find the exit and press SPACE there to win!";
            ScreenManager.SpriteBatch.DrawString(tnr12, instructions, new Vector2(400 - (int)tnr12.MeasureString(instructions).X / 2,
                240 - (int)tnr12.MeasureString(instructions).Y / 2), Color.Black);
            instructions = "Press ENTER to continue";
            ScreenManager.SpriteBatch.DrawString(tnr12, instructions, new Vector2(400 - (int)tnr12.MeasureString(instructions).X / 2,
                260 - (int)tnr12.MeasureString(instructions).Y / 2), Color.Black);
            instructions = "(BEWARE: the rain falls away from the EXIT)";
            ScreenManager.SpriteBatch.DrawString(tnr12, instructions, new Vector2(400 - (int)tnr12.MeasureString(instructions).X / 2,
                300 - (int)tnr12.MeasureString(instructions).Y / 2), Color.Black);

            ScreenManager.SpriteBatch.End();
        }
    }
}
