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

    public class GameplayScreen : GameScreen
    {
        private ContentManager content;
        private Game Game;
        private RainParticleSystem RainSystem;

        private Texture2D texture;
        private SoundEffect steps1;
        private SoundEffect steps2;
        private SoundEffect steps3;
        private SoundEffect steps4;
        private SoundEffect steps5;
        private SoundEffect bump;
        private Song rain;
        private SpriteFont tnrsmall;
        private SpriteFont tnrbig;

        private KeyboardState newKeyboardState;
        private KeyboardState oldKeyboardState;

        private string MazeFileName;
        private IMaze HedgeMaze;
        private int X;
        private int Y;
        private int destinationX;
        private int destinationY;
        private CardinalDirection Direction;
        private CardinalDirection destinationDirection;
        private int visionDepth;

        private Matrix Projection;
        private QuadMazeView DrawsMaze;

        private bool firstTime = true;
        private bool quit = false;
        private bool pixelated = false;
        private MovementState MovementState = MovementState.Still;
        private float movementAnimationTimer = 0;

        public GameplayScreen(Game game, RainParticleSystem rainSystem, string mazeFileName)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            Game = game;
            RainSystem = rainSystem;
            MazeFileName = mazeFileName;
        }

        // Load graphics content for the game
        public override void Activate()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            HedgeMaze = new SquareMaze(MazeFileName, content);
            X = HedgeMaze.StartX;
            Y = HedgeMaze.StartY;
            destinationX = HedgeMaze.StartX;
            destinationY = HedgeMaze.StartY;
            Direction = HedgeMaze.StartDirection;
            destinationDirection = HedgeMaze.StartDirection;
            visionDepth = HedgeMaze.VisionDepth;

            texture = content.Load<Texture2D>("HedgeMaze");
            steps1 = content.Load<SoundEffect>("footsteps1");
            steps2 = content.Load<SoundEffect>("footsteps2");
            steps3 = content.Load<SoundEffect>("footsteps3");
            steps4 = content.Load<SoundEffect>("footsteps4");
            steps5 = content.Load<SoundEffect>("footsteps5");
            bump = content.Load<SoundEffect>("zapsplat_foley_bush_leaves_thick_hard_quick_movement_impact_001");
            rain = content.Load<Song>("zapsplat_nature_rain_downpour_garden_medium_turns_heavy_half_way_wind_picks_up_80329");
            MediaPlayer.Volume = 0.5f;
            tnrsmall = content.Load<SpriteFont>("TNRsmall");
            tnrbig = content.Load<SpriteFont>("TNRbig");

            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Game.GraphicsDevice.Viewport.AspectRatio, .1f, 100f);
            DrawsMaze = new QuadMazeView(visionDepth, Game);
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
                UpdateFallingRainDirection();
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(rain);
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
                        bump.Play();
                        movementAnimationTimer = 0;
                    }
                    else
                    {
                        MovementState = MovementState.MovingForward;
                        PlayFootsteps();
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
                    PlayFootsteps();
                    movementAnimationTimer = 0;
                    destinationX = X;
                    destinationY = Y;
                    destinationDirection = (CardinalDirection)(((int)Direction + 1) % 4);
                }
                else if (newKeyboardState.IsKeyDown(Keys.Right) && oldKeyboardState.IsKeyUp(Keys.Right))
                {
                    MovementState = MovementState.TurningRight;
                    PlayFootsteps();
                    movementAnimationTimer = 0;
                    destinationX = X;
                    destinationY = Y;
                    destinationDirection = (CardinalDirection)(((int)Direction + 3) % 4);
                }
                else if (newKeyboardState.IsKeyDown(Keys.Space) && oldKeyboardState.IsKeyUp(Keys.Space))
                {
                    if (X == HedgeMaze.ExitX && Y == HedgeMaze.ExitY && Direction == HedgeMaze.ExitDirection)
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

                    UpdateFallingRainDirection();
                }
            }

            if (quit)
            {
                MediaPlayer.Stop();
                ExitScreen();
            }
        }
        public void UpdateFallingRainDirection()
        {
            switch (Direction)
            {
                case CardinalDirection.North:
                    RainSystem.Offset = 200 * (X - HedgeMaze.ExitX) / (HedgeMaze.Width - 2);
                    break;
                case CardinalDirection.South:
                    RainSystem.Offset = 200 * (HedgeMaze.ExitX - X) / (HedgeMaze.Width - 2);
                    break;
                case CardinalDirection.West:
                    RainSystem.Offset = 200 * (HedgeMaze.ExitY - Y) / (HedgeMaze.Height - 2);
                    break;
                case CardinalDirection.East:
                    RainSystem.Offset = 200 * (Y - HedgeMaze.ExitY) / (HedgeMaze.Height - 2);
                    break;
            }
        }

        private int lastStepPlayed = 5;
        private void PlayFootsteps()
        {
            int stepPair = RandomHelper.Next(1, 4);
            if (stepPair == lastStepPlayed) stepPair++;
            switch (stepPair)
            {
                case 1:
                    steps1.Play();
                    break;
                case 2:
                    steps2.Play();
                    break;
                case 3:
                    steps3.Play();
                    break;
                case 4:
                    steps4.Play();
                    break;
                case 5:
                    steps5.Play();
                    break;
            }
            lastStepPlayed = stepPair;
        }

        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = ScreenManager.SpriteBatch;

            if (pixelated) Draw2D(spriteBatch);
            else Draw3D(spriteBatch);

            spriteBatch.Begin();
            RainSystem.Draw(gameTime);
            spriteBatch.End();

            if (X == HedgeMaze.ExitX && Y == HedgeMaze.ExitY && Direction == HedgeMaze.ExitDirection)
            {
                spriteBatch.Begin();
                string title = "EXIT";
                ScreenManager.SpriteBatch.DrawString(tnrbig, title, new Vector2(400 - (int)tnrbig.MeasureString(title).X / 2,
                    120 - (int)tnrbig.MeasureString(title).Y / 2), Color.White);
                string instructions = "(Press SPACE to proceed)";
                ScreenManager.SpriteBatch.DrawString(tnrsmall, instructions, new Vector2(400 - (int)tnrsmall.MeasureString(instructions).X / 2,
                    200 - (int)tnrsmall.MeasureString(instructions).Y / 2), Color.White);
                spriteBatch.End();
            }
        }

        private void Draw3D(SpriteBatch spriteBatch)
        {
            // furthest-back-ground (and sky)
            SamplerState samplerState = SamplerState.PointClamp;
            spriteBatch.Begin(SpriteSortMode.Deferred, null, samplerState, null, null, null, null);
            spriteBatch.Draw(texture, Vector2.Zero, new Rectangle(0, 30, 50, 30), Color.White, 0, new Vector2(0, 0), 16, SpriteEffects.None, 0);
            spriteBatch.End();
            float scaleFactor;
            Matrix view;
            Vector3 viewForward = Vector3.Forward;
            switch (Direction)
            {
                case CardinalDirection.West:
                    viewForward = Vector3.Transform(viewForward, Matrix.CreateRotationY(MathHelper.PiOver2));
                    break;
                case CardinalDirection.South:
                    viewForward = Vector3.Transform(viewForward, Matrix.CreateRotationY(MathHelper.Pi));
                    break;
                case CardinalDirection.East:
                    viewForward = Vector3.Transform(viewForward, Matrix.CreateRotationY(-MathHelper.PiOver2));
                    break;
            }
            switch (MovementState)
            {
                case MovementState.Still:
                    view = Matrix.CreateLookAt(Vector3.Zero, viewForward, Vector3.Up);
                    DrawsMaze.Draw(view, Projection, HedgeMaze, X, Y);
                    break;
                case MovementState.Bump:
                    scaleFactor = 0.25f - 4 * (movementAnimationTimer - .25f) * (movementAnimationTimer - .25f);
                    view = Matrix.CreateLookAt(viewForward * scaleFactor, viewForward, Vector3.Up);
                    DrawsMaze.Draw(view, Projection, HedgeMaze, X, Y);
                    break;
                case MovementState.MovingForward:
                    scaleFactor = 4 * movementAnimationTimer;
                    view = Matrix.CreateLookAt(viewForward * scaleFactor, viewForward * (scaleFactor + 1), Vector3.Up);
                    DrawsMaze.Draw(view, Projection, HedgeMaze, X, Y);
                    break;
                case MovementState.TurningLeft:
                    scaleFactor = MathHelper.Pi * movementAnimationTimer;
                    view = Matrix.CreateLookAt(Vector3.Zero, Vector3.Transform(viewForward, Matrix.CreateRotationY(scaleFactor)), Vector3.Up);
                    DrawsMaze.Draw(view, Projection, HedgeMaze, X, Y);
                    break;
                case MovementState.TurningRight:
                    scaleFactor = -MathHelper.Pi * movementAnimationTimer;
                    view = Matrix.CreateLookAt(Vector3.Zero, Vector3.Transform(viewForward, Matrix.CreateRotationY(scaleFactor)), Vector3.Up);
                    DrawsMaze.Draw(view, Projection, HedgeMaze, X, Y);
                    break;
            }
        }

        private void Draw2D(SpriteBatch spriteBatch)
        {
            SamplerState samplerState = SamplerState.PointClamp;
            float scaleFactor;
            Matrix transform;
            switch (MovementState)
            {
                case MovementState.Still:
                    spriteBatch.Begin(SpriteSortMode.Deferred, null, samplerState, null, null, null, null);
                    DrawHedgeView2D(spriteBatch, X, Y, Direction, 0);
                    spriteBatch.End();
                    break;
                case MovementState.Bump:
                    scaleFactor = 1.25f - 4 * (movementAnimationTimer - .25f) * (movementAnimationTimer - .25f);
                    transform = Matrix.CreateScale(scaleFactor) * Matrix.CreateTranslation(400 - 400 * scaleFactor, 240 - 240 * scaleFactor, 0);
                    spriteBatch.Begin(SpriteSortMode.Deferred, null, samplerState, null, null, null, transform);
                    DrawHedgeView2D(spriteBatch, X, Y, Direction, 0);
                    spriteBatch.End();
                    break;
                case MovementState.MovingForward:
                    scaleFactor = 1 + 4*movementAnimationTimer/3;
                    transform = Matrix.CreateScale(scaleFactor) * Matrix.CreateTranslation(400 - 400 * scaleFactor, 240 - 240 * scaleFactor, 0);
                    spriteBatch.Begin(SpriteSortMode.Deferred, null, samplerState, null, null, null, transform);
                    DrawHedgeView2D(spriteBatch, X, Y, Direction, 0);
                    spriteBatch.End();
                    break;
                case MovementState.TurningLeft:
                    spriteBatch.Begin(SpriteSortMode.Deferred, null, samplerState, null, null, null, null);
                    DrawHedgeView2D(spriteBatch, X, Y, destinationDirection, -800*(1 - 2 * movementAnimationTimer));
                    DrawHedgeView2D(spriteBatch, X, Y, Direction, 800 * 2 * movementAnimationTimer);
                    spriteBatch.End();
                    break;
                case MovementState.TurningRight:
                    spriteBatch.Begin(SpriteSortMode.Deferred, null, samplerState, null, null, null, null);
                    DrawHedgeView2D(spriteBatch, X, Y, destinationDirection, 800 * (1 - 2 * movementAnimationTimer));
                    DrawHedgeView2D(spriteBatch, X, Y, Direction, -800 * 2 * movementAnimationTimer);
                    spriteBatch.End();
                    break;
            }
        }

        private void DrawHedgeView2D(SpriteBatch spriteBatch, int x, int y, CardinalDirection direction, float offset)
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
            for (int i = visionDepth; i > 0; i--)
            {
                DrawHedgeSquare2D(spriteBatch, x + xOffset * i, y + yOffset * i, direction, i, offset);
            }
            // current square
            if (HedgeMaze.HasWall(x, y, direction))
                spriteBatch.Draw(texture,
                    new Vector2(offset, 0),
                    new Rectangle(0, 0, 50, 30), Color.White, 0, new Vector2(0, 0), 16, SpriteEffects.None, 0);
        }

        private void DrawHedgeSquare2D(SpriteBatch spriteBatch, int x, int y, CardinalDirection direction, int depth, float offset)
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
