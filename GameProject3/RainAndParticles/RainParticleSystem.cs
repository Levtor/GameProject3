using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameProject3
{
    public class RainParticleSystem : ParticleSystem
    {
        Rectangle Source;

        public int Offset = 0;

        public bool IsRaining { get; set; } = false;

        public RainParticleSystem(Game game, Rectangle source) : base(game, 20000)
        {
            Source = source;
            InitializeConstants();
            LoadContent();
        }

        protected override void InitializeConstants()
        {
            textureFilename = "Raindrop";
            minNumParticles = 100;
            maxNumParticles = 150;
        }

        protected override void InitializeParticle(ref Particle p, Vector2 where)
        {
            var sideVelocity = RandomHelper.NextFloat(10, -10);
            var rotation = 0f;
            var velocity = new Vector2(sideVelocity, RandomHelper.NextFloat(250, 400));
            var lifetime = 540 / velocity.Y;
            var acceleration = Vector2.Zero;

            p.Initialize(where, velocity, acceleration, new Color(Color.CornflowerBlue, 128), lifetime: lifetime, rotation: rotation);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if(IsRaining) AddParticles(Source);
        }

        protected override void UpdateParticle(ref Particle particle, float dt)
        {
            // Update particle's linear motion values
            particle.Velocity += particle.Acceleration * dt;
            particle.Position += (particle.Velocity + new Vector2(Offset, 0)) * dt;

            // Update the particle's angular motion values
            particle.AngularVelocity += particle.AngularAcceleration * dt;
            particle.Rotation += particle.AngularVelocity * dt;

            // Update the time the particle has been alive 
            particle.TimeSinceStart += dt;
        }
    }
}
