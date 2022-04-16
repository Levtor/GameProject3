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
            for (int i = squareSideLength; i > 0; i -= 2)
            {
                if (i % 2 == 1)
                {
                    for (int j = 0; j < i; j++)
                    {
                        quads[quadIndex] = new Quad(game,
                            new Vector3((2 * j - i), 1, i),
                            new Vector3((2 + 2 * j - i), 1, i),
                            new Vector3((2 * j - i), -1, i),
                            new Vector3((2 + 2 * j - i), -1, i)
                            );
                        quadIndex++;
                    }
                    for (int j = 0; j < i; j++)
                    {
                        quads[quadIndex] = new Quad(game,
                            new Vector3(i, 1, (2 * j - i)),
                            new Vector3(i, 1, (2 + 2 * j - i)),
                            new Vector3(i, -1, (2 * j - i)),
                            new Vector3(i, -1, (2 + 2 * j - i))
                            );
                        quadIndex++;
                    }
                    for (int j = 0; j < i; j++)
                    {
                        quads[quadIndex] = new Quad(game,
                            new Vector3(-(2 * j - i), 1, -i),
                            new Vector3(-(2 + 2 * j - i), 1, -i),
                            new Vector3(-(2 * j - i), -1, -i),
                            new Vector3(-(2 + 2 * j - i), -1, -i)
                            );
                        quadIndex++;
                    }
                    for (int j = 0; j < i; j++)
                    {
                        quads[quadIndex] = new Quad(game,
                            new Vector3(-i, 1, -(2 * j - i)),
                            new Vector3(-i, 1, -(2 + 2 * j - i)),
                            new Vector3(-i, -1, -(2 * j - i)),
                            new Vector3(-i, -1, -(2 + 2 * j - i))
                            );
                        quadIndex++;
                    }
                }
                else
                {
                    for (int j = 0; j < i; j++)
                    {
                        quads[quadIndex] = new Quad(game,
                            new Vector3((j - (i / 2)), 1, i - 1),
                            new Vector3((j - (i / 2)), 1, i + 1),
                            new Vector3((j - (i / 2)), -1, i - 1),
                            new Vector3((j - (i / 2)), -1, i + 1)
                            );
                        quadIndex++;
                    }
                    for (int j = 0; j < i; j++)
                    {
                        quads[quadIndex] = new Quad(game,
                            new Vector3(i - 1, 1, (j - (i / 2))),
                            new Vector3(i + 1, 1, (j - (i / 2))),
                            new Vector3(i - 1, -1, (j - (i / 2))),
                            new Vector3(i + 1, -1, (j - (i / 2)))
                            );
                        quadIndex++;
                    }
                    for (int j = 0; j < i; j++)
                    {
                        quads[quadIndex] = new Quad(game,
                            new Vector3(-(j - (i / 2)), 1, -i + 1),
                            new Vector3(-(j - (i / 2)), 1, -i - 1),
                            new Vector3(-(j - (i / 2)), -1, -i + 1),
                            new Vector3(-(j - (i / 2)), -1, -i - 1)
                            );
                        quadIndex++;
                    }
                    for (int j = 0; j < i; j++)
                    {
                        quads[quadIndex] = new Quad(game,
                            new Vector3(-i + 1, 1, -(j - (i / 2))),
                            new Vector3(-i - 1, 1, -(j - (i / 2))),
                            new Vector3(-i + 1, -1, -(j - (i / 2))),
                            new Vector3(-i - 1, -1, -(j - (i / 2)))
                            );
                        quadIndex++;
                    }
                }
            }
        }

        public void Draw(Matrix view, Matrix projection, IMaze maze, int X, int Y, CardinalDirection direction)
        {
            int drawIndex = 0;
            int directInt = (4 - (int)direction) % 4;
            int index = 0;
            for (int i = squareSideLength; i > 0; i -= 2)
            {
                if (i % 2 == 1)
                {
                    index = drawIndex + i * (directInt + 0);
                    for (int j = 0; j < i; j++)
                    {
                        if (maze.HasWall(X + (j - (i / 2)), Y - (i / 2), CardinalDirection.North)) quads[index].Draw(view, projection);
                        index++;
                    }
                    index = drawIndex + i * ((directInt + 1) % 4);
                    for (int j = 0; j < i; j++)
                    {
                        if (maze.HasWall(X + (i / 2), Y + (j - (i / 2)), CardinalDirection.East)) quads[index].Draw(view, projection);
                        index++;
                    }
                    index = drawIndex + i * ((directInt + 2) % 4);
                    for (int j = 0; j < i; j++)
                    {
                        if (maze.HasWall(X - (j - (i / 2)), Y + (i / 2), CardinalDirection.South)) quads[index].Draw(view, projection);
                        index++;
                    }
                    index = drawIndex + i * ((directInt + 3) % 4);
                    for (int j = 0; j < i; j++)
                    {
                        if (maze.HasWall(X - (i / 2), Y - (j - (i / 2)), CardinalDirection.East)) quads[index].Draw(view, projection);
                        index++;
                    }
                    drawIndex += 4 * i;
                }
                else
                {
                    index = drawIndex + i * (directInt + 0);
                    for (int j = 0; j < i; j++)
                    {
                        if (maze.HasWall(X + (j - (i / 2)), Y - (i / 2), CardinalDirection.West)) quads[index].Draw(view, projection);
                    }
                    index = drawIndex + i * (directInt + 1);
                    for (int j = 0; j < i; j++)
                    {
                        if (maze.HasWall(X + (i / 2), Y + (j - (i / 2)), CardinalDirection.North)) quads[index].Draw(view, projection);
                    }
                    index = drawIndex + i * (directInt + 2);
                    for (int j = 0; j < i; j++)
                    {
                        if (maze.HasWall(X - (j - (i / 2)), Y + (i / 2), CardinalDirection.East)) quads[index].Draw(view, projection);
                    }
                    index = drawIndex + i * (directInt + 3);
                    for (int j = 0; j < i; j++)
                    {
                        if (maze.HasWall(X - (i / 2), Y - (j - (i / 2)), CardinalDirection.South)) quads[index].Draw(view, projection);
                    }
                    drawIndex += 4 * i;
                }
            }
        }
    }
}
