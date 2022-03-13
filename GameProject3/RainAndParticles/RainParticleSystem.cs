using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameProject3
{
    public class RainParticleSystem : ParticleSystem
    {
        Rectangle Source;
        public CardinalDirection Direction = CardinalDirection.East;
        public bool IsRaining { get; set; } = false;

        public RainParticleSystem(Game game, Rectangle source) : base(game, 16000)
        {
            Source = source;
            InitializeConstants();
            LoadContent();
        }

        protected override void InitializeConstants()
        {
            textureFilename = "Raindrop";
            minNumParticles = 60;
            maxNumParticles = 120;
        }

        protected override void InitializeParticle(ref Particle p, Vector2 where)
        {
            var sideVelocity = 0;
            var rotation = 0f;
            switch (Direction)
            {
                case CardinalDirection.North:
                    sideVelocity = -120;
                    rotation = MathHelper.PiOver4;
                    break;
                case CardinalDirection.South:
                    sideVelocity = 120;
                    rotation = -MathHelper.PiOver4;
                    break;
                case CardinalDirection.West:
                    sideVelocity = -30;
                    rotation = MathHelper.PiOver2 / 3;
                    break;
                case CardinalDirection.East:
                    sideVelocity = 30;
                    rotation = -MathHelper.PiOver2/3;
                    break;
            }

            var velocity = new Vector2(sideVelocity, RandomHelper.NextFloat(240, 360));
            var lifetime = 540 / velocity.Y;
            var acceleration = Vector2.Zero;

            p.Initialize(where, velocity, acceleration, new Color(Color.CornflowerBlue, 128), lifetime: lifetime, rotation: rotation);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if(IsRaining) AddParticles(Source);
        }
    }
}
