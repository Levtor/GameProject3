using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject3
{
    /// <summary>
    /// A class representing a quad (a rectangle composed of two triangles)
    /// </summary>
    public class Quad
    {
        /// <summary>
        /// The vertices of the quad
        /// </summary>
        VertexPositionTexture[] vertices;

        /// <summary>
        /// The vertex indices of the quad
        /// </summary>
        short[] indices;

        /// <summary>
        /// The effect to use rendering the quad
        /// </summary>
        BasicEffect effect;

        /// <summary>
        /// The game this quad belongs to 
        /// </summary>
        Game game;

        /// <summary>
        /// Constructs the Quad
        /// </summary>
        /// <param name="game">The Game the Quad belongs to</param>
        public Quad(Game game, Vector3 topLeft, Vector3 topRight, Vector3 bottomLeft, Vector3 bottomRight)
        {
            this.game = game;
            InitializeVertices(topLeft, topRight, bottomLeft, bottomRight);
            InitializeIndices();
            InitializeEffect();
        }

        /// <summary>
        /// Initializes the vertices of our quad
        /// </summary>
        public void InitializeVertices(Vector3 topLeft, Vector3 topRight, Vector3 bottomLeft, Vector3 bottomRight)
        {
            vertices = new VertexPositionTexture[4];
            // Define vertex 0 (top left)
            vertices[0].Position = topLeft;
            vertices[0].TextureCoordinate = new Vector2(0, -1);
            // Define vertex 1 (top right)
            vertices[1].Position = topRight;
            vertices[1].TextureCoordinate = new Vector2(1, -1);
            // define vertex 2 (bottom right)
            vertices[2].Position = bottomRight;
            vertices[2].TextureCoordinate = new Vector2(1, 0);
            // define vertex 3 (bottom left) 
            vertices[3].Position = bottomLeft;
            vertices[3].TextureCoordinate = new Vector2(0, 0);
        }

        /// <summary>
        /// Initialize the indices of our quad
        /// </summary>
        public void InitializeIndices()
        {
            indices = new short[6];

            // Define triangle 0 
            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 2;
            // define triangle 1
            indices[3] = 2;
            indices[4] = 3;
            indices[5] = 0;
        }

        /// <summary>
        /// Initializes the basic effect used to draw the quad
        /// </summary>
        public void InitializeEffect()
        {
            effect = new BasicEffect(game.GraphicsDevice);
            effect.World = Matrix.Identity;
            effect.TextureEnabled = true;
            effect.Texture = game.Content.Load<Texture2D>("Hedge");
        }

        /// <summary>
        /// Draws the quad
        /// </summary>
        /// <param name="camera">The camera to use to draw the quad</param>
        public void Draw(Matrix view, Matrix projection)
        {
            // Cache old rasterizer state
            RasterizerState oldState = game.GraphicsDevice.RasterizerState;

            // Disable backface culling 
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            game.GraphicsDevice.RasterizerState = rasterizerState;

            // set the view and projection matrices
            effect.View = view;
            effect.Projection = projection;

            // Apply our effect
            effect.CurrentTechnique.Passes[0].Apply();

            // Render the quad
            game.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionTexture>(
                PrimitiveType.TriangleList,
                vertices,   // The vertex collection
                0,          // The starting index in the vertex array
                4,          // The number of indices in the shape
                indices,    // The index collection
                0,          // The starting index in the index array
                2           // The number of triangles to draw
            );

            // Restore the prior rasterizer state 
            game.GraphicsDevice.RasterizerState = oldState;
        }
    }
}
