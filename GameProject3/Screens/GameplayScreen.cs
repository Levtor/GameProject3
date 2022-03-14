using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GameProject3
{
    public enum MovementState
    {
        Still,
        Bump,
        MovingForward,
        TurningLeft,
        TurningRight
    }

    // This screen implements the actual game logic. It is just a
    // placeholder to get the idea across: you'll probably want to
    // put some more interesting gameplay in here!
    public class GameplayScreen : GameScreen
    {
        private ContentManager content;

        private Texture2D texture;
        private SoundEffect steps;
        private SoundEffect bump;
        private Song rain;
        private SpriteFont tnr12;
        private SpriteFont tnr30;

        private KeyboardState newKeyboardState;
        private KeyboardState oldKeyboardState;

        private bool firstTime;
        private bool quit;
        private MovementState MovementState = MovementState.Still;
        private float movementAnimationTimer = 0;
        private int X = 0;
        private int Y = 0;
        private int destinationX = 0;
        private int destinationY = 0;
        private CardinalDirection Direction = CardinalDirection.East;
        private CardinalDirection destinationDirection = CardinalDirection.East;
        private int ExitX;
        private int ExitY;
        private CardinalDirection ExitDirection;
        private Maze HedgeMaze;
        public RainParticleSystem RainSystem;

        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        // Load graphics content for the game
        public override void Activate()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            firstTime = true;
            quit = false;
            HedgeMaze = new Maze(10, 6,
                new bool[]
                {
                    true , true , true , true , true , true , true , true , true , true ,
                    false, true , false, false, false, true , true , true , false, false,
                    false, true , false, false, false, true , false, false, true , true ,
                    false, false, true , false, true , false, true , true , true , false,
                    false, true , true , true , false, true , true , true , true , false,
                    false, false, true , true , false, true , false, false, true , false,
                    true , true , true , true , true , true , true , true , true , true 
                },
                new bool[]
                {
                    true , false, false, true , true , false, false, false, false, true , true ,
                    true , true , false, false, true , true , false, true , true , false, true ,
                    true , true , true , true , true , false, false, true , false, false, true ,
                    true , true , false, false, false, true , false, false, false, false, true ,
                    true , false, true , false, true , false, false, false, true , true , true ,
                    true , false, false, false, false, true , true , false, false, false, true 
                });
            ExitX = 9;
            ExitY = 0;
            ExitDirection = CardinalDirection.North;

            // TODO: use this.Content to load your game content here
            texture = content.Load<Texture2D>("HedgeMaze");
            //steps = content.Load<SoundEffect>("LetterHit");
            //bump = content.Load<SoundEffect>("LetterHit");
            //rain = content.Load<Song>("Lobo Loco - Woke up This Morning - RocknRoll (ID 1672)");
            tnr12 = content.Load<SpriteFont>("TNR12");
            tnr30 = content.Load<SpriteFont>("TNR30");
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Unload()
        {
            content.Unload();
        }

        // This method checks the GameScreen.IsActive property, so the game will
        // stop updating when the pause menu is active, or if you tab away to a different application.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            if (IsActive)
            {
                RainSystem.Update(gameTime);
            }
        }

        // Unlike the Update method, this will only be called when the gameplay screen is active.
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (firstTime)
            {
                firstTime = false;
                if (RainSystem != null) RainSystem.IsRaining = true;
                //MediaPlayer.IsRepeating = true;
                //MediaPlayer.Play(rain);
                newKeyboardState = Keyboard.GetState();
            }

            oldKeyboardState = newKeyboardState;
            newKeyboardState = Keyboard.GetState();

            if(MovementState == MovementState.Still)
            {
                if (newKeyboardState.IsKeyDown(Keys.Up) && oldKeyboardState.IsKeyUp(Keys.Up))
                {
                    if (HedgeMaze.HasWall(X, Y, Direction))
                    {
                        MovementState = MovementState.Bump;
                        //bump.Play();
                        movementAnimationTimer = 0;
                    }
                    else
                    {
                        MovementState = MovementState.MovingForward;
                        //steps.Play();
                        movementAnimationTimer = 0;
                        destinationX = X;
                        destinationY = Y;
                        destinationDirection = Direction;
                        if (Direction == CardinalDirection.North) destinationY -= 1;
                        else if (Direction == CardinalDirection.South) destinationY += 1;
                        else if (Direction == CardinalDirection.West) destinationX -= 1;
                        else if (Direction == CardinalDirection.East) destinationX += 1;
                    }
                }
                else if (newKeyboardState.IsKeyDown(Keys.Left) && oldKeyboardState.IsKeyUp(Keys.Left))
                {
                    MovementState = MovementState.TurningLeft;
                    //steps.Play();
                    movementAnimationTimer = 0;
                    destinationX = X;
                    destinationY = Y;
                    destinationDirection = (CardinalDirection)(((int)Direction + 1) % 4);
                }
                else if (newKeyboardState.IsKeyDown(Keys.Right) && oldKeyboardState.IsKeyUp(Keys.Right))
                {
                    MovementState = MovementState.TurningRight;
                    //steps.Play();
                    movementAnimationTimer = 0;
                    destinationX = X;
                    destinationY = Y;
                    destinationDirection = (CardinalDirection)(((int)Direction + 3) % 4);
                }
                else if (newKeyboardState.IsKeyDown(Keys.Space) && oldKeyboardState.IsKeyUp(Keys.Space))
                {
                    if (X == ExitX && Y == ExitY && Direction == ExitDirection)
                    {
                        quit = true;
                    }
                }
            }
            else
            {
                movementAnimationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (movementAnimationTimer > 0.5)
                {
                    MovementState = MovementState.Still;
                    X = destinationX;
                    Y = destinationY;
                    Direction = destinationDirection;
                    RainSystem.Direction = Direction;
                }
            }

            if (quit)
            {
                MediaPlayer.Stop();
                ExitScreen();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here
            var spriteBatch = ScreenManager.SpriteBatch;
            SamplerState samplerState = SamplerState.PointClamp;
            float scaleFactor;
            Matrix transform;
            switch (MovementState)
            {
                case MovementState.Still:
                    spriteBatch.Begin(SpriteSortMode.Deferred, null, samplerState, null, null, null, null);
                    DrawHedgeView(spriteBatch, X, Y, Direction, 0);
                    spriteBatch.End();
                    break;
                case MovementState.Bump:
                    scaleFactor = 1.25f - 4 * (movementAnimationTimer - .25f) * (movementAnimationTimer - .25f);
                    transform = Matrix.CreateScale(scaleFactor) * Matrix.CreateTranslation(400 - 400 * scaleFactor, 240 - 240 * scaleFactor, 0);
                    spriteBatch.Begin(SpriteSortMode.Deferred, null, samplerState, null, null, null, transform);
                    DrawHedgeView(spriteBatch, X, Y, Direction, 0);
                    spriteBatch.End();
                    break;
                case MovementState.MovingForward:
                    scaleFactor = 1 + 4*movementAnimationTimer/3;
                    transform = Matrix.CreateScale(scaleFactor) * Matrix.CreateTranslation(400 - 400 * scaleFactor, 240 - 240 * scaleFactor, 0);
                    spriteBatch.Begin(SpriteSortMode.Deferred, null, samplerState, null, null, null, transform);
                    DrawHedgeView(spriteBatch, X, Y, Direction, 0);
                    spriteBatch.End();
                    break;
                case MovementState.TurningLeft:
                    spriteBatch.Begin(SpriteSortMode.Deferred, null, samplerState, null, null, null, null);
                    DrawHedgeView(spriteBatch, X, Y, destinationDirection, -800*(1 - 2 * movementAnimationTimer));
                    DrawHedgeView(spriteBatch, X, Y, Direction, 800 * 2 * movementAnimationTimer);
                    spriteBatch.End();
                    break;
                case MovementState.TurningRight:
                    spriteBatch.Begin(SpriteSortMode.Deferred, null, samplerState, null, null, null, null);
                    DrawHedgeView(spriteBatch, X, Y, destinationDirection, 800 * (1 - 2 * movementAnimationTimer));
                    DrawHedgeView(spriteBatch, X, Y, Direction, -800 * 2 * movementAnimationTimer);
                    spriteBatch.End();
                    break;
            }

            spriteBatch.Begin();
            RainSystem.Draw(gameTime);
            spriteBatch.End();

            if (X==ExitX && Y == ExitY && Direction == ExitDirection)
            {
                spriteBatch.Begin();
                string title = "EXIT";
                ScreenManager.SpriteBatch.DrawString(tnr30, title, new Vector2(400 - (int)tnr30.MeasureString(title).X / 2,
                    120 - (int)tnr30.MeasureString(title).Y / 2), Color.White);
                string instructions = "(Press SPACE to win!)";
                ScreenManager.SpriteBatch.DrawString(tnr12, instructions, new Vector2(400 - (int)tnr12.MeasureString(instructions).X / 2,
                    200 - (int)tnr12.MeasureString(instructions).Y / 2), Color.White);
                spriteBatch.End();
            }
        }

        private void DrawHedgeView(SpriteBatch spriteBatch, int x, int y, CardinalDirection direction, float offset)
        {
            // furthest-back-ground (and sky)
            spriteBatch.Draw(texture, new Vector2(offset, 0), new Rectangle(0, 30, 50, 30), Color.White, 0, new Vector2(0, 0), 16, SpriteEffects.None, 0);
            int xOffset = 0;
            int yOffset = 0;
            switch (direction)
            {
                case CardinalDirection.North:
                    yOffset = -1;
                    break;
                case CardinalDirection.West:
                    xOffset = -1;
                    break;
                case CardinalDirection.South:
                    yOffset = 1;
                    break;
                case CardinalDirection.East:
                    xOffset = 1;
                    break;
            }
            // distant squares
            for (int i = 3; i > 0; i--)
            {
                DrawHedgeSquare(spriteBatch, x + xOffset * i, y + yOffset * i, direction, i, offset);
            }
            // current square
            if (HedgeMaze.HasWall(x, y, direction))
                spriteBatch.Draw(texture,
                    new Vector2(offset, 0),
                    new Rectangle(0, 0, 50, 30), Color.White, 0, new Vector2(0, 0), 16, SpriteEffects.None, 0);
        }

        private void DrawHedgeSquare(SpriteBatch spriteBatch, int x, int y, CardinalDirection direction, int depth, float offset)
        {
            float sizeScale1 = 0.6f;
            float sizeScale2 = 1;
            for (int i=depth; i > 1; i--)
            {
                sizeScale1 *= 0.6f;
                sizeScale2 *= 0.6f;
            }
            float locationScale1 = (1 - sizeScale1) / 2;
            float locationScale2 = (1 - sizeScale2) / 2;
            int eitherSide = (int)Math.Ceiling((1 - sizeScale1) / (2 * sizeScale1));

            int horizontI = 0;
            int verticI = 0;
            switch (direction)
            {
                case CardinalDirection.North:
                    horizontI = 1;
                    break;
                case CardinalDirection.West:
                    verticI = -1;
                    break;
                case CardinalDirection.South:
                    horizontI = -1;
                    break;
                case CardinalDirection.East:
                    verticI = 1;
                    break;
            }
            for (int i = -eitherSide; i <= eitherSide; i++)
            {
                if (HedgeMaze.HasWall(x + horizontI * i, y + verticI * i, direction))
                    spriteBatch.Draw(texture,
                        new Vector2(offset + (800 * locationScale1) + i * (800 * sizeScale1), (480 * locationScale1)),
                        new Rectangle(0, 0, 50, 30), Color.White, 0, new Vector2(0, 0), 16 * sizeScale1, SpriteEffects.None, 0);
            }
            for (int i = 1 - eitherSide; i < eitherSide; i++)
            {
                if (HedgeMaze.HasWall(x + horizontI * i, y + verticI * i, (CardinalDirection)(((int)direction + 1) % 4)))
                    spriteBatch.Draw(texture,
                        new Vector2(offset + (800 * locationScale2) + i * (800 * sizeScale2), (480 * locationScale2)),
                        new Rectangle(50, 0, 50, 30), Color.White, 0, new Vector2(0, 0), 16 * sizeScale2, SpriteEffects.None, 0);
                if (HedgeMaze.HasWall(x + horizontI * i, y + verticI * i, (CardinalDirection)(((int)direction + 3) % 4)))
                    spriteBatch.Draw(texture,
                        new Vector2(offset + (800 * locationScale2) + i * (800 * sizeScale2), (480 * locationScale2)),
                        new Rectangle(50, 0, 50, 30), Color.White, 0, new Vector2(0, 0), 16 * sizeScale2, SpriteEffects.FlipHorizontally, 0);
            }
        }
    }
}
