using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameProject3
{
    public class QuadMazeView
    {
        int squareSideLength;
        Quad[] quads;

        public QuadMazeView(int depth, Game game)
        {
            squareSideLength = 2 * depth + 1;
            quads = new Quad[(8 * depth + 4) * (depth + 1)];
            int quadIndex = 0;
            //defines the quads on an inward clockwise spiral
            for (int i = squareSideLength; i > 0; i--)
            {
                if (i % 2 == 1)
                {
                    for (int j = 0; j < i; j++)
                    {
                        quads[quadIndex] = new Quad(game,
                            new Vector3((2 * j - i), .4f, -i),
                            new Vector3((2 + 2 * j - i), .4f, -i),
                            new Vector3((2 * j - i), -.6f, -i),
                            new Vector3((2 + 2 * j - i), -.6f, -i)
                            );
                        quadIndex++;
                    }
                    for (int j = 0; j < i; j++)
                    {
                        quads[quadIndex] = new Quad(game,
                            new Vector3(i, .4f, (2 * j - i)),
                            new Vector3(i, .4f, (2 + 2 * j - i)),
                            new Vector3(i, -.6f, (2 * j - i)),
                            new Vector3(i, -.6f, (2 + 2 * j - i))
                            );
                        quadIndex++;
                    }
                    for (int j = 0; j < i; j++)
                    {
                        quads[quadIndex] = new Quad(game,
                            new Vector3(-(2 * j - i), .4f, i),
                            new Vector3(-(2 + 2 * j - i), .4f, i),
                            new Vector3(-(2 * j - i), -.6f, i),
                            new Vector3(-(2 + 2 * j - i), -.6f, i)
                            );
                        quadIndex++;
                    }
                    for (int j = 0; j < i; j++)
                    {
                        quads[quadIndex] = new Quad(game,
                            new Vector3(-i, .4f, -(2 * j - i)),
                            new Vector3(-i, .4f, -(2 + 2 * j - i)),
                            new Vector3(-i, -.6f, -(2 * j - i)),
                            new Vector3(-i, -.6f, -(2 + 2 * j - i))
                            );
                        quadIndex++;
                    }
                }
                else
                {
                    for (int j = 0; j < i; j++)
                    {
                        quads[quadIndex] = new Quad(game,
                            new Vector3((1 + 2 * j - i), .4f, -(i - 1)),
                            new Vector3((1 + 2 * j - i), .4f, -(i + 1)),
                            new Vector3((1 + 2 * j - i), -.6f, -(i - 1)),
                            new Vector3((1 + 2 * j - i), -.6f, -(i + 1))
                            );
                        quadIndex++;
                    }
                    for (int j = 0; j < i; j++)
                    {
                        quads[quadIndex] = new Quad(game,
                            new Vector3(i - 1, .4f, (1 + 2 * j - i)),
                            new Vector3(i + 1, .4f, (1 + 2 * j - i)),
                            new Vector3(i - 1, -.6f, (1 + 2 * j - i)),
                            new Vector3(i + 1, -.6f, (1 + 2 * j - i))
                            );
                        quadIndex++;
                    }
                    for (int j = 0; j < i; j++)
                    {
                        quads[quadIndex] = new Quad(game,
                            new Vector3(-(1 + 2 * j - i), .4f, i - 1),
                            new Vector3(-(1 + 2 * j - i), .4f, i + 1),
                            new Vector3(-(1 + 2 * j - i), -.6f, i - 1),
                            new Vector3(-(1 + 2 * j - i), -.6f, i + 1)
                            );
                        quadIndex++;
                    }
                    for (int j = 0; j < i; j++)
                    {
                        quads[quadIndex] = new Quad(game,
                            new Vector3(-i + 1, .4f, -(1 + 2 * j - i)),
                            new Vector3(-i - 1, .4f, -(1 + 2 * j - i)),
                            new Vector3(-i + 1, -.6f, -(1 + 2 * j - i)),
                            new Vector3(-i - 1, -.6f, -(1 + 2 * j - i))
                            );
                        quadIndex++;
                    }
                }
            }
        }

        public void Draw(Matrix view, Matrix projection, IMaze maze, int X, int Y, CardinalDirection direction)
        {
            int drawIndex = 0;
            for (int i = squareSideLength; i > 0; i--)
            {
                if (i % 2 == 1)
                {
                    for (int j = 0; j < i; j++)
                    {
                        if (maze.HasWall(X + (j - (i / 2)), Y - (i / 2), CardinalDirection.North)) quads[drawIndex].Draw(view, projection);
                        drawIndex++;
                    }
                    for (int j = 0; j < i; j++)
                    {
                        if (maze.HasWall(X + (i / 2), Y + (j - (i / 2)), CardinalDirection.East)) quads[drawIndex].Draw(view, projection);
                        drawIndex++;
                    }
                    for (int j = 0; j < i; j++)
                    {
                        if (maze.HasWall(X - (j - (i / 2)), Y + (i / 2), CardinalDirection.South)) quads[drawIndex].Draw(view, projection);
                        drawIndex++;
                    }
                    for (int j = 0; j < i; j++)
                    {
                        if (maze.HasWall(X - (i / 2), Y - (j - (i / 2)), CardinalDirection.West)) quads[drawIndex].Draw(view, projection);
                        drawIndex++;
                    }
                }
                else
                {
                    for (int j = 0; j < i; j++)
                    {
                        if (maze.HasWall(X + (j - (i / 2)), Y - (i / 2), CardinalDirection.East)) quads[drawIndex].Draw(view, projection);
                        drawIndex++;
                    }
                    for (int j = 0; j < i; j++)
                    {
                        if (maze.HasWall(X + (i / 2), Y + (j - (i / 2)), CardinalDirection.South)) quads[drawIndex].Draw(view, projection);
                        drawIndex++;
                    }
                    for (int j = 0; j < i; j++)
                    {
                        if (maze.HasWall(X - (j - (i / 2)), Y + (i / 2), CardinalDirection.West)) quads[drawIndex].Draw(view, projection);
                        drawIndex++;
                    }
                    for (int j = 0; j < i; j++)
                    {
                        if (maze.HasWall(X - (i / 2), Y - (j - (i / 2)), CardinalDirection.North)) quads[drawIndex].Draw(view, projection);
                        drawIndex++;
                    }
                }
            }
        }
    }
}
